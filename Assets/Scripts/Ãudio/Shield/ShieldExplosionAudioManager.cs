using UnityEngine;

public class ShieldExplosionAudioManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ShieldCountdownExplosionManager explosionManager;
    [SerializeField] private AudioSource audioSource;

    [Header("Thresholds (seconds remaining)")]
    [SerializeField] private float calmTickThreshold = 5f;
    [SerializeField] private float anxiousTickThreshold = 3f;

    [Header("Tick Clips")]
    [SerializeField] private AudioClip calmTickClip;
    [SerializeField] private AudioClip anxiousTickClip;

    [Header("Explosion Clip")]
    [SerializeField] private AudioClip explosionClip;

    private enum TickState { Silent, Calm, Anxious }
    private TickState currentState = TickState.Silent;

    private void Awake()
    {
        audioSource.loop = true;
        explosionManager.OnCountdownTick+=HandleCountdownTick;
        explosionManager.OnShieldExplode+=HandleExplosion;
    }

    private void HandleCountdownTick(float remainingTime)
    {
        TickState desiredState = GetDesiredState(remainingTime);

        if (desiredState == currentState) return;

        currentState = desiredState;

        switch (currentState)
        {
            case TickState.Silent:
                audioSource.Stop();
                break;

            case TickState.Calm:
                audioSource.clip = calmTickClip;
                audioSource.Play();
                break;

            case TickState.Anxious:
                audioSource.clip = anxiousTickClip;
                audioSource.Play();
                break;
        }
    }

    private TickState GetDesiredState(float remainingTime)
    {
        if (remainingTime <= anxiousTickThreshold) return TickState.Anxious;
        if (remainingTime <= calmTickThreshold) return TickState.Calm;
        return TickState.Silent;
    }

    private void HandleExplosion()
    {
        audioSource.Stop();
        currentState = TickState.Silent;
        audioSource.PlayOneShot(explosionClip);
    }
}