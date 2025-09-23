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

    // Utilise Awake pour initialiser avant que d'autres scripts ne se réveillent.
    void Awake()
    {
        // 1. Vérifiez si la liste est vide. Si c'est le cas, désactivez le script
        // pour éviter les erreurs à l'exécution.
        if (positionList == null || positionList.Count == 0)
        {
            Debug.LogError("La liste des positions est vide ! Le script MovingObject sera désactivé pour éviter les erreurs.");
            enabled = false; // Désactive le script
            return;
        }

        // 2. Si un objet à déplacer n'est pas assigné, utilisez l'objet actuel.
        if (objToMove == null)
        {
            objToMove = this.gameObject;
        }

        // 3. Initialise la cible. C'est maintenant sûr de le faire.
        currentIndex = 0;
        currentTarget = positionList[currentIndex];
    }

     void FixedUpdate()
    {
        // Si la cible actuelle n'est pas définie, on sort.
        if (currentTarget == null) return;

        // Calcule le déplacement et applique le mouvement
        Vector3 moveDir = (currentTarget.position - objToMove.transform.position).normalized;
        Vector3 moveStep = moveDir * speed * Time.fixedDeltaTime;
        objToMove.transform.position += moveStep;

        // Si l'objet est proche de la cible, il passe à la position suivante.
        if (autoMove && Vector3.Distance(objToMove.transform.position, currentTarget.position) < 0.1f)
        {
            MoveToNextPos();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    //     //Set la position et l'index initial a la première valeur de la liste 
    //     if (positionList.Count > 0)
    //     {
    //         currentIndex = 0;
    //         currentTarget = positionList[currentIndex];
    //     }

    // }

    public void MoveToNextPos()
    {
        // Incrémente l'index pour passer à la position suivante
        currentIndex++;

        // Gère la fin de la liste
        if (currentIndex >= positionList.Count)
        {
            if (looping)
            {
                currentIndex = 0; // Revient au début
            }
            else
            {
                // Si pas de looping, on reste sur la dernière position.
                // Tu peux aussi désactiver le mouvement ici.
                Debug.Log("Fin de la trajectoire. Mouvement arrêté.");
                currentTarget = null; // Stoppe le mouvement
                return;
            }
        }

        // Met à jour la cible actuelle
        currentTarget = positionList[currentIndex];
    }
}