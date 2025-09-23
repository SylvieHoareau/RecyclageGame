// Un script attaché au joueur pour gérer la collecte.
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // On cherche le composant Collectible.
        Collectible collectedObject = other.GetComponent<Collectible>();

        // Si l'objet a un script Collectible, on le "collecte".
        if (collectedObject != null)
        {
            // On appelle la méthode Collect() du Collectible
            collectedObject.Collect(gameObject);
        }
    }
}