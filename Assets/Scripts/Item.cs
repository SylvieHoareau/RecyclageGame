using UnityEngine;
using System;

[Serializable] // IMPORTANT : Rend la classe visible dans l'Inspecteur
public class Item
{
    public string itemName;
    public int quantity;
    public Sprite itemIcon;

    public Item(string name, int qty, Sprite icon)
    {
        itemName = name;
        quantity = qty;
        itemIcon = icon;
    }
}