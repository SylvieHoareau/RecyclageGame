using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // On vérifie si l'objet qui entre dans le trigger est le joueur.
        if (collision.gameObject.CompareTag("Player"))
        {
            // Le joueur a un comportement spécifique.
            // On délègue la gestion de sa destruction au TimeLoopManager.
            if (TimeLoopManager.Instance != null)
            {
                TimeLoopManager.Instance.RestartLoop();
            }

        }
        else
        {
           // Pour tous les autres objets, on les marque comme détruits pour la persistance.
            if (PersistentState.Instance != null)
            {
                // La méthode MarkAsDestroyed a été mise à jour pour prendre le GUID
                PersistentID id = collision.gameObject.GetComponent<PersistentID>();
                if (id != null)
                {
                    PersistentState.Instance.MarkAsDestroyed(id.GUID);
                }
            }
            // Et on détruit l'objet dans la scène.
            Destroy(collision.gameObject);
        }
    }
}