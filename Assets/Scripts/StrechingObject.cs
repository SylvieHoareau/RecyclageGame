using UnityEngine;

public class StrechingObject : MonoBehaviour
{
    public Vector3 nextScale = Vector3.one;
    public float speed = 2;

    private Vector3 targetScale = Vector3.one;

    private BoxCollider2D boxCollider;
    private Vector2 originalColliderSize;


    void Awake()
    {
        // Récupère le composant BoxCollider2D au démarrage
        boxCollider = GetComponent<BoxCollider2D>();
        // Stocke la taille originale du collider au démarrage
        if (boxCollider != null)
        {
            originalColliderSize = boxCollider.size;
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

    public void ChangeStretch()
    {
        targetScale = nextScale;
        PersistentState.Instance.SaveStretch(gameObject, targetScale);
    }
}
