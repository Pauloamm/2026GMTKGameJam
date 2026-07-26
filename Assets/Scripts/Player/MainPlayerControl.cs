using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class MainPlayerControl : MonoBehaviour
{

    //Animation Variables
    private Vector2 playerHorizontalOrientation;
    [SerializeField] private Animator playerAnimator;


    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private InputActionAsset gameplayInputMapRef;


  

    [Header("Jump Variables")]
    [SerializeField] private InputActionReference jumpActionRef;
    [SerializeField] private float jumpForce;


    private enum JumpState
    {
        Grounded,
        JumpingNormalGravity,       // stage 1: steps 0 - firstHoldThresholdSteps, normal gravity
        JumpingFloat,  // stage 2: firstHoldThresholdSteps - (first+second), low gravity
        NormalFalling,
        ForcedFalling
    };
    
    private JumpState currentJumpState;
    [SerializeField] private bool isGrounded;
    private bool hasJumpedButtonBeenPressedThisFrame;

    //Float Jump steps variables
    private bool isJumpHeld;
    private int jumpHoldStepCounter;
    [SerializeField] private int firstHoldThresholdSteps = 20;
    [SerializeField] private int secondHoldThresholdSteps =30;

    //Gravity scales
    private const float normalGravityScale = 1f;
    [SerializeField] private float floatyGravityScale = 0.5f;
    [SerializeField] private float forcedCancelGravityScale = 1.5f;






    [Header("Jump Raycast Variables")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckOffset;
    [SerializeField] private float groundCheckDistance = 0.1f;

    //Coyote Time
    [SerializeField] private float coyoteTime = 0.12f;
    private float coyoteTimeCounter;




    [Header("Horizontal Movement")]
    [SerializeField] private InputActionReference moveActionRef;
    [SerializeField] private float horizontalDirectionalForce;
    private Vector2 moveInput;

    void Awake()
    {
        playerHorizontalOrientation = Vector2.one;


        gameplayInputMapRef.Enable();
        jumpActionRef.action.started += OnJumpActionTrigerred;
        jumpActionRef.action.canceled += OnJumpActionReleased;

        moveActionRef.action.performed += OnMovePerforming;
        moveActionRef.action.canceled += OnMoveStopped;

    }

    private void OnMoveStopped(InputAction.CallbackContext ctx)
    {
        //Debug.Log("Moving button stopped");

        moveInput = Vector2.zero;

    }

    private void OnMovePerforming(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();

    }
    private void OnJumpActionTrigerred(InputAction.CallbackContext context)
    {
        if (isJumpHeld) return;// for some reason event trigerring multiple times per click :))



        hasJumpedButtonBeenPressedThisFrame = context.ReadValueAsButton();
        isJumpHeld = true;

    }
    private void OnJumpActionReleased(InputAction.CallbackContext context)
    {
        //if (!isJumpHeld) return;

        isJumpHeld = false;
    }


    private bool CheckIsGrounded()
    {
        Vector2 origin = (Vector2)transform.position + groundCheckOffset;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);
        return hit.collider != null;
    }

    private void FixedUpdate()
    {


        //Horizontal Movement
        if (moveInput != Vector2.zero)
        {

            playerRigidbody.linearVelocityX = (moveInput.x * horizontalDirectionalForce);
            playerHorizontalOrientation.x = moveInput.x;
            this.transform.localScale = playerHorizontalOrientation;
            playerAnimator.SetBool("IsMoving", true);


        }
        else {


            ResetPlayerHorizontalLinearVelocity();
            playerAnimator.SetBool("IsMoving", false);

        }

        //Jump check
        isGrounded = CheckIsGrounded();
        playerAnimator.SetBool("IsGrounded", isGrounded);
        if (isGrounded && this.currentJumpState != JumpState.Grounded)
        {
            currentJumpState = JumpState.Grounded;
            jumpHoldStepCounter = 0;
            LeaveJumpAnimationAnimation();
        }
        ManageCoyoteTimeCounter(isGrounded);

        bool canJumpThisFixedUpdate = this.coyoteTimeCounter > 0 && hasJumpedButtonBeenPressedThisFrame;
        if (canJumpThisFixedUpdate) MakePlayerJump();
        hasJumpedButtonBeenPressedThisFrame = false;

        //Manage Jump Steps
        if(currentJumpState != JumpState.Grounded)
        {
            jumpHoldStepCounter += 1; // Steps-> FixedUpdate Ran how many times
            currentJumpState = GetJumpStateFromSteps();
            ApplyGravityForJumpState(currentJumpState);
            TriggerJumpAnimation();

        }




    }

    //Jump Functions

    private JumpState GetJumpStateFromSteps()
    {
        JumpState nextFrameJumpState = JumpState.NormalFalling; // default

        if (jumpHoldStepCounter > firstHoldThresholdSteps) // passed the max apex -> force fall
            nextFrameJumpState = JumpState.NormalFalling;

        else if (!isJumpHeld) // when button released before the time
             nextFrameJumpState = JumpState.ForcedFalling;

        else if (jumpHoldStepCounter < firstHoldThresholdSteps) // when holding and not pass threshold for float behaviour apex
            nextFrameJumpState = JumpState.JumpingNormalGravity;

        else if (jumpHoldStepCounter < firstHoldThresholdSteps + secondHoldThresholdSteps) // when passed the threshold and before reaching the max gets floaty
            nextFrameJumpState = JumpState.JumpingFloat;


        //Debug.Log("HOLD JUMP STEP COUNTER: " + jumpHoldStepCounter);
        //Debug.Log("JUMP STATE: " + nextFrameJumpState);
        return nextFrameJumpState;
    }

    private void ApplyGravityForJumpState(JumpState currentState)
    {
        switch (currentJumpState)
        {
            case JumpState.NormalFalling:
                playerRigidbody.gravityScale = normalGravityScale;
                break;
                case JumpState.ForcedFalling:
                playerRigidbody.gravityScale = forcedCancelGravityScale;
                break;
            case JumpState.JumpingNormalGravity:
                playerRigidbody.gravityScale = normalGravityScale;
                break;
            case JumpState.JumpingFloat:
                playerRigidbody.gravityScale = floatyGravityScale;
                break;

            default:
                playerRigidbody.gravityScale = normalGravityScale;
                break;




        }



       
    }
    private void ManageCoyoteTimeCounter(bool canResetTimer)
    {
        if (canResetTimer) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= Time.fixedDeltaTime;


    }
    private void ResetPlayerHorizontalLinearVelocity() => playerRigidbody.linearVelocityX = 0f;

    private void MakePlayerJump() {
        playerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        currentJumpState = GetJumpStateFromSteps();
    }

    private void TriggerJumpAnimation() {


        playerAnimator.ResetTrigger("JumpFinished");
        playerAnimator.SetTrigger("JumpStarted");

    }
    
    private void LeaveJumpAnimationAnimation() {

        playerAnimator.ResetTrigger("JumpStarted");
        playerAnimator.SetTrigger("JumpFinished");


    }

    //Debug
    private void OnDrawGizmosSelected()
    {
        Vector2 origin = (Vector2)transform.position + groundCheckOffset;
        Gizmos.color = isGrounded ? Color.cyan : Color.yellow;
        Gizmos.DrawLine(origin, origin + Vector2.down * groundCheckDistance);
    }
}
