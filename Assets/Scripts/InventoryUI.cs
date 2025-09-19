using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro

public class InventoryUI : MonoBehaviour
{
    // Référence statique pour le Singleton
    public static InventoryUI Instance;
    public Transform contentParent; // le Panel où sont instanciés les items
    public GameObject itemSlotPrefab; // prefab créé à l'étape 1

    void Awake()
    {
        // On s'assure qu'il n'y a qu'une seule instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RefreshInventory()
    {
        // Supprime les anciens slots
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Crée un slot pour chaque item
        foreach (var item in Inventory.Instance.GetAllItems())
        {
            GameObject slotObject = Instantiate(itemSlotPrefab, contentParent);
            InventorySlotUI slotUI = slotObject.GetComponent<InventorySlotUI>();

            // On vérifie que le composant est bien présent
            if (slotUI != null)
            {
                slotUI.UpdateSlot(item.itemSprite, item.itemName);
            }
            else
            {
                Debug.LogError("Le prefab 'itemSlotPrefab' n'a pas le script 'InventorySlotUI' !");
            }

            // Ajout de vérifications pour éviter les erreurs
            // Transform itemNameTransform = slot.transform.Find("ItemName");
            // if (itemNameTransform != null)
            // {
            //     TMP_Text itemNameText = itemNameTransform.GetComponent<TMP_Text>();
            //     if (itemNameText != null)
            //     {
            //         itemNameText.text = item.itemName;
            //     }
            // }

            // Transform itemImageTransform = slot.transform.Find("ItemImage");
            // if (itemImageTransform != null)
            // {
            //     Image itemImage = itemImageTransform.GetComponent<Image>();
            //     if (itemImage != null)
            //     {
            //         itemImage.sprite = item.itemSprite;
            //     }
            // }
        }
    }
}

