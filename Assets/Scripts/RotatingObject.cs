using UnityEngine;


/// <summary>
/// Gère la rotation constante d'un objet.
/// Peut également appliquer une force au joueur en cas de collision.
/// </summary>
public class RotatingObject : MonoBehaviour
{
    // Vitesse de rotation en degrés par seconde. Visible et modifiable dans l'inspecteur.
    [SerializeField] private float _rotationSpeed = 10f;
    // Force appliquée au joueur lors d'une collision.
    [SerializeField] private float _pushForce = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // La méthode Update est appelée à chaque frame du jeu.
    void Update()
    {
        // On fait tourner l'objet autour de son axe Z.
        // Vector3.forward correspond à l'axe Z, ce qui est parfait pour la 2D.
        // On multiplie par Time.deltaTime pour rendre la rotation fluide et indépendante du framerate.
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
    }

    // Cette méthode est appelée par le moteur physique Unity
    // lorsqu'un objet entre en collision avec le nôtre (2D).
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Appliquer une force au joueur
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Pousse le joueur dans la direction opposée
                Vector2 pushDirection = (playerRb.transform.position - transform.position).normalized;

                // On applique la force instantanément (ForceMode2D.Impulse).
                playerRb.AddForce(pushDirection * _pushForce, ForceMode2D.Impulse); 
            }
        }
    }
}
