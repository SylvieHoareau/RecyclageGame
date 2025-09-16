using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Responsable de la boucle temporelle
public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance; // Singleton
    [SerializeField] private Transform playerSpawn; // point de respawn
    private GameObject player;

    [Header("Boucle temporelle")]
    public float loopDuration = 20f; // durée d'une boucle en secondes
    private float loopTimer;

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject); // persiste entre scènes
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        loopTimer = loopDuration;
    }

    // Update is called once per frame
    void Update()
    {
        // Décompte du timer
        loopTimer -= Time.deltaTime;

        if (loopTimer <= 0f)
        {
            RestartLoop();
        }
    }

    public void RestartLoop()
    {
        Debug.Log("Nouvelle boucle");

        // Réinitialise seulement le joueur
        player.transform.position = playerSpawn.position;

        // Pas touche aux autres objets

        loopTimer = loopDuration;

        // Réapplique l’état persistant aux objets de la scène
        PersistentState.Instance.ApplyStateToScene();
    }

    public void EndLevel()
    {
        Debug.Log("Niveau terminé !");
        // Ici, on charge le niveau suivant
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
