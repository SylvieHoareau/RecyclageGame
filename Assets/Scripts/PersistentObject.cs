using UnityEngine;

/// <summary>
/// Un composant pour les objets qui doivent persister entre les boucles.
/// </summary>
public class PersistentObject : MonoBehaviour
{
    private PersistentID persistentID;

    private void Awake()
    {
        persistentID = GetComponent<PersistentID>();
        if (persistentID == null)
        {
            Debug.LogError($"L'objet {gameObject.name} n'a pas de PersistentID !", this);
        }
    }

    /// <summary>
    /// Cette méthode est appelée par le PersistentState pour sauvegarder l'état de cet objet.
    /// </summary>
    public void SaveState()
    {
        // On vérifie si l'objet a bougé, s'il a été étiré, ou s'il a été détruit
        // Si c'est le cas, on appelle les méthodes du PersistentState pour les sauvegarder.

        // Sauvegarde de la position si l'objet a un Rigidbody2D et a bougé
        // On peut vérifier l'état de l'objet ou s'il a bougé
        if (GetComponent<Rigidbody2D>() != null)
        {
            // On peut ajouter une logique pour ne sauvegarder la position que si l'objet a réellement bougé
            PersistentState.Instance.SavePosition(persistentID.GUID, transform.position);
        }

        // Sauvegarde de la taille si l'objet a été étiré
        // Exemple : on ne sauve la taille que si elle a changé par rapport à la taille d'origine
        // Vous aurez besoin d'un script qui gère le stretching pour appeler cette méthode au bon moment.
        // PersistentState.Instance.SaveStretch(persistentID.GUID, transform.localScale);
    }
}