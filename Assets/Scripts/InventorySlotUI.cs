using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    // Les références à tes composants UI
    public Image itemImage;
    public TMP_Text itemName;

    // Une méthode pour mettre à jour le slot
    public void UpdateSlot(Sprite newSprite, string newName)
    {
        // On s'assure que les références ne sont pas nulles avant de les utiliser
        if (itemImage != null)
        {
            itemImage.sprite = newSprite;
        }

        if (itemName != null)
        {
            itemName.text = newName;
        }
    }
}