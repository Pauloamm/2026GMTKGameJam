using System;
using UnityEngine;

public class ShieldManager : MonoBehaviour
{
    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private GameObject shieldObject;

    [Header("Parent Player")]
    [SerializeField] private Transform playerTransformForParenting;
    [SerializeField] private Vector2 relativeParentPositionOffset;

    [Header("Movement")]
    [SerializeField] private float shieldThrowVelocity;

    private ThrownShieldTriggerManager thrownShieldTriggerManager;
    private Rigidbody2D shieldRigidbody2D;

    private bool isShieldRecalling;
    private Vector2 shieldDirectionToMove;
    private bool canThrowShieldNextFrame;
    private bool canRecallShieldNextFrame;

    private bool isShieldHeld = true;
    public bool IsShieldHeld => isShieldHeld;

    public event Action OnShieldThrown;
    public event Action OnShieldStartingRecall;
    public event Action OnShieldRecalled;

    private void Awake()
    {
        shieldRigidbody2D = shieldObject.GetComponent<Rigidbody2D>();

        thrownShieldTriggerManager = shieldObject.GetComponent<ThrownShieldTriggerManager>();
        thrownShieldTriggerManager.OnShieldCloseToPlayerWhileRecalling += HandleShieldRecalled;
        thrownShieldTriggerManager.OnShieldRicochet += InvertShieldDirectionForRicochet;
        HandleShieldRecalled(); // consider a deactivated shield in the beginning

        MakeShieldImmobile();

        shieldObject.transform.SetParent(playerTransformForParenting);

        inputReader.ShieldThrowPressed += HandleThrowPressed;
        inputReader.ShieldRecallPressed += HandleRecallPressed;
    }

    private void HandleThrowPressed()
    {
        if (!isShieldHeld) return;

        canThrowShieldNextFrame = true;
        SetShieldThrowDirection();
        isShieldHeld = false;
        OnShieldThrown?.Invoke();
    }

    private void HandleRecallPressed()
    {
        if (isShieldHeld) return;

        canRecallShieldNextFrame = true;
        SetShieldRecallDirection();
    }

    private void FixedUpdate()
    {
        if (canThrowShieldNextFrame)
        {
            shieldObject.transform.localPosition = relativeParentPositionOffset;
            shieldObject.SetActive(true);
            shieldObject.transform.parent = null;
            canThrowShieldNextFrame = false;
        }

        if (canRecallShieldNextFrame)
        {
            OnShieldStartingRecall?.Invoke();
            isShieldRecalling = true;
            thrownShieldTriggerManager.SetRecalling(true);
            canRecallShieldNextFrame = false;
        }

        MoveShieldInDirection();
    }

    private void SetShieldThrowDirection()
    {
        if (playerTransformForParenting == null) return;

        shieldDirectionToMove.x = playerTransformForParenting.localScale.x;
    }

    private void SetShieldRecallDirection()
    {
        if (playerTransformForParenting == null) return;

        shieldDirectionToMove = (playerTransformForParenting.position - shieldObject.transform.position).normalized;
    }

    private void MoveShieldInDirection()
    {
        if (isShieldRecalling) SetShieldRecallDirection();
        shieldRigidbody2D.linearVelocity = shieldDirectionToMove * shieldThrowVelocity;
    }

    private void MakeShieldImmobile()
    {
        shieldDirectionToMove = Vector2.zero;
    }

    private void HandleShieldRecalled()
    {
        isShieldRecalling = false;
        isShieldHeld = true;
        thrownShieldTriggerManager.SetRecalling(false);

        shieldObject.transform.parent = playerTransformForParenting;
        shieldObject.transform.localPosition = relativeParentPositionOffset;

        shieldRigidbody2D.linearVelocity = Vector2.zero;
        shieldDirectionToMove = Vector2.zero;

        shieldObject.SetActive(false);

        OnShieldRecalled?.Invoke();
    }

    private void InvertShieldDirectionForRicochet()
    {
        Debug.Log("Shield ricochet triggered, inverting direction");
        shieldDirectionToMove = -shieldDirectionToMove;
    }
}