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