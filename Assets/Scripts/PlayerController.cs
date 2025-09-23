using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerActions inputActions;
    private Animator animator; // Ajout d'une référence à l'Animator

    private Vector2 moveInput;

    [SerializeField] private float speed = 10f;
    [SerializeField] private float acceleration = 10f;

    // Appeler en premier avant meme le Start()
    void Awake()
    {
        // Recupere la réference au rigidbody et désactive la gravité
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Récupère le composant Animator

        // Toujours désactiver la gravité pour un mouvement cinématique
        rb.gravityScale = 0;

        // Crée un nouveau Player actions, pour pourvoir récuperer les inputs du joueurs
        inputActions = new PlayerActions();
        inputActions.Enable();

        LinkActions();
    }

    void LinkActions()
    {
        // Lie les actions du player input au fonctions correspondantes 
        // inputActions.PlayerInput.Move.started += OnMove; //started correspond a quand le joueur apuis sur le bouton
        inputActions.PlayerInput.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); //performed correspond a quand la valeur change (surtout utile avec les input de joystick)
        inputActions.PlayerInput.Move.canceled += ctx => moveInput = Vector2.zero; //canceled correspond a quand le joueur relache le bouton
    }

    // Fixed update est appelé a chaque update du moteur physique. Il est preférable de modifier a ce moment les positions des objects physiques
    void FixedUpdate()
    {
        // Calcule de la nouvelle vélocité en fonction de input du joueur
        Vector2 targetVelocity = moveInput * speed;
        Vector2 newVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, Time.fixedDeltaTime * acceleration);
        rb.linearVelocity = newVelocity;
    }

    void Update()
    {
        // On utilise Update pour les mises à jour de l'Animator car elles ne sont pas liées à la physique
        // et sont donc plus fluides si elles sont exécutées à chaque frame.

        // Gère la direction du sprite en fonction de l'input
        // if (moveInput.x > 0)
        // {
        //     transform.localScale = new Vector3(1, 1, 1);
        // }
        // else if (moveInput.x < 0)
        // {
        //     transform.localScale = new Vector3(-1, 1, 1);
        // }
        
        // Met à jour la vitesse de l'Animator pour la transition d'Idle à Move
        animator.SetFloat("MoveSpeed", moveInput.magnitude);

        // Met à jour les paramètres de l'Animator pour le Blend Tree
        // Ces valeurs définissent la direction du mouvement
        animator.SetFloat("Horizontal", moveInput.x);
        animator.SetFloat("Vertical", moveInput.y);
    }

    // void OnMove(InputAction.CallbackContext context)
    // {
    //     moveInput = context.ReadValue<Vector2>();

    //      // Si le mouvement est enclenché
    //     if (moveInput != Vector2.zero)
    //     {
    //         // Mettez à jour les paramètres de direction de l'Animator
    //         animator.SetFloat("Horizontal", moveInput.x);
    //         animator.SetFloat("Vertical", moveInput.y);
    //     }
    // }
}
