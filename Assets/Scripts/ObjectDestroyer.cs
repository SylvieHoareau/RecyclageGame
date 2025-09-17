using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // On vérifie si l'objet qui entre dans le trigger est le joueur.
        if (collision.gameObject.CompareTag("Player"))
        {
            // Au lieu de détruire le joueur, on demande au GameFlowManager de le réinitialiser.
            GameFlowManager.Instance.RestartLoop();
        }
        else
        {
            // Pour tous les autres objets, on les détruit et on les marque comme détruits pour la persistance.
            PersistentState.Instance.MarkAsDestroyed(collision.gameObject);
            Destroy(collision.gameObject);
        }
    }
}