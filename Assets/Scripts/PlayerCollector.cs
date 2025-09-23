// Un script attaché au joueur pour gérer la collecte.
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // On vérifie si l'objet avec lequel on est entré en collision a un script Item.
        Item collectedItem = other.GetComponent<Item>();

        // Si l'objet est un Item, on le "collecte".
        if (collectedItem != null)
        {
            // On appelle la méthode Collect() de l'Item
            collectedItem.Collect();
            // On peut ajouter ici l'objet à un inventaire.
            // Inventory.Instance.AddItem(collectedItem.ItemName, collectedItem.Quantity);
        }
    }
}