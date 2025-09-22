using UnityEngine;

public class MovingPlatformPassenger : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Le joueur devient enfant de la plateforme → il se déplace avec elle
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Le joueur n’est plus enfant → il reprend son mouvement indépendant
            collision.transform.SetParent(null);
        }
    }
}
