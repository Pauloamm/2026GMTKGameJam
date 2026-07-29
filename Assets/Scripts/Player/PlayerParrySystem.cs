using System;
using System.Collections;
using UnityEngine;

public class PlayerParrySystem : MonoBehaviour
{
    private enum ParryState
    {
        Idle,
        Parrying,
        Recovery
    }

    [Header("References")]
    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private ShieldManager shieldManager;

    [Header("Timing (seconds)")]
    [SerializeField] private float parryWindowDuration = 0.2f;
    [SerializeField] private float parryRecoveryDuration = 0.4f;

    private bool hasParryBeenPressedThisFrame;
    private ParryState currentState = ParryState.Idle;

    public bool IsParryWindowActive => currentState == ParryState.Parrying;

    public event Action OnParryStarted;
    public event Action OnParryRecoveryStarted;

    private void Awake()
    {
        inputReader.ParryPressed += OnParryPressed;
    }

    private void OnParryPressed()
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
        OnParryStarted?.Invoke();

        yield return new WaitForSeconds(parryWindowDuration);

        currentState = ParryState.Recovery;
        OnParryRecoveryStarted?.Invoke();

        yield return new WaitForSeconds(parryRecoveryDuration);

        currentState = ParryState.Idle;
    }
}