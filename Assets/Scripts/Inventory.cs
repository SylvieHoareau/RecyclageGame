using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite itemSprite;

    public Item(string name, Sprite sprite)
    {
        itemName = name;
        itemSprite = sprite;
    }
}

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    private List<Item> items = new List<Item>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre les scènes et les boucles
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(Item newItem)
    {
        items.Add(newItem);
        Debug.Log($"[Inventory] Ajouté : {newItem.itemName}");
    }

    public bool HasItem(string itemName)
    {
        return items.Exists(i => i.itemName == itemName);
    }

    public List<Item> GetAllItems()
    {
        return items;
    }

    public void ClearInventory()
    {
        items.Clear();
    }
}
