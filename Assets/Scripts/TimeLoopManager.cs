using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gère la boucle temporelle et le rechargement de la scène
/// </summary>
public class TimeLoopManager : MonoBehaviour
{
    // Singleton pour un accès facile
    public static TimeLoopManager Instance { get; private set; }

    [Header("Paramètres de la boucle")]
    [Tooltip("Durée d'une boucle en secondes.")]
    [SerializeField] private float loopDuration = 30f;
    [SerializeField] private int maxLoops = 3;

    private float timeRemaining;
    private int loopCount = 1;

    // Propriétés publiques pour que d'autres scripts puissent les lire
    public float TimeRemaining => timeRemaining;
    public float LoopDuration => loopDuration;
    public int LoopCount => loopCount;

    // Événement pour notifier les autres scripts qu'une boucle a redémarré
    public event System.Action OnLoopRestart;    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

     private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // On réinitialise l'état persistant au début de chaque scène
        // avant le démarrage de la boucle.
        if (PersistentState.Instance != null)
        {
            PersistentState.Instance.ApplyStateToScene();
        }
    }


    void Start()
    {
        // Démarre la première boucle
        StartNewLoop();
    }

    // Update is called once per frame
    void Update()
    {
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            RestartLoop();
        }
    }

     /// <summary>
    /// Réinitialise l'état et prépare la prochaine boucle.
    /// </summary>
    public void StartNewLoop()
    {
        timeRemaining = loopDuration;
        // On notifie les autres scripts, comme l'UI, que la boucle a démarré/redémarré.
        OnLoopRestart?.Invoke();
    }

    /// <summary>
    /// Redémarre la boucle temporelle ou termine la partie si le nombre de boucles est atteint.
    /// </summary>
    public void RestartLoop()
    {
        // Sauvegarde l'état de la scène avant de recharger
        if (PersistentState.Instance != null)
        {
            PersistentState.Instance.SaveCurrentState();
        }
        
        loopCount++;

        // Si on dépasse le nombre max de boucles, c'est Game Over.
        if (loopCount > maxLoops)
        {
            Debug.Log("Game Over : Nombre maximum de boucles atteint.");
            // On peut appeler une méthode du GameFlowManager pour charger une scène de Game Over
            GameFlowManager.Instance.LoadScene("GameOverScene");
        }
        else
        {
            Debug.Log($"Temps écoulé ! Boucle {loopCount}/{maxLoops}.");
            // Sauvegarde et réinitialise l'état avant de recharger
            if (PersistentState.Instance != null)
            {
                // Sauvegardez l'état des objets à la fin de la boucle
                PersistentState.Instance.SaveCurrentState(); // Vous devez ajouter cette méthode
                PersistentState.Instance.ClearState();
            }

            // On demande au GameFlowManager de recharger la scène actuelle
            GameFlowManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
