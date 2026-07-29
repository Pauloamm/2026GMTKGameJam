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

    public event Action<EnemyBase> OnDeath;
    public event Action OnDamaged;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        OnDamaged?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        IDamageable player = other.GetComponentInChildren<IDamageable>();
        if (player != null)
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