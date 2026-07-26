using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class EnemyBase : MonoBehaviour, IDamageable
{

    [Header("Health")]
    [SerializeField] protected int maxHealth = 3;
    protected int currentHealth;

    [Header("Contact Damage")]
    [SerializeField] protected int contactDamage = 1;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;


    public event Action<EnemyBase> OnDeath;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;


    }

    public virtual void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;

        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        OnDeath?.Invoke(this);
        this.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable player) && other.gameObject.CompareTag("Player"))
        {
            
            player.TakeDamage(contactDamage);
        }
    }


    protected void FaceDirection(float direction)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }
}