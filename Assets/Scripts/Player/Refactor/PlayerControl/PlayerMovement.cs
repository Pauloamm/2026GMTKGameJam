using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private float horizontalDirectionalForce =6f; // default 6f

    private Rigidbody2D rb;
    private Vector2 facingScale = Vector2.one;

    private bool isMoving;
    public event Action<bool> OnMovementStateChanged;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 moveInput = inputReader.MoveInput;

        if (moveInput != Vector2.zero)
        {
            rb.linearVelocityX = moveInput.x * horizontalDirectionalForce;
            UpdateFacing(moveInput.x);
            SetIsMoving(true);
        }
        else
        {
            rb.linearVelocityX = 0f;
            SetIsMoving(false);
        }
    }

    private void UpdateFacing(float direction)
    {
        facingScale.x = direction;
        transform.localScale = facingScale;
    }

    private void SetIsMoving(bool value)
    {
        if (isMoving == value) return;

        isMoving = value;
        OnMovementStateChanged?.Invoke(isMoving);
    }
}