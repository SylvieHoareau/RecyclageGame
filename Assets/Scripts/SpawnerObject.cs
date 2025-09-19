using UnityEngine;

public class SpawnerObject : MonoBehaviour
{
    [Header("Prefabs à spawn (niveau 1)")]
    [SerializeField] private GameObject[] flowersPrefabs;       // plusieurs fleurs pour niveau 1

    [Header("Prefabs à spawn (niveau 2)")]
    [SerializeField] private GameObject[] collectiblesPrefabs;  // plusieurs collectibles pour niveau 2

    private GameObject[] prefabsToSpawn;      // les prefabs actifs pour cette scène
    [SerializeField] private int amount = 1;

    void Start()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

       // Choix automatique selon le niveau
        if (sceneName == "Level1")
        {
            prefabsToSpawn = flowersPrefabs;
        }
        else if (sceneName == "Level2")
        {
            prefabsToSpawn = collectiblesPrefabs;
        }
        else
        {
            prefabsToSpawn = new GameObject[0]; // sécurité
        }
    }

    /// <summary>
    /// Spawn un nombre défini d’objets (aléatoires parmi la liste active).
    /// </summary>
    // public void Spawn()
    // {
    //     if (prefabsToSpawn == null || prefabsToSpawn.Length == 0)
    //     {
    //         Debug.LogWarning("Aucun prefab assigné à spawn !");
    //         return;
    //     }

    //     for (int i = 0; i < amount; i++)
    //     {
    //         GameObject prefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];
    //         Vector3 pos = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
    //         Instantiate(prefab, pos, Quaternion.identity);
    //         Debug.Log($"Spawn de {prefab.name} en {pos}");
    //     }
    // }

    // Pour afficher tous les spawn objects en une seule fois
    public void SpawnAll()
    {
        if (prefabsToSpawn == null || prefabsToSpawn.Length == 0)
        {
            Debug.LogWarning("Aucun prefab assigné à spawn !");
            return;
        }

        // Boucle à travers les prefabs et les instancie
        foreach (GameObject prefab in prefabsToSpawn)
        {
            Vector3 pos = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            // Instancie le prefab et stocke la référence dans une nouvelle variable
            GameObject newObject = Instantiate(prefab, pos, Quaternion.identity);
            
            // Active le nouvel objet
            newObject.SetActive(true);
            Debug.Log($"✅ Spawn de {prefab.name}");
        }
    }

}
