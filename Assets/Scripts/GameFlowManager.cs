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
    // [SerializeField] private GameObject flowersPrefab; // La fleur que nous allons instancier
    // [SerializeField] private float flowersToSpawn = 5; // Nombre de fleurs à faire apparaître

    // Variables pour les objectifs de collecte
    [Header("Objectifs de collecte")]
    [SerializeField] private string[] requiredFlowers; // Noms des fleurs requises (ex: "FleurRouge", "FleurBleue")
    [SerializeField] private string[] requiredVegetables; // Noms des légumes requis (ex: "Carrot", "Cabbage", "Turnip")

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
        // Cherche le point de spawn
        GameObject spawn = GameObject.FindGameObjectWithTag("PlayerSpawn");
        if (spawn == null)
        {
            Debug.LogError("PlayerSpawn non trouvé ! Assurez-vous que l'objet a le tag 'PlayerSpawn'.");
            return;
        }

        playerSpawn = spawn.transform;

        // Cherche le joueur dans la scène actuelle et le point de spawn
        player = GameObject.FindGameObjectWithTag("Player");

        // Si le joueur n'est pas déjà dans la scène, on l'instancie
        if (player == null)
        {
            if (playerPrefab == null)
            {
                Debug.LogError("PlayerPrefab n'est pas assigné dans l'inspecteur !");
                return;
            }

            // Instancie le joueur à la position de spawn
            player = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
            Debug.Log("Player non trouvé ! Assurez-vous que l'objet a le tag 'Player'.");
        }
        else
        {
            // Si le joueur existe déjà (car il est persistant), on le déplace au nouveau point de spawn.
            player.transform.position = playerSpawn.position;
            Debug.Log("Joueur persistant déplacé au point de spawn de la nouvelle scène.");
        }

        

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

    // Méthode pour vérifier si le joueur a tous les objets requis
    private bool CheckIfObjectivesMet(string[] requiredItems)
    {
        // Si le tableau d'items requis est vide, l'objectif est considéré comme rempli
        if (requiredItems.Length == 0)
        {
            return true;
        }

        // Vérifie si le joueur a tous les items dans son inventaire
        foreach (string itemName in requiredItems)
        {
            if (!Inventory.Instance.HasItem(itemName))
            {
                Debug.Log($"Objectif non atteint : l'item '{itemName}' est manquant.");
                return false; // Manque un item, donc retourne faux
            }
        }

        // Tous les items requis sont présents
        return true;
    }

    // --- TRIGGERS & INTERACTIONS ---
    // Méthode pour gérer l'interaction avec le TriggerObject

    public void HandleCrateTrigger()
    {
        // Trouve l'objet Bridge avec le script StrechingObject
        // Ce code ne sera appelé que via le spawner trigger du Level 1
        var bridge = FindObjectOfType<StrechingObject>();
        if (bridge != null)
        {
            bridge.ChangeStretch();
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

        // Actualise l'UI en utilisant le singleton
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
        // levelCompleted = true;

        Debug.Log("Le joueur a atteint le point de fin de niveau !");

        // Utilise le nom de la scène pour adapter la logique
        string sceneName = SceneManager.GetActiveScene().name;
        string[] objectives = new string[0]; // Tableau vide par défaut

        if (sceneName == "Level1")
        {
            objectives = requiredFlowers;
        }
        else if (sceneName == "Level2")
        {
            objectives = requiredVegetables;
        }

        // Vérifie si les objectifs de collecte sont atteints
        if (CheckIfObjectivesMet(objectives))
        {
            Debug.Log("Objectifs de collecte atteints ! Fin du niveau.");
            levelCompleted = true; // Empêche les appels multiples

            // Fait apparaître des fleurs pour le niveau 1
            if (sceneName == "Level1" && spawnerToTrigger != null)
            {
                spawnerToTrigger.SpawnAll();
            }

            // On appelle la fonction de fin de niveau après un court délai pour laisser l'animation se faire
            Invoke("EndLevel", 2f);
        }
        else
        {
            Debug.LogWarning("Objectifs de collecte non atteints. Le niveau ne peut pas se terminer.");
            // Tu peux ajouter ici des feedbacks pour le joueur, comme un message à l'écran
        }

        // On appelle la fonction de fin de niveau après un court délai pour laisser l'animation des fleurs se faire
        // Invoke("EndLevel", 2f);
    }

    /// <summary>
    /// Charge le niveau suivant.
    /// </summary>
    public void EndLevel()
    {
        Debug.Log("Niveau terminé !");

        // (Optionnel) Reset de l'inventaire pour le prochain niveau
        if (Inventory.Instance == null)
        {
            Debug.LogError("Inventory.Instance est NULL !");
        }
        else
        {
            Inventory.Instance.ClearInventory();
        }

        // Inventory.Instance.ClearInventory();

        // Réinitialise l'état persistant avant de changer de scène pour éviter les bugs
        if (PersistentState.Instance == null)
        {
            Debug.LogError("PersistentState.Instance est NULL !");
        }
        else
        {
            PersistentState.Instance.ClearState();
        }
        // PersistentState.Instance.ClearState();

        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("nextSceneName est vide !");
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
        // SceneManager.LoadScene(nextSceneName);
    }
}
