using System;
using UnityEngine;

public class DamageSoundEffectController : MonoBehaviour
{

    [Header("Audio References")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;

    [Header("Damageable reference")]
    [SerializeField] private MonoBehaviour damageableSource;
     private IDamageable damageable;


    private void Awake()
    {
        damageable = damageableSource as IDamageable;
        damageable.OnDamaged += HandleDamaged;

    }

    private void HandleDamaged()
    {
        audioSource.PlayOneShot(hitSound);

    }
}