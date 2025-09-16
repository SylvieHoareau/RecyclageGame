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
    private Dictionary<string, Vector3> movedObjects = new Dictionary<string, Vector3>();


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

        PersistentID id = obj.GetComponent<PersistentID>();
        if (id != null)
        {
            destroyedObjects.Add(id.GUID);
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
        PersistentID id = obj.GetComponent<PersistentID>();
        if (id != null)
        {
            stretchedObjects[id.GUID] = newScale;
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
    public void SavePosition(GameObject obj, Vector3 newPos)
    {
        PersistentID id = obj.GetComponent<PersistentID>();
        if (id != null)
        {
            movedObjects[id.GUID] = newPos;
        }
    }

    public Vector3? GetSavedPosition(string objName)
    {
        if (movedObjects.ContainsKey(objName))
        {
            return movedObjects[objName];
        }
        return null;
    }

    //=====================
    // Application des états
    //======================
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
