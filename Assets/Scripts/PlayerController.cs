using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput)), RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // References
    private CharacterController controller;
    private PlayerInput input;
    [SerializeField] private Transform avatar;

    [Header("Jump Variables")]
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float gravity;

    [Header("Crouch Variables")]
    [SerializeField] private float crouchTime;
    private float currentCrouchTime;
    private bool isCrouching;

    // Movement
    private float verticalSpeed;
    private Vector3 velocity;
    private bool isGrounded;

    // Inputs
    private bool jump;
    private bool drop;

    // Input Checks
    private bool canJump;
    private bool canDrop;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();

        input.onActionTriggered += OnAction;
    }

    private void Update()
    {
        // Set Grounded
        isGrounded = verticalSpeed <= 0f && Physics.CheckSphere(transform.position + Vector3.up * controller.radius, controller.radius + 0.01f, LayerMask.GetMask("Solid"));

        // Drop Logic
        if (drop && canDrop)
        {
            canDrop = false;
            verticalSpeed = gravity;
            avatar.localScale = new Vector3(1f, 0.5f, 1f);
            currentCrouchTime = crouchTime;
            isCrouching = true;
        }

        if (isCrouching)
        {
            currentCrouchTime -= Time.deltaTime;

            if (currentCrouchTime <= 0f)
            {
                avatar.localScale = Vector3.one;
                isCrouching = false;
            }
        }

        // Jump Logic
        // Check Grounded
        if (isGrounded)
        {
            // Set Vertical Speed to 0
            verticalSpeed = 0f;

            // Check if should be jumping
            if (jump && canJump)
            {
                // Set jumping
                verticalSpeed = jumpSpeed;
                canJump = false;
            }
        }
        // Otherwise
        else
        {
            // Apply Gravity
            canJump = false;
            verticalSpeed += gravity * Time.deltaTime;
        }

        // Reset Inputs
        if (!jump)
        {
            canJump = true;
        }

        if (!drop)
        {
            canDrop = true;
        }

        // Apply Movement
        velocity.y = verticalSpeed;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnEnable()
    {
        if (input != null)
        {
            input.onActionTriggered += OnAction;
        }
    }

    public void OnDisable()
    {
        input.onActionTriggered -= OnAction;
    }

    public void OnAction(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Jump":
                jump = context.ReadValue<float>() > 0.5f;
                break;
            case "Drop":
                drop = context.ReadValue<float>() > 0.5f;
                break;
        }
    }
}
