using UnityEngine;

public class StrechingObject : MonoBehaviour
{
    public Vector3 nextScale = Vector3.one;
    public float speed = 2;

    private Vector3 targetScale = Vector3.one;

    private BoxCollider2D boxCollider;
    private Vector2 originalColliderSize;
    [SerializeField] private Collider2D waterCollider; // le collider de l'eau sous le pont

    // Ajout d'une référence au PersistentID de cet objet.
    private PersistentID persistentID;

    void Awake()
    {
        // Récupère le composant BoxCollider2D au démarrage
        boxCollider = GetComponent<BoxCollider2D>();
        // Stocke la taille originale du collider au démarrage
        if (boxCollider != null)
        {
            originalColliderSize = boxCollider.size;
        }

         // Récupère la référence au PersistentID.
        // C'est crucial pour identifier cet objet de manière unique.
        persistentID = GetComponent<PersistentID>();
        if (persistentID == null)
        {
            Debug.LogError("StretchingObject a besoin d'un composant PersistentID pour fonctionner.", this);
            enabled = false; // Désactive le script s'il n'y a pas d'ID persistant
            return;
        }

        // Applique l'état sauvegardé si la scène vient d'être rechargée.
        // Ceci est le complément de la sauvegarde, il applique l'état au démarrage de la scène.
        if (PersistentState.Instance != null)
        {
            Vector3? savedScale = PersistentState.Instance.GetStretch(persistentID.GUID);
            if (savedScale.HasValue)
            {
                // Applique la taille sauvegardée et met à jour le collider immédiatement
                transform.localScale = savedScale.Value;
                UpdateCollider();

                // On désactive aussi le collider de l'eau si le pont est déjà étiré
                if (waterCollider != null && savedScale.Value.x > originalColliderSize.x)
                {
                    waterCollider.enabled = false;
                }
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Tant que la scale de l'objet est différents de la target scale, on modifie la scale
        if (targetScale != transform.localScale)
        {
            Vector3 scaleDir = (targetScale - transform.localScale).normalized; // Comme pour le MovingObject on normalie la difference pour garder un mouvement constant
            Vector3 stepscale = scaleDir * speed * Time.deltaTime; // Ajoute la speed a la direction
            transform.localScale += stepscale;

            // Redimensionne le collider en même temps que l'objet
            if (boxCollider != null)
            {
                // L'offset et la taille sont multipliés par l'échelle de l'objet.
                // On peut donc simplement redimensionner le collider ici.
                 // Applique la nouvelle taille en multipliant la taille originale par la scale actuelle
                boxCollider.size = new Vector2(originalColliderSize.x * transform.localScale.x, originalColliderSize.y * transform.localScale.y);

                  // Ajuste l'offset pour maintenir le collider centré
                boxCollider.offset = new Vector2((transform.localScale.x - 1) * 0.5f, 0);
            }
        }
    }

    /// <summary>
    /// Met à jour la taille du collider et son offset en fonction de la scale.
    /// </summary>
    private void UpdateCollider()
    {
        if (boxCollider != null)
        {
            // Applique la nouvelle taille en multipliant la taille originale par la scale actuelle
            boxCollider.size = new Vector2(originalColliderSize.x * transform.localScale.x, originalColliderSize.y * transform.localScale.y);

            // Ajuste l'offset pour maintenir le collider centré
            boxCollider.offset = new Vector2((transform.localScale.x - 1) * 0.5f, 0);
        }
    }

    public void ChangeStretch()
    {
        // Sert à étirer le pont
        targetScale = nextScale;
        // Sauvegarde l'état immédiatement après le changement
        if (PersistentState.Instance != null && persistentID != null)
        {
            PersistentState.Instance.SaveStretch(persistentID.GUID, targetScale);
        }

        // On désactive l'eau quand le pont est ouvert
        if (waterCollider != null)
        {
            waterCollider.enabled = false;
        }
    }
}
