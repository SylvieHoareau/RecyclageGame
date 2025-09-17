using UnityEngine;
using UnityEngine.Events;

public class BridgeTrigger : MonoBehaviour
{
    // Cet événement sera déclenché lorsque le joueur entre dans le trigger du pont.
    // Vous pouvez y lier la méthode ChangeStretch() du StrechingObject dans l'éditeur.
    public UnityEvent OnPlayerEnter;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifiez si l'objet qui a déclenché le trigger est le joueur.
        if (other.gameObject.CompareTag("Player"))
        {
            // Déclenche l'événement pour étirer le pont.
            // Cela découple la logique du pont de la logique de destruction.
            OnPlayerEnter?.Invoke();
        }
    }
}