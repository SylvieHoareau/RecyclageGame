using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private List<Transform> positionList;
    [SerializeField] private GameObject objToMove;
    [SerializeField] private float speed = 5;
    [SerializeField] private bool autoMove = true;
    [SerializeField] private bool looping = true;

    private int currentIndex = 0;
    private Transform currentTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set la position et l'index initial a la première valeur de la liste 
        if (positionList.Count > 0)
        {
            currentIndex = 0;
            currentTarget = positionList[currentIndex];
        }

    }

    // Fixed update est appelé a chaque update du moteur physique. Il est preférable de modifier a ce moment les positions des objects physiques
    void FixedUpdate()
    {
        // Calcule de l'avancer que vas faire l'object durran cette Fixed frame
        Vector3 moveDir = (currentTarget.position - objToMove.transform.position).normalized; // On normalise la direction pour que peut importe la distance entre l'objet et la target la vitesse reste identique 
        Vector3 moveStep = moveDir * speed * Time.fixedDeltaTime; // Ajoute la speed a la direction
        objToMove.transform.position += moveStep;

        //Si automove et activer et que la distance entre la target et l'objet est infferieur a 0.1, Passer a la position suivante
        if (autoMove && (objToMove.transform.position - currentTarget.position).magnitude < 0.1f)
        {
            MoveToNextPos();
        }
    }

    public void MoveToNextPos()
    {
        // Augmente l'index (équivament a faire currentIndex = currentIndex + 1)
        currentIndex++;

        // Si looping, fait bouclé l'index a 0 si il est superieur a la taille de la liste
        if (looping)
        {
            if (currentIndex > positionList.Count)
            {
                currentIndex = 0;
            }
        }

        // Change la target
        if (currentIndex < positionList.Count)
        {
            currentTarget = positionList[currentIndex];
        }

    }


}