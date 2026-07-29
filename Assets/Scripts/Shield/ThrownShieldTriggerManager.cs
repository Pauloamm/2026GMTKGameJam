using UnityEngine;
using System;

public class ThrownShieldTriggerManager : MonoBehaviour
{
    [SerializeField] private LayerMask layersToIgnore;

    public event Action OnShieldRicochet;
    public event Action OnShieldCloseToPlayerWhileRecalling;

    private bool isRecalling;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip ricochetSound;
    public void SetRecalling(bool recalling)
    {
        isRecalling = recalling;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((layersToIgnore.value & (1 << other.gameObject.layer)) != 0) return;

        if (other.CompareTag("Player"))
        {
            if (isRecalling)
            {
                CatchShield();
            }

            return;
        }

        if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.TakeDamage(1);
        }
        else if (audioSource != null && ricochetSound != null)
        {
            audioSource.PlayOneShot(ricochetSound);
        }

        OnShieldRicochet?.Invoke();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isRecalling && other.CompareTag("Player"))
        {
            CatchShield();
        }
    }

    private void CatchShield()
    {
        isRecalling = false;
        OnShieldCloseToPlayerWhileRecalling?.Invoke();
    }
}