using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Gère la mort/respawn du joueur
            GameFlowManager.Instance.RestartLoop();
        }
        else
        {
            // Gère la destruction d'autres objets, en les marquant pour la persistance
            PersistentState.Instance.MarkAsDestroyed(collision.gameObject);
            Destroy(collision.gameObject);
        }
    }
}
