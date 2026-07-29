using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{

    [Header("Input Action References")]
    [SerializeField] private InputActionAsset gameplayInputMapRef;
    [SerializeField] private InputActionReference moveActionRef;
    [SerializeField] private InputActionReference jumpActionRef;
    [SerializeField] private InputActionReference parryActionRef;

    

    private Vector2 moveInput;
    public Vector2 MoveInput => moveInput;

    private bool isJumpHeld;
    public bool IsJumpHeld => isJumpHeld;

    public event Action JumpPressed;
    public event Action ParryPressed;

    private void Awake()
    {
        gameplayInputMapRef.Enable();

        moveActionRef.action.performed += OnMovePerformed;
        moveActionRef.action.canceled += OnMoveCanceled;

        jumpActionRef.action.started += OnJumpStarted;
        jumpActionRef.action.canceled += OnJumpCanceled;

        parryActionRef.action.started += OnParryStarted;
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }

    private void OnJumpStarted(InputAction.CallbackContext context)
    {
        if (isJumpHeld) return;

        isJumpHeld = true;
        JumpPressed?.Invoke();
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        isJumpHeld = false;
    }

    private void OnParryStarted(InputAction.CallbackContext context)
    {
        ParryPressed?.Invoke();
    }
}