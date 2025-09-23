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
    public void OnItemCollected(string itemName)
    {
        // Ajoute l'item à l'inventaire persistant
        // (Assumons que vous avez un script Inventory persistant)
        Inventory.Instance.AddItem(new Item(itemName, null)); // Remplacez null par le Sprite de l'objet
        Debug.Log($"Item collecté : {itemName}");

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