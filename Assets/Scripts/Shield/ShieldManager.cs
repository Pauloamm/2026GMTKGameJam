using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using UnityEngine.Events;

public class ShieldManager : MonoBehaviour
{

    //EVENTS
    public UnityEvent OnShieldStartingRecall;

    private ThrownShieldTriggerManager thrownShieldTriggerManager;

    [SerializeField] private GameObject shieldObject;
    public UnityEvent OnShieldThrown;

    private bool isShieldRecalling;
    private Vector2 shieldDirectionToMove;
    private bool canThrowShieldNextFrame;
    private bool canRecallShieldNextFrame;


    [Header("Shield Actions")]
    [SerializeField] private InputActionReference shieldThrowAction;
    [SerializeField] private InputActionReference shieldRecallAction;
    private Vector2 moveInput;

    [Header("Parent Player")]
    [SerializeField] private Transform playerTransformForParenting;
    [SerializeField] private Vector2 relativeParentPositionOffset;


    private Rigidbody2D shieldRigidbody2D;
    [SerializeField] private float shieldThrowVelocity;

    [Header("Animation")]
    [SerializeField] private Animator playerAnimator;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip throwSound;
    [SerializeField] private float throwSoundStartTime = 0.1f;

    //SOURCE OF TRUTH FOR SHIELD HELD
    public bool IsShieldHeld { get; private set; } = true;
    void Awake()
    {
        //Get shield rb
        shieldRigidbody2D = shieldObject.GetComponent<Rigidbody2D>();

        //Setup listener on unified trigger manager
        thrownShieldTriggerManager = shieldObject.GetComponent<ThrownShieldTriggerManager>();
        thrownShieldTriggerManager.OnShieldCloseToPlayerWhileRecalling.AddListener(OnShieldRecalled);
        thrownShieldTriggerManager.OnShieldRicochet.AddListener(InvertShieldDirectionForRicochet);
        OnShieldRecalled();// consider a deactivated shield in the begining

        canThrowShieldNextFrame = false;
        canRecallShieldNextFrame = false;
        MakeShieldImobile();

        shieldObject.transform.SetParent(playerTransformForParenting);


        //Action Setup
        shieldThrowAction.action.started += ThrowShield;
        shieldRecallAction.action.started += RecallShield;
    }


    void ThrowShield(InputAction.CallbackContext context)
    {
        if (!IsShieldHeld) return;

        canThrowShieldNextFrame = true;
        SetShieldThrowDirection();
        IsShieldHeld = false;
        playerAnimator.SetTrigger("ShieldThrown");
        PlayThrowSoundFromOffset();
        OnShieldThrown?.Invoke();
    }

    private void PlayThrowSoundFromOffset()
    {
        audioSource.clip = throwSound;
        audioSource.time = throwSoundStartTime;
        audioSource.Play();
    }

    void RecallShield(InputAction.CallbackContext context)
    {
        if (IsShieldHeld) return;

        canRecallShieldNextFrame = true;
        SetShieldRecalDirection();
    }
    void FixedUpdate()
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
            OnShieldStartingRecall.Invoke();
            isShieldRecalling = true;
            thrownShieldTriggerManager.SetRecalling(true);
            canRecallShieldNextFrame = false;

        }


        MoveShieldInDirection();
    }

    void SetShieldThrowDirection()
    {
        if (playerTransformForParenting == null) return; // when player dies when shield is out

        shieldDirectionToMove.x = playerTransformForParenting.localScale.x;// change only left right

        //Debug.Log($"Facing scale.x = {playerTransformForParenting.localScale.x}, throw direction = {shieldDirectionToMove.x}");

    }

    void SetShieldRecalDirection()
    {
        if (playerTransformForParenting == null) return; // when player dies while shield recalling

        shieldDirectionToMove = (playerTransformForParenting.position - shieldObject.transform.position).normalized;
    }



    void MoveShieldInDirection()
    {
        if (isShieldRecalling) SetShieldRecalDirection();
        shieldRigidbody2D.linearVelocity = shieldDirectionToMove * shieldThrowVelocity;


    }

    void MakeShieldImobile()
    {
        shieldDirectionToMove = Vector2.zero;
    }

    void OnShieldRecalled()
    {
        isShieldRecalling = false;
        IsShieldHeld = true;
        thrownShieldTriggerManager.SetRecalling(false);

        //reparent
        shieldObject.transform.parent = playerTransformForParenting;
        shieldObject.transform.localPosition = relativeParentPositionOffset;

        //reset velocity and throw direction
        shieldRigidbody2D.linearVelocity = Vector2.zero;
        shieldDirectionToMove = Vector2.zero;

        //deactivate throw shield, the other is animation
        shieldObject.SetActive(false);
    }

    void InvertShieldDirectionForRicochet()
    {
        Debug.Log("Shield ricochet triggered, inverting direction");
        shieldDirectionToMove = -shieldDirectionToMove;
    }
}