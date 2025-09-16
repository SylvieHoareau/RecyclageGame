using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PersistentID))]
public class PersistentIDEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Récupérer le PersistentID ciblé
        PersistentID pid = (PersistentID)target;

        // Commencer un groupe vertical “safe”
        EditorGUILayout.BeginVertical();
        try
        {
            // Affiche le GUID (readonly)
            EditorGUILayout.LabelField("Persistent GUID", pid.GUID);

            // Tu peux ajouter d'autres infos ici si nécessaire
        }
        finally
        {
            // Toujours fermer le groupe vertical, même en cas d'erreur
            EditorGUILayout.EndVertical();
        }

        // Appelle le GUI par défaut pour le reste de l’Inspector
        DrawDefaultInspector();
    }
}
