// using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gère la boucle temporelle, la progression du niveau et la fin de partie.
/// Compatible avec le niveau 1 (caisse/pont) et le niveau 2 (collectibles + UI).
/// </summary>
public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance; // Singleton

    [Header("Paramètres de scènes")]
    [SerializeField] private string nextSceneName = ""; // Nom de la scène suivante

    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawn; // point de respawn
    private GameObject player;

    [Header("Progression du Niveau")]
    [SerializeField] private SpawnerObject spawnerToTrigger;
    [SerializeField] private GameObject flowersPrefab; // La fleur que nous allons instancier
    [SerializeField] private float flowersToSpawn = 5; // Nombre de fleurs à faire apparaître

    private bool levelCompleted = false; // Pour éviter de déclencher l'événement plusieurs fois

    [Header("Boucle temporelle")]
    [Tooltip("Durée d'une boucle en secondes")]
    public float loopDuration = 20f; // durée d'une boucle en secondes
    [HideInInspector] // On masque cette variable dans l'éditeur car elle est mise à jour par le script
    public float loopTimer;
    [HideInInspector] // Variable publique pour que la LoopBar puisse y accéder
    public int loopCount = 1;

    // Événement appelé à chaque redémarrage de boucle
    public event System.Action OnLoopRestart;

    [Header("UI")]
    [SerializeField] private InventoryUI inventoryUI;

    // --- LIFECYCLE ---
    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persiste entre scènes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeLevel();
        inventoryUI.RefreshInventory(); // Affiche l'inventaire au départ
    }

    void Update()
    {
        HandleLoopTimer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Décompte du timer
        loopTimer -= Time.deltaTime;

        if (loopTimer <= 0f)
        {
            // Appelle la méthode pour sauvegarder l'état
            // SaveCurrentState();
            RestartLoop();
        }
    }
    
      // S'abonne à l'événement de changement de scène
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Se désabonne à l'événement de changement de scène
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // --- INITIALISATION ---

    // Méthode appelée après le chargement d'une nouvelle scène
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Réinitialiser les paramètres pour la nouvelle scène
        InitializeLevel();
        inventoryUI?.RefreshInventory();
    }

    private void InitializeLevel()
    {
        // Cherche le joueur dans la scène actuelle et le point de spawn
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player non trouvé ! Assurez-vous que l'objet a le tag 'Player'.");
            return;
        }

        // Cherche le point de spawn
        GameObject spawn = GameObject.FindGameObjectWithTag("PlayerSpawn");
        if (spawn == null)
        {
            Debug.LogError("PlayerSpawn non trouvé ! Assurez-vous que l'objet a le tag 'PlayerSpawn'.");
            return;
        }

        playerSpawn = spawn.transform;

        // Réinitialise le timer de la boucle
        loopTimer = loopDuration;
    }

    // --- LOOP LOGIC ---
    private void HandleLoopTimer()
    {
        loopTimer -= Time.deltaTime;

        if (loopTimer <= 0f)
        {
            RestartLoop();
        }
    }

    public void RestartLoop()
    {
        Debug.Log($"Nouvelle boucle #{loopCount +1}");

        // Sauvegarde l'état actuel avant de le réinitialiser
        // pour que les changements de la boucle précédente soient mémorisés.
        SaveCurrentState();

        // Appelle la fonction ClearState pour effacer
        // l'état persistant et préparer la prochaine boucle.
        PersistentState.Instance.ClearState();

        // Replace le joueur au spawn
        if (player != null && playerSpawn != null)
        {
            player.transform.position = playerSpawn.position;
        }

        // Reset du timer
        loopTimer = loopDuration;
        loopCount++;

        // Réapplique l’état persistant aux objets de la scène
        PersistentState.Instance.ApplyStateToScene();
        
        // 🔔 Prévenir les autres scripts (UI, son, effets visuels)
        OnLoopRestart?.Invoke();
    }

    // Méthode pour sauvegarder l'état (à la fin de la boucle)
    private void SaveCurrentState()
    {
        // Sauvegarde l'état de tous les objets persistants
        var persistentObjects = FindObjectsOfType<PersistentID>();
        foreach (var obj in persistentObjects)
        {
            PersistentState.Instance.SavePosition(obj.gameObject, obj.transform.position);
            PersistentState.Instance.SaveStretch(obj.gameObject, obj.transform.localScale);
            // etc. si d'autres états sont à sauvegarder
        }
    }

    // --- TRIGGERS & INTERACTIONS ---
    // Méthode pour gérer l'interaction avec le TriggerObject
    public void HandleTrigger(GameObject triggeredObject)
    {
        // . On vérifie si l'objet qui est entré dans le trigger est la caisse.
        if (triggeredObject.CompareTag("Crate"))
        {
            // Trouve l'objet Bridge avec le script StrechingObject.
            var bridge = FindObjectOfType<StrechingObject>();
            if (bridge != null)
            {
                bridge.ChangeStretch();
            }
            // Ici, vous pourriez aussi gérer d'autres actions, comme un son ou un effet visuel.
        }
    }

    // Méthode pour la collecte des objets dans le niveau 2
    public void HandleCollectible(Collectible collectible, GameObject collector)
    {
        // Crée un Item à partir du Collectible
        Item newItem = new Item(collectible.itemName, collectible.itemSprite);

        // Ajout à l'inventaire persistant
        Inventory.Instance.AddItem(newItem);
        Debug.Log($"[GameFlowManager] Collecté : {collectible.itemName}");

        // Ajouter ici des effets de feedback (sons, particules…)
        // Example : PlaySFX("Pickup"); ou VFXManager.SpawnEffect(...)

        // Détruire l'objet si nécessaire
        if (collectible.destroyOnCollect)
        {
            Destroy(collectible.gameObject);
        }

        // Actualise l'UI
        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
        if (inventoryUI != null)
        {
            inventoryUI.RefreshInventory();
        }

        // (Optionnel) progression de niveau
        // Ex : si tu veux que collecter X objets valide un objectif :
        // if (Inventory.Instance.GetAllItems().Count >= requiredCollectibles)
        // {
        //     HandleLevelCompletion();
        // }
    }


    /// <summary>
    /// Gère la logique de fin de niveau.
    /// </summary>
    public void HandleLevelCompletion()
    {
        // On s'assure que la logique ne s'exécute qu'une seule fois
        if (levelCompleted) return;
        levelCompleted = true;

        Debug.Log("Le joueur a atteint le point de fin de niveau !");

        // Fait apparaître des fleurs à la position du spawner
        // if (spawnerToTrigger != null && flowersPrefab != null)
        // {
        //     for (int i = 0; i < flowersToSpawn; i++)
        //     {
        //         // Ici, on instancie les fleurs. On pourrait ajouter une légère variance
        //         Vector3 spawnPosition = spawnerToTrigger.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        //         Instantiate(flowersPrefab, spawnPosition, Quaternion.identity);
        //     }
        // }

        // Fait apparaître tous les prefabs configurés dans le spawner
        if (spawnerToTrigger != null)
        {
            spawnerToTrigger.SpawnAll();
        }

        // On appelle la fonction de fin de niveau après un court délai pour laisser l'animation des fleurs se faire
        Invoke("EndLevel", 2f);
    }

    /// <summary>
    /// Charge le niveau suivant.
    /// </summary>
    public void EndLevel()
    {
        Debug.Log("Niveau terminé !");

        // (Optionnel) Reset de l'inventaire pour le prochain niveau
        Inventory.Instance.ClearInventory();

        // Réinitialise l'état persistant avant de changer de scène pour éviter les bugs
        PersistentState.Instance.ClearState();
        SceneManager.LoadScene(nextSceneName);
    }
}
