using UnityEngine;

/// <summary>
/// Permet à un objet (le joueur) de monter et de se déplacer avec une plateforme mouvante.
/// Ce script doit être attaché à la plateforme mouvante.
/// </summary>
public class MovingPlatformPassenger : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Le joueur devient enfant de la plateforme en ajustant sa position pour qu'il ne 'saute' pas
            // lors du changement de parent. Cela garantit un mouvement fluide.
            // On le fait seulement si le joueur n'est pas déjà un enfant de la plateforme.            if (collision.transform.parent != transform)
            {
                collision.transform.SetParent(transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // On vérifie si l'objet qui vient de quitter la collision est bien le joueur.
        if (collision.gameObject.CompareTag("Player"))
        {
            // Le joueur n'est plus enfant de la plateforme. Son transform redevient relatif à la scène.
            // On s'assure qu'on ne détache que si le parent est bien cette plateforme pour éviter des bugs.
            if (collision.transform.parent == transform)
            {
                collision.transform.SetParent(null);
            }
        }
    }
}
