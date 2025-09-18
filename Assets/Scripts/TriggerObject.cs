using UnityEngine;
using UnityEngine.Events;

public class TriggerObject : MonoBehaviour
{
    [Tooltip("Événement déclenché quand un objet entre dans le trigger")]
    public UnityEvent<GameObject> OnTriggerEnterEvent;

    [Tooltip("Événement déclenché quand un objet sort du trigger")]
    public UnityEvent<GameObject> OnTriggerExitEvent;

    void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnterEvent?.Invoke(collision.gameObject);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerExitEvent?.Invoke(collision.gameObject);
    }
}
