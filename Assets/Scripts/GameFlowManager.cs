// using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Responsable de la boucle temporelle
public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance; // Singleton

    [Header("Player")]
    [SerializeField] private Transform playerSpawn; // point de respawn
    private GameObject player;

    [Header("Progression du Niveau")]
    [SerializeField] private SpawnerObject spawnerToTrigger;
    [SerializeField] private GameObject flowersPrefab; // La fleur que nous allons instancier
    [SerializeField] private float flowersToSpawn = 5; // Nombre de fleurs à faire apparaître

    private bool levelCompleted = false; // Pour éviter de déclencher l'événement plusieurs fois


    // Ajoutez une référence directe au joueur ici
    [SerializeField] private GameObject playerPrefab;
    private GameObject currentPlayerInstance;

    [Header("Boucle temporelle")]
    [Tooltip("Durée d'une boucle en secondes")]
    public float loopDuration = 20f; // durée d'une boucle en secondes
    [HideInInspector] // On masque cette variable dans l'éditeur car elle est mise à jour par le script
    public float loopTimer;

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
        player = GameObject.FindGameObjectWithTag("Player");
        loopTimer = loopDuration;
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

    // Crée une nouvelle instance du joueur et met à jour la référence
    // private void InstantiatePlayer()
    // {
    //     // Détruit l'ancienne instance si elle existe pour éviter les doublons
    //     if (currentPlayerInstance != null)
    //     {
    //         Destroy(currentPlayerInstance);
    //     }

    //     // Instancie le nouveau joueur au point de respawn
    //     currentPlayerInstance = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
    // }

    public void RestartLoop()
    {
        Debug.Log("Nouvelle boucle");

        // Utilise la référence déjà existante pour réinitialiser la position
        // currentPlayerInstance.transform.position = playerSpawn.position;

        // Réinitialise seulement le joueur à sa position de départ
        if (player != null)
        {
            player.transform.position = playerSpawn.position;
        }

        // Pas touche aux autres objets

        loopTimer = loopDuration;

        // Réapplique l’état persistant aux objets de la scène
        PersistentState.Instance.ApplyStateToScene();
    }

    // Méthode pour sauvegarder l'état
    // private void SaveCurrentState()
    // {
    //     // Sauvegarde la position de tous les SlidingObject à la fin de la boucle
    //     // Ceci est une solution simple, mais fonctionne.
    //     var slidingObjects = FindObjectsOfType<SlidingObject>();
    //     foreach (var obj in slidingObjects)
    //     {
    //         PersistentState.Instance.SavePosition(obj.gameObject, obj.transform.position);
    //     }
    // }

    // Méthode pour gérer l'interaction avec le TriggerObject
    public void HandleTrigger(GameObject triggeredObject)
    {
        // 1. On vérifie si l'objet qui est entré dans le trigger est la caisse.
        if (triggeredObject.CompareTag("Crate"))
        {
            // 2. Si c'est la caisse, on trouve le pont et on appelle sa méthode d'étirement.
            //    Assurez-vous que votre pont a bien le script StrechingObject.
            var bridge = FindObjectOfType<StrechingObject>();
            if (bridge != null)
            {
                bridge.ChangeStretch();
            }
            // Ici, vous pourriez aussi gérer d'autres actions, comme un son ou un effet visuel.
        }

        // // On vérifie si l'objet qui est entré dans le trigger est le joueur
        // if (triggeredObject.CompareTag("Player"))
        // {
        //     // Si c'est le joueur, on le réinitialise (déclencher la boucle)
        //     RestartLoop();
        // }
        // else if (triggeredObject.CompareTag("Crate")) // Supposons que votre caisse a le tag "Crate"
        // {
        //     // Si c'est la caisse, on applique la logique de persistance (si nécessaire)
        //     // Par exemple, on peut sauvegarder sa position pour la prochaine boucle
        //     PersistentState.Instance.SavePosition(triggeredObject, triggeredObject.transform.position);

        //     // On peut aussi déclencher l'action du TriggerObject ici,
        //     // comme l'étirement du pont
        //     // triggeredObject.GetComponent<StrechingObject>()?.ChangeStretch();
        //     Debug.Log("Une caisse a activé le déclencheur !");
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
        if (spawnerToTrigger != null && flowersPrefab != null)
        {
            for (int i = 0; i < flowersToSpawn; i++)
            {
                // Ici, on instancie les fleurs. On pourrait ajouter une légère variance
                Vector3 spawnPosition = spawnerToTrigger.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                Instantiate(flowersPrefab, spawnPosition, Quaternion.identity);
            }
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
        // Ici, on charge le niveau suivant
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    

}
