using UnityEngine;

public class SpawnerTriggerScript : MonoBehaviour
{
    [SerializeField] private SpawnerObject spawner; // assigner dans l'inspecteur

    private bool triggered = false; // pour éviter plusieurs appels

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Identifier les problèmes de collider ou de tag
        Debug.Log($"Trigger détecté avec {other.name}");

        if (triggered) return;

        // Vérifie si c'est le joueur
        if (other.CompareTag("Player"))
        {
            // Identfier les problèmes d'assignation dans SpawnerObject
            Debug.Log("Le joueur est entré dans le trigger !");
            if (spawner != null)
            {
                spawner.Spawn(); // déclenche le spawner
                triggered = true; // une seule fois
            }
            else
            {
                // Identifier si le tableau est vide dans l'inspecteur
                Debug.LogWarning("Spawner non assigné sur SpawnerTrigger !");
            }
        }
    }
}
