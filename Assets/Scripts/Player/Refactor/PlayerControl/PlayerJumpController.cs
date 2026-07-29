using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerJumpController : MonoBehaviour
{
    private enum JumpState
    {
        Grounded,
        JumpingNormalGravity,
        JumpingFloat,
        NormalFalling,
        ForcedFalling
    }

    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private float jumpForce;

    [Header("Jump Hold Steps")]
    [SerializeField] private int firstHoldThresholdSteps = 20;
    [SerializeField] private int secondHoldThresholdSteps = 30;

    [Header("Gravity Scales")]
    [SerializeField] private float normalGravityScale = 1f;
    [SerializeField] private float floatyGravityScale = 0.5f;
    [SerializeField] private float forcedCancelGravityScale = 1.5f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckOffset;
    [SerializeField] private float groundCheckDistance = 0.1f;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime = 0.12f;

    private Rigidbody2D rb;
    private JumpState currentJumpState;
    private int jumpHoldStepCounter;
    private float coyoteTimeCounter;
    private bool jumpRequested;

    private bool isGrounded;
    public bool IsGrounded => isGrounded;

    public event Action<bool> OnGroundedStateChanged;
    public event Action OnJumpStarted;
    public event Action OnJumpFinished;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputReader.JumpPressed += OnJumpPressed;
    }

    private void OnJumpPressed()
    {
        jumpRequested = true;
    }

    private void FixedUpdate()
    {
        UpdateGroundedState();
        UpdateCoyoteTime();

        if (jumpRequested && coyoteTimeCounter > 0f)
        {
            PerformJump();
        }
        jumpRequested = false;

        if (currentJumpState != JumpState.Grounded)
        {
            jumpHoldStepCounter++;
            currentJumpState = GetJumpStateFromSteps();
            ApplyGravityForJumpState(currentJumpState);
        }
    }

    private void UpdateGroundedState()
    {
        Vector2 origin = (Vector2)transform.position + groundCheckOffset;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);
        bool wasGrounded = isGrounded;
        isGrounded = hit.collider != null;

        if (isGrounded != wasGrounded)
        {
            OnGroundedStateChanged?.Invoke(isGrounded);
        }

        if (isGrounded && currentJumpState != JumpState.Grounded)
        {
            currentJumpState = JumpState.Grounded;
            jumpHoldStepCounter = 0;
            OnJumpFinished?.Invoke();
        }
    }

    private void UpdateCoyoteTime()
    {
        coyoteTimeCounter = isGrounded ? coyoteTime : coyoteTimeCounter - Time.fixedDeltaTime;
    }

    private void PerformJump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        coyoteTimeCounter = 0f;
        jumpHoldStepCounter = 0;
        currentJumpState = GetJumpStateFromSteps();
        OnJumpStarted?.Invoke();
    }

    private JumpState GetJumpStateFromSteps()
    {
        if (jumpHoldStepCounter > firstHoldThresholdSteps)
            return JumpState.NormalFalling;

        if (!inputReader.IsJumpHeld)
            return JumpState.ForcedFalling;

        if (jumpHoldStepCounter < firstHoldThresholdSteps)
            return JumpState.JumpingNormalGravity;

        if (jumpHoldStepCounter < firstHoldThresholdSteps + secondHoldThresholdSteps)
            return JumpState.JumpingFloat;

        return JumpState.NormalFalling;
    }

    private void ApplyGravityForJumpState(JumpState state)
    {

        switch (state) {

            case JumpState.ForcedFalling:
                rb.gravityScale = forcedCancelGravityScale;
                break;

            case JumpState.JumpingFloat:
                rb.gravityScale = floatyGravityScale;
                break;

            default:
                rb.gravityScale = normalGravityScale;
                break;


        }


    }

    private void OnDrawGizmosSelected()
    {
        Vector2 origin = (Vector2)transform.position + groundCheckOffset;
        Gizmos.color = isGrounded ? Color.cyan : Color.yellow;
        Gizmos.DrawLine(origin, origin + Vector2.down * groundCheckDistance);
    }
}