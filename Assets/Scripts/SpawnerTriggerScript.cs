using UnityEngine;

public class SpawnerTriggerScript : MonoBehaviour
{
    [SerializeField] private SpawnerObject spawner; // assigner dans l'inspecteur

    private bool triggered = false; // pour éviter plusieurs appels

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        // Vérifie si c'est le joueur
        if (other.CompareTag("Player"))
        {
            if (spawner != null)
            {
                spawner.Spawn(); // déclenche le spawner
                triggered = true; // une seule fois
            }
            else
            {
                Debug.LogWarning("Spawner non assigné sur SpawnerTrigger !");
            }
        }
    }
}
