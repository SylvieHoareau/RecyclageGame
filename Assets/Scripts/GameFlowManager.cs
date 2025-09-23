// using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gère la boucle temporelle, la progression du niveau et la fin de partie.
/// Compatible avec le niveau 1 (caisse/pont) et le niveau 2 (collectibles + UI).
/// </summary>
public class GameFlowManager : MonoBehaviour
{
    // Singleton, pour un accès facile depuis n'importe où
    public static GameFlowManager Instance { get; private set; }

    [Header("Player")]
    [Tooltip("Prefab du joueur à instancier au début du jeu.")]
    [SerializeField] private GameObject playerPrefab;
    private GameObject playerInstance;

    [Header("Progression du Niveau")]
    [Tooltip("Le nom de la prochaine scène à charger.")]
    [SerializeField] private string nextSceneName = "";

    [Header("Transition de niveau")]
    [Tooltip("La balise du point de spawn du joueur dans la scène. Ex: 'PlayerSpawn'.")]
    [SerializeField] private string playerSpawnTag = "PlayerSpawn";

    // Champ statique pour stocker le nom de la scène à recharger
    public static string SceneToReload;

    // --- LIFECYCLE ---
    void Awake()
    {
        // Si aucune instance de GameFlowManager n'existe, on la crée.
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad s'assure que cet objet ne sera pas détruit
            // lors du chargement d'une nouvelle scène.
            DontDestroyOnLoad(gameObject); // persiste entre scènes

            // On s'abonne à l'événement de changement de scène.
            // OnSceneLoaded sera appelé chaque fois qu'une scène est chargée.
            SceneManager.sceneLoaded += OnSceneLoaded; // S'abonne à l'événement de chargement de scène
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Nettoyage de l'événement pour éviter les fuites de mémoire
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

    /// <summary>
    /// Appeler cette méthode lorsque le joueur commence un niveau
    /// </summary>
    public void SetSceneToReload(string sceneName)
    {
        SceneToReload = sceneName;
    }

    /// <summary>
    /// Charge une nouvelle scène par son nom.
    /// </summary>
    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Nom de scène invalide.");
            return;
        }

        // Optionnel : Désactive le joueur pendant le chargement pour éviter les bugs
        if (playerInstance != null)
        {
            playerInstance.SetActive(false);
        }

        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Est appelée à chaque fois qu'une nouvelle scène est chargée.
    /// Gère la création et le repositionnement du joueur.
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Trouve le point de spawn du joueur dans la nouvelle scène
        Transform playerSpawn = GameObject.FindGameObjectWithTag(playerSpawnTag)?.transform;

        if (playerSpawn == null)
        {
            Debug.LogError($"Point de spawn avec le tag '{playerSpawnTag}' introuvable dans la scène '{scene.name}'.");
            return;
        }

        if (playerInstance == null)
        {
            // Cas initial : le joueur n'existe pas encore, on l'instancie.
            playerInstance = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
        }
        else
        {
            // Cas de rechargement : on repositionne l'instance existante.
            playerInstance.transform.position = playerSpawn.position;
            playerInstance.SetActive(true); // Assurez-vous qu'il est actif
        }
    }

    /// <summary>
    /// Gère la fin d'un niveau et la transition vers le suivant.
    /// </summary>
    public void EndLevel(string sceneToLoad)
    {
         if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogWarning("Le nom de la prochaine scène n'est pas défini !");
            return;
        }

        Debug.Log("Niveau terminé. Chargement du prochain niveau : " + sceneToLoad);

        // Nettoyage de l'état persistant si nécessaire
        if (PersistentState.Instance != null)
        {
            PersistentState.Instance.ClearState();
        }

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Le nom de la prochaine scène est vide !");
        }

        LoadScene(sceneToLoad);
    }
    

}
