using UnityEngine;
using System;

[ExecuteAlways] // S’exécute même dans l’éditeur
public class PersistentID : MonoBehaviour
{
    [SerializeField, HideInInspector] private string guid = "";

    public string GUID => guid;

    void Awake()
    {
        // Si l’objet n’a pas encore de GUID, on en génère un
        if (string.IsNullOrEmpty(guid))
        {
            guid = Guid.NewGuid().ToString();
        }
    }
}
