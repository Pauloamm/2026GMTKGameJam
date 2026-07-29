using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerJumpController playerJumpController;


    // Hashes for faster lookup since the string never changes(easir to comapre int to int than converting it everytime, small polish)
    private readonly int isMovingHash = Animator.StringToHash("IsMoving");
    private readonly int isGroundedHash = Animator.StringToHash("IsGrounded");
    private readonly int jumpStartedHash = Animator.StringToHash("JumpStarted");
    private readonly int jumpFinishedHash = Animator.StringToHash("JumpFinished");

    private void Awake()
    {
        playerMovement.OnMovementStateChanged += HandleMovementStateChanged;
        playerJumpController.OnGroundedStateChanged += HandleGroundedStateChanged;
        playerJumpController.OnJumpStarted += HandleJumpStarted;
        playerJumpController.OnJumpFinished += HandleJumpFinished;
    }

    private void HandleMovementStateChanged(bool isMoving)
    {
        playerAnimator.SetBool(isMovingHash, isMoving);
    }

    private void HandleGroundedStateChanged(bool isGrounded)
    {
        playerAnimator.SetBool(isGroundedHash, isGrounded);
    }

    private void HandleJumpStarted()
    {
        playerAnimator.ResetTrigger(jumpFinishedHash);
        playerAnimator.SetTrigger(jumpStartedHash);
    }

    private void HandleJumpFinished()
    {
        playerAnimator.ResetTrigger(jumpStartedHash);
        playerAnimator.SetTrigger(jumpFinishedHash);
    }
}