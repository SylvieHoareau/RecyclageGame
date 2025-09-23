using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Paramètres de collecte")]
    public string itemName;
    public int quantity = 1;
    public Sprite itemSprite;
    // public bool destroyOnCollect = true;

    // Le code suivant peut être dans un PlayerController
    // ou un script spécifique au joueur qui gère les collisions.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // On trouve le CollectibleManager et on l'informe
            CollectibleManager cm = FindObjectOfType<CollectibleManager>();
            if (cm != null)
            {
                cm.OnItemCollected(this);
            }
            
            // Détruit l'objet collectable après qu'il a été traité.
            Destroy(gameObject);
        }
    }
}
