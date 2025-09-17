using UnityEngine;

public class SpawnerObject : MonoBehaviour
{

    public GameObject spawnPrefab;

    public void SpawnObject()
    {
        if (spawnPrefab == null) return;

        Instantiate(spawnPrefab, transform.position, transform.rotation);
        
    }
}
