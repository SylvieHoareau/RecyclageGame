using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Sauvegarder l'état de destruction
        PersistentState.Instance.MarkAsDestroyed(collision.gameObject);
        Destroy(collision.gameObject);
    }
}
