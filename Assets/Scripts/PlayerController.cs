using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerActions inputActions;
    private Animator animator; // Ajout d'une référence à l'Animator

    private Vector2 moveInput;

    [SerializeField] private float speed = 10;
    [SerializeField] private float acceleration = 10;

    // Appeler en premier avant meme le Start()
    void Awake()
    {
        // Recupere la réference au rigidbody et désactive la gravité
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Récupère le composant Animator

        rb.gravityScale = 0;

        // Crée un nouveau Player actions, pour pourvoir récuperer les inputs du joueurs
        inputActions = new PlayerActions();
        inputActions.Enable();

        LinkActions();
    }

    void LinkActions()
    {
        // Lie les actions du player input au fonctions correspondantes 
        inputActions.PlayerInput.Move.started += OnMove; //started correspond a quand le joueur apuis sur le bouton
        inputActions.PlayerInput.Move.performed += OnMove; //performed correspond a quand la valeur change (surtout utile avec les input de joystick)
        inputActions.PlayerInput.Move.canceled += OnMove; //canceled correspond a quand le joueur relache le bouton
    }

    // Fixed update est appelé a chaque update du moteur physique. Il est preférable de modifier a ce moment les positions des objects physiques
    void FixedUpdate()
    {
        // Calcule de la nouvelle vélocité en fonction de input du joueur
        Vector2 newVelocity = Vector2.Lerp(rb.linearVelocity, moveInput * speed, Time.fixedDeltaTime * acceleration);
        rb.linearVelocity = newVelocity;
    }

    void Update()
    {
        // On utilise Update pour les mises à jour de l'Animator car elles ne sont pas liées à la physique
        // et sont donc plus fluides si elles sont exécutées à chaque frame.
        
        // Gère la direction du sprite en fonction de l'input
        if (moveInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        // 1. On vérifie si la référence `animator` n'est pas nulle.
        // 2. Si elle existe, on met à jour la vitesse de l'animation.
        //    Sinon, on ne fait rien, ce qui évite l'erreur.
        if (animator != null)
        {
            // `moveInput.magnitude` donne la longueur du vecteur, parfaite pour la vitesse.
            animator.SetFloat("MoveSpeed", moveInput.magnitude);
        }

        // Met à jour les paramètres de l'Animator pour le Blend Tree
        animator.SetFloat("Horizontal", moveInput.x);
        animator.SetFloat("Vertical", moveInput.y);
    }
}
