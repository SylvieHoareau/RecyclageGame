using UnityEngine;


public class StrechingObject : MonoBehaviour
{

    public Vector3 nextScale = Vector3.one;
    public float speed = 2;

    private Vector3 targetScale = Vector3.one;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
        }
    }

    public void ChangeStretch()
    {
        targetScale = nextScale;
    }
}
