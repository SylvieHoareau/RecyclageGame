using UnityEngine;
using UnityEngine.UI;
using TMPro; // si tu utilises TextMeshPro

public class InventoryUI : MonoBehaviour
{
    public Transform contentParent; // le Panel où sont instanciés les items
    public GameObject itemSlotPrefab; // prefab créé à l'étape 1

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
            GameObject slot = Instantiate(itemSlotPrefab, contentParent);
            slot.transform.Find("ItemName").GetComponent<TMP_Text>().text = item.itemName;
            slot.transform.Find("ItemImage").GetComponent<Image>().sprite = item.itemSprite;
        }
    }
}
