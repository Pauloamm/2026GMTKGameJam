using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParrySystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ShieldManager shieldManager;
    private enum ParryState
    {
        Idle,
        Parrying,
        Recovery
    }

    [Header("Input")]
    [SerializeField] private InputActionReference parryActionRef;
    private bool hasParryBeenPressedThisFrame;

    [Header("Timing (seconds)")]
    [SerializeField] private float parryWindowDuration = 0.2f;
    [SerializeField] private float parryRecoveryDuration = 0.4f;

    private ParryState currentState = ParryState.Idle;

    public bool IsParryWindowActive => currentState == ParryState.Parrying;

    private void Awake()
    {
        parryActionRef.action.started += OnParryActionTriggered;
    }

    private void OnParryActionTriggered(InputAction.CallbackContext context)
    {
        Debug.Log(shieldManager.IsShieldHeld);
        if (!shieldManager.IsShieldHeld)
        {
            Debug.Log("Parry ignored, shield is not in hand");
            return;
        }

        if (currentState != ParryState.Idle)
        {
            Debug.Log($"Parry input ignored, currently in {currentState}");
            return;
        }

        hasParryBeenPressedThisFrame = true;
    }

    private void FixedUpdate()
    {
        if (hasParryBeenPressedThisFrame)
        {
            StartCoroutine(ParryRoutine());
        }

        hasParryBeenPressedThisFrame = false;
    }

    private IEnumerator ParryRoutine()
    {
        currentState = ParryState.Parrying;
        Debug.Log("Parry state: Parrying (window active)");

        yield return new WaitForSeconds(parryWindowDuration);

        currentState = ParryState.Recovery;
        Debug.Log("Parry state: Recovery");

        yield return new WaitForSeconds(parryRecoveryDuration);

        currentState = ParryState.Idle;
        Debug.Log("Parry state: Idle (ready)");
    }
}