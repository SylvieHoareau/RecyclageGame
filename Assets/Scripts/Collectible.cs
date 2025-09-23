using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Paramètres de collecte")]
    public string itemName;
    public Sprite itemSprite;
    public bool destroyOnCollect = true;

    public void Collect(GameObject collector)
    {
        // On trouve le CollectibleManager de la scène et on l'informe
        CollectibleManager cm = FindObjectOfType<CollectibleManager>();
        if (cm != null)
        {
            cm.OnItemCollected(this.itemName); // Assumons que le Collectible a une variable itemName
        }
        
        Destroy(gameObject);
    }
}
