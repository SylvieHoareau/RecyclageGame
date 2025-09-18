using UnityEngine;

public class SpawnerObject : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject flowersPrefab;        // Prefab pour niveau 1
    public GameObject collectiblePrefab;    // Prefab pour niveau 2
    private GameObject prefabToSpawn;       // Prefab actif pour cette scène

    [Header("Paramètres du spawn")]
    public int amount = 1;

    void Start()
    {
        // Sélectionne le prefab selon la scène
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (sceneName == "Level1")
            prefabToSpawn = flowersPrefab;
        else if (sceneName == "Level2")
            prefabToSpawn = collectiblePrefab;
    }

    public void Spawn()
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 pos = transform.position + new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f), 0);
            Instantiate(prefabToSpawn, pos, Quaternion.identity);
        }

        Debug.Log($"Spawned {amount} objects at {transform.position} in scene {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
    }
}
