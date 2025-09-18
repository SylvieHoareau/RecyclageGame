using UnityEngine;

public class SpawnerObject : MonoBehaviour
{
    [Header("Prefabs à spawn")]
    public GameObject[] flowersPrefabs;       // plusieurs fleurs pour niveau 1
    public GameObject[] collectiblesPrefabs;  // plusieurs collectibles pour niveau 2

    private GameObject[] prefabsToSpawn;      // les prefabs actifs pour cette scène
    public int amount = 1;                    

    void Start()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (sceneName == "Level1")
            prefabsToSpawn = flowersPrefabs;
        else if (sceneName == "Level2")
            prefabsToSpawn = collectiblesPrefabs;
    }

    public void Spawn()
    {
        Debug.Log("Spawn() appelé !");
        for (int i = 0; i < amount; i++)
        {
            if (prefabsToSpawn.Length == 0)
            {
                Debug.LogWarning("Aucun prefab assigné à spawn !");
                return;
            }

            // Choisit un prefab aléatoire dans le tableau
            GameObject prefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];

            Vector3 pos = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            Instantiate(prefab, pos, Quaternion.identity);
            Debug.Log($"✅ Spawn de {prefab.name} en {pos}");
        }
    }
}
