using System;
using UnityEngine;

public class ShieldCountdownExplosionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private ShieldManager shieldManager;

    [Header("Timer")]
    [SerializeField] private float explosionCountdownDuration = 3.5f;
    private float currentCountdown;

    [Header("Damage")]
    [SerializeField] private int baseExplosionDamage = 1;
    [SerializeField] private int currentExplosionDamage = 1;

    [Header("Explosion")]
    [SerializeField] private float explosionRadius = 2f;


    public event Action OnShieldExplode;
    public event Action<float> OnCountdownTick;

    public float TimeUntilExplosion => currentCountdown;


    [Header("VFX")]
    [SerializeField] private ParticleSystem explosionParticles;


    private void Awake()
    {
        ResetCountdown();
        currentExplosionDamage = baseExplosionDamage;

    }


    private void Update()
    {
        
        currentCountdown -= Time.deltaTime;
        OnCountdownTick?.Invoke(currentCountdown);

        if (currentCountdown <= 0f)
        {
            Explode();
        }
    }

    public void OnParrySuccess()
    {
        currentExplosionDamage *= 2;
        ResetCountdown();
    }

    private void Explode()
    {
        
        Vector2 explosionPosition = shieldManager.IsShieldHeld ? playerTransform.position : shieldObject.transform.position;

        //VFX
       explosionParticles.transform.position = explosionPosition;
       explosionParticles.Play();

        Collider2D[] hits = Physics2D.OverlapCircleAll(explosionPosition, explosionRadius);

        foreach (Collider2D hit in hits)
        {

            IDamageable creature = hit.GetComponentInChildren<IDamageable>();
            if (creature != null)
            {
                Debug.Log("Explosion dealt damage to " + hit.gameObject.name);
                creature.TakeDamage(currentExplosionDamage);
            }
                
        }

        OnShieldExplode?.Invoke();

        currentExplosionDamage = baseExplosionDamage;
        ResetCountdown();
    }

    private void ResetCountdown()
    {
        currentCountdown = explosionCountdownDuration;
    }

   //private void OnDrawGizmosSelected()
   //{
   //    Vector2 origin = shieldManager.IsShieldHeld && playerTransform != null
   //        ? playerTransform.position
   //        : (shieldObject != null ? (Vector2)shieldObject.transform.position : Vector2.zero);
   //
   //    Gizmos.color = Color.red;
   //    Gizmos.DrawWireSphere(origin, explosionRadius);
   //}
}