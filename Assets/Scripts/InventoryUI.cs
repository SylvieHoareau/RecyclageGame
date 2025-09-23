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
                slotUI.UpdateSlot(item.itemIcon, item.itemName);
            }
            else
            {
                Debug.LogError("Le prefab 'itemSlotPrefab' n'a pas le script 'InventorySlotUI' !");
            }
        }
    }
}

