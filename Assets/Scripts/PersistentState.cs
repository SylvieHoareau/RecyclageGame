using UnityEngine;
using System.Collections.Generic;
using System.Linq; // Nécessaire pour les requêtes LINQ

public class PersistentState : MonoBehaviour
{
    public static PersistentState Instance { get; private set; }

    // Liste des objets détruits
    private HashSet<string> destroyedObjects = new HashSet<string>();

    // Dictionnaire des scales des objets stretchés
    private Dictionary<string, Vector3> stretchedObjects = new Dictionary<string, Vector3>();

    // Dictionnaire des positions des objets déplacés
    private Dictionary<string, Vector3> movedObjects = new Dictionary<string, Vector3>();


    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            // Empêche d'avoir 2 Persistent State dans la scène
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Réinitialise toutes les données de persistance.
    /// </summary>
    public void ClearState()
    {
        // Efface tous les éléments des collections de persistance.
        // C'est le moyen le plus simple et efficace de réinitialiser l'état.
        destroyedObjects.Clear();
        stretchedObjects.Clear();
        movedObjects.Clear();

        Debug.Log("L'état persistant a été réinitialisé.");
    }

    /// <summary>
    /// Enregistre l'état actuel de tous les objets persistants dans la scène.
    /// </summary>
     public void SaveCurrentState()
    {
        // 1. On trouve tous les objets de la scène qui ont le composant PersistentObject
        PersistentObject[] persistentObjects = FindObjectsOfType<PersistentObject>();

        // 2. Pour chaque objet, on lui demande de sauvegarder son état
        foreach (var obj in persistentObjects)
        {
            obj.SaveState();
        }

        Debug.Log($"État de {persistentObjects.Length} objets sauvegardé.");
    }

    // ============================
    // Méthodes pour gérer la persistance
    // Ces méthodes sont appelées par les objets eux-mêmes.
    // ============================
    
     public void SavePosition(string guid, Vector3 newPos)
    {
        movedObjects[guid] = newPos;
    }

    public Vector3? GetSavedPosition(string guid)
    {
        if (movedObjects.TryGetValue(guid, out Vector3 pos))
        {
            return pos;
        }
        return null;
    }

    public void SaveStretch(string guid, Vector3 newScale)
    {
        stretchedObjects[guid] = newScale;
    }

    public Vector3? GetStretch(string guid)
    {
        if (stretchedObjects.TryGetValue(guid, out Vector3 scale))
        {
            return scale;
        }
        return null;
    }

    public void MarkAsDestroyed(string guid)
    {
        destroyedObjects.Add(guid);
        Debug.Log($"Objet '{guid}' marqué comme détruit.");
    }

    public bool IsDestroyed(string guid)
    {
        return destroyedObjects.Contains(guid);
    }

    /// <summary>
    /// Applique les états sauvegardés aux objets de la scène.
    /// </summary>
    public void ApplyStateToScene()
    {
        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            PersistentID id = obj.GetComponent<PersistentID>();
            if (id == null) continue;

            string objID = id.GUID;

            // Vérifie destruction
            if (IsDestroyed(objID))
            {
                Destroy(obj);
                continue;
            }

            // Vérifie le stretching
            Vector3? savedScale = GetStretch(objID);
            if (savedScale.HasValue)
            {
                obj.transform.localScale = savedScale.Value;
            }

            // Vérifie la position
            Vector3? savedPos = GetSavedPosition(objID);
            if (savedPos.HasValue)
            {
                obj.transform.position = savedPos.Value;
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
