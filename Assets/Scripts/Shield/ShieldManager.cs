using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using UnityEngine.Events;

public class ShieldManager : MonoBehaviour
{

    //EVENTS
    public UnityEvent OnShieldStartingRecall;

    private ThrownShieldCollisionManager thrownShieldCollisionManager;

    [SerializeField]private GameObject shieldObject;


    private enum ShieldState
    {
        INHAND,
        THROWN,
        RECALLING
    };
    private ShieldState currentShieldState;
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


    void Awake()
    {
        //Get shield rb
        shieldRigidbody2D = shieldObject.GetComponent<Rigidbody2D>();

        //Setur listener on collision manager
        thrownShieldCollisionManager = shieldObject.GetComponent<ThrownShieldCollisionManager>();
        thrownShieldCollisionManager.OnShieldCloseToPlayerWhileRecalling.AddListener(OnShieldRecalled);
        OnShieldRecalled();// consider a deactivated shield in the begining



        canThrowShieldNextFrame = false;
        canRecallShieldNextFrame = false;
        MakeShieldImobile();

        currentShieldState = ShieldState.INHAND;
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
        currentShieldState = ShieldState.THROWN;

    }

    void ThrowShield(InputAction.CallbackContext context)
    {
        canThrowShieldNextFrame = true;
        SetShieldThrowDirection();

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
        shieldRigidbody2D.linearVelocity = shieldDirectionToMove*shieldThrowVelocity;
        Debug.Log("Shield VELOCITY " + shieldRigidbody2D.linearVelocity.magnitude);
        //Debug.Log("Shield Moving");


    }

    void MakeShieldImobile()
    {
        shieldDirectionToMove = Vector2.zero;
    }

    void OnShieldRecalled()
    {
        //reparent
        shieldObject.transform.parent = playerTransformForParenting;
        shieldObject.transform.localPosition = relativeParentPositionOffset;

        //reset velocity and throw direction
        shieldRigidbody2D.linearVelocity= Vector2.zero;
        shieldDirectionToMove= Vector2.zero;
        //deactivate throw shield, the other is animation
        shieldObject.SetActive(false);
    }
}
