using UnityEngine;
using System.Collections.Generic;

public class PersistentState : MonoBehaviour
{
    public static PersistentState Instance;

    // Liste des objets détruits
    private HashSet<string> destroyedObjects = new HashSet<string>();

    // Dictionnaire des scales des objets stretchés
    private Dictionary<string, Vector3> stretchedObjects = new Dictionary<string, Vector3>();

    // Dictionnaire des positions des objets déplacés
    private Dictionary<string, Vector3> movedObjets = new Dictionary<string, Vector3>();


    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ============================
    // Gestion de la destruction 
    // ============================
    public void MarkAsDestroyed(GameObject obj)
    {
        if (!string.IsNullOrEmpty(obj.name))
        {
            destroyedObjects.Add(obj.name);
        }
    }

    public bool IsDestroyed(string objName)
    {
        return destroyedObjects.Contains(objName);
    }

    //===========================
    // Gestion du stretching
    //===========================
    public void SaveStretch(GameObject obj, Vector3 newScale)
    {
        if (!string.IsNullOrEmpty(obj.name))
        {
            stretchedObjects[obj.name] = newScale;
        }
    }

    public Vector3? GetStretch(string objName)
    {
        if (stretchedObjects.ContainsKey(objName))
        {
            return stretchedObjects[objName];
        }
        return null;
    }

    // ==========================
    // Gestion du déplacement
    // ==========================
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
