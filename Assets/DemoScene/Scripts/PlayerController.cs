using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerActions inputActions;

    private Vector2 moveInput;

    [SerializeField] private float speed = 50;
    [SerializeField] private float acceleration = 20;

    // Appeler en premier avant meme le Start()
    void Awake()
    {
        // Recupere la réference au rigidbody et désactive la gravité
        rb = GetComponent<Rigidbody2D>();
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

    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
