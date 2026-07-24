using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using UnityEngine.Events;

public class ShieldManager : MonoBehaviour
{

    //EVENTS
    public UnityEvent OnShieldStartingRecall;

    private ThrownShieldCollidersManager thrownShieldCollisionManager;
    private ThrownShieldRicochetBehaviour thrownShieldRicochetBehaviour;

    [SerializeField]private GameObject shieldObject;
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
    
    //SOURCE OF TRUTH FOR SHIELD HELD
    public bool IsShieldHeld { get; private set; } = true;
    void Awake()
    {
        //Get shield rb
        shieldRigidbody2D = shieldObject.GetComponent<Rigidbody2D>();

        //Setur listener on collision manager
        thrownShieldCollisionManager = shieldObject.GetComponent<ThrownShieldCollidersManager>();
        thrownShieldCollisionManager.OnShieldCloseToPlayerWhileRecalling.AddListener(OnShieldRecalled);
        OnShieldRecalled();// consider a deactivated shield in the begining

        //setup listener for ricochet
        thrownShieldRicochetBehaviour = shieldObject.GetComponent<ThrownShieldRicochetBehaviour>();
        thrownShieldRicochetBehaviour.OnShieldRicochet.AddListener(InvertShieldDirectionForRicochet);

        canThrowShieldNextFrame = false;
        canRecallShieldNextFrame = false;
        MakeShieldImobile();

        shieldObject.transform.SetParent(playerTransformForParenting);


        //Action Setup
        shieldThrowAction.action.started += ThrowShield;
        shieldRecallAction.action.started += RecallShield;
    }



    void GetShieldInHand() // Deactivates throwable shield from view
    {
        shieldObject.SetActive(false);
        shieldObject.transform.SetParent(playerTransformForParenting);
        shieldObject.transform.localPosition = relativeParentPositionOffset;

    }

    void ThrowShield(InputAction.CallbackContext context)
    {
        canThrowShieldNextFrame = true;
        SetShieldThrowDirection();
        IsShieldHeld = false;
        OnShieldThrown?.Invoke();


    }

    void RecallShield(InputAction.CallbackContext context)
    {
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
            canRecallShieldNextFrame = false;

        }


        MoveShieldInDirection();
    }

    void SetShieldThrowDirection()
    {
        shieldDirectionToMove.x = playerTransformForParenting.localScale.x;// change only left right
    }

    void SetShieldRecalDirection()
    {
        shieldDirectionToMove = (playerTransformForParenting.position - shieldObject.transform.position).normalized;
    }



    void MoveShieldInDirection()
    {
        if (isShieldRecalling) SetShieldRecalDirection();
        shieldRigidbody2D.linearVelocity = shieldDirectionToMove*shieldThrowVelocity;


    }

    void MakeShieldImobile()
    {
        shieldDirectionToMove = Vector2.zero;
    }

    void OnShieldRecalled()
    {
        isShieldRecalling = false;
        IsShieldHeld = true;

        //reparent
        shieldObject.transform.parent = playerTransformForParenting;
        shieldObject.transform.localPosition = relativeParentPositionOffset;

        //reset velocity and throw direction
        shieldRigidbody2D.linearVelocity= Vector2.zero;
        shieldDirectionToMove= Vector2.zero;

        //deactivate throw shield, the other is animation
        shieldObject.SetActive(false);
    }

    void InvertShieldDirectionForRicochet()
    {
        shieldDirectionToMove = -shieldDirectionToMove;
    }
}
