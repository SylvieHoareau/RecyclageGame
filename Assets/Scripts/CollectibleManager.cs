using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// Gère les objectifs de collecte et la logique de fin de niveau.
/// </summary>
public class CollectibleManager : MonoBehaviour
{
    // C'est un manager par scène, donc pas de Singleton
    
    [Header("Objectifs de collecte")]
    [SerializeField] private string[] requiredItems;
    [Tooltip("Le nom de la scène suivante à charger.")]
    [SerializeField] private string nextSceneName;

    private void Start()
    {
        // Met à jour l'UI de l'inventaire au début du niveau
        UpdateUI();
    }

     /// <summary>
    /// Gère la collecte d'un objet.
    /// </summary>
    public void OnItemCollected(Collectible collectible)
    {
         // On s'assure que l'inventaire existe
        if (Inventory.Instance != null)
        {
            // Crée un nouvel 'Item' de données et l'ajoute à l'inventaire
            Item newItem = new Item(collectible.itemName, collectible.quantity, collectible.itemSprite);
            Inventory.Instance.AddItem(newItem);
            
            Debug.Log($"Item collecté : {collectible.itemName}");
        }
        else
        {
            Debug.LogError("Inventory.Instance est null. Assurez-vous qu'il existe dans la scène.");
        }
        
        // Actualise l'UI de l'inventaire
        UpdateUI();

        // Vérifie si les objectifs sont atteints après chaque collecte
        if (CheckIfObjectivesMet())
        {
            Debug.Log("Objectifs de collecte atteints ! Préparation pour le niveau suivant.");
            Invoke("LoadNextLevel", 2f); // Délai pour des animations ou effets
        }
    }

    /// <summary>
    /// Vérifie si le joueur a tous les objets requis.
    /// </summary>
    private bool CheckIfObjectivesMet()
    {
        if (requiredItems.Length == 0)
        {
            return true; // Si aucun item n'est requis, l'objectif est toujours atteint
        }

        foreach (string itemName in requiredItems)
        {
            if (!Inventory.Instance.HasItem(itemName))
            {
                return false; // Il manque un item
            }
        }
        return true;
    }

    /// <summary>
    /// Charge la prochaine scène.
    /// </summary>
    private void LoadNextLevel()
    {
        // Assurez-vous que l'inventaire est réinitialisé si nécessaire
        Inventory.Instance.ClearInventory();
        
        // Utilise le GameFlowManager pour passer à la scène suivante
        GameFlowManager.Instance.LoadScene(nextSceneName);
    }

    /// <summary>
    /// Trouve l'InventoryUI dans la scène et la rafraîchit.
    /// </summary>
    private void UpdateUI()
    {
        InventoryUI ui = FindObjectOfType<InventoryUI>();
        if (ui != null)
        {
            ui.RefreshInventory();
        }
    }
}