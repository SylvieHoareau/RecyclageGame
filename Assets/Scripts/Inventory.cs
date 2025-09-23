using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Gère les objets collectés par le joueur.
/// Fonctionne comme un Singleton persistant.
/// </summary>
public class Inventory : MonoBehaviour
{
    // Instance publique statique pour le pattern Singleton
    public static Inventory Instance { get; private set; }

    // La liste d'objets qui constitue l'inventaire
    private List<Item> items = new List<Item>();

    // Événement pour notifier les autres scripts (comme l'UI)
    // qu'un changement a eu lieu.
    public event System.Action OnInventoryChanged;

    void Awake()
    {
        // Implémentation du Singleton
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

    /// <summary>
    /// Ajoute un objet à l'inventaire.
    /// </summary>
    public void AddItem(Item item)
    {
        if (item == null)
        {
            Debug.LogWarning("Tentative d'ajouter un objet null à l'inventaire.");
            return;
        }

        items.Add(item);
        Debug.Log($"[Inventory] Ajouté : {item.itemName}");
        
        // Notifie les abonnés que l'inventaire a changé
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// Retire un objet de l'inventaire par son nom.
    /// </summary>
    public void RemoveItem(string itemName)
    {
        Item itemToRemove = items.FirstOrDefault(item => item.itemName == itemName);
        if (itemToRemove != null)
        {
            items.Remove(itemToRemove);
            Debug.Log($"Objet '{itemName}' retiré de l'inventaire.");
            OnInventoryChanged?.Invoke();
        }
        else
        {
            Debug.LogWarning($"Objet '{itemName}' non trouvé dans l'inventaire.");
        }
    }

    /// <summary>
    /// Vérifie si un objet est dans l'inventaire.
    /// </summary>
    public bool HasItem(string itemName)
    {
        return items.Exists(i => i.itemName == itemName);
    }

    /// <summary>
    /// Retourne la liste complète des objets.
    /// </summary>
    public List<Item> GetAllItems()
    {
        return new List<Item>(items); // Retourne une copie pour éviter des modifications externes
    }

    /// <summary>
    /// Réinitialise l'inventaire.
    /// </summary>
    public void ClearInventory()
    {
        items.Clear();
        Debug.Log("Inventaire réinitialisé.");
        OnInventoryChanged?.Invoke();
    }
}
