using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;

public class Shield : MonoBehaviour
{

    private enum ShieldState
    {
        INHAND,
        THROWN,
        RECALLING
    };
    private ShieldState currentShieldState;
    
    
    [Header("Shield Actions")] 
    [SerializeField] private InputActionReference shieldThrowAction;
    [SerializeField] private InputActionReference shieldRecallAction;
    private Vector2 moveInput; 
    
    [Header("Parent Player")]
    [SerializeField] private Transform playerTransformForParenting;
    private Vector2 relativeParentPosition;
    
    
    private Rigidbody2D shieldRigidbody2D;
    [SerializeField] private float shieldThrowVelocity;
    
    
    void Awake()
    {
        currentShieldState =  ShieldState.INHAND;
        this.transform.SetParent(playerTransformForParenting);
        relativeParentPosition = this.transform.localPosition;
        
        shieldRigidbody2D  = this.GetComponent<Rigidbody2D>();
        
        //Action Setup
        shieldThrowAction.action.started += ThrowShield;
        shieldRecallAction.action.started += RecallShield;

    }



    void GetShieldInHand() // Deactivates throwable shield from view
    {
        this.gameObject.SetActive(false);
        this.transform.SetParent(playerTransformForParenting);
        this.transform.localPosition = relativeParentPosition;
        currentShieldState =  ShieldState.THROWN;

    }
    
    void ThrowShield(InputAction.CallbackContext context)
    {
        this.gameObject.SetActive(true);
        this.transform.parent = null;
        currentShieldState = ShieldState.THROWN;
        

    }

    void RecallShield(InputAction.CallbackContext context)
    {
        currentShieldState = ShieldState.RECALLING;

    }
    void FixedUpdate()
    {
        switch (currentShieldState)
        {
            case ShieldState.THROWN:
                shieldRigidbody2D.linearVelocityX = shieldThrowVelocity;
                Debug.Log("Shield Moving at " + shieldRigidbody2D.linearVelocityX);
                break;
            
                
            
        }
    }
}
