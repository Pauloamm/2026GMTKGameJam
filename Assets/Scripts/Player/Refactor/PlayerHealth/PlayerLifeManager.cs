using System;
using UnityEngine;

public class PlayerLifeManager : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;

    private bool isInvincible;

    public event Action OnDeath;
    public event Action OnDamaged;
    public event Action<int> OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || currentHealth <= 0) return;

        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth);
        OnDamaged?.Invoke();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
            return;
        }

        StartInvincibility();
    }

    public void SetInvincible(bool value)
    {
        isInvincible = value;
    }

    private void StartInvincibility()
    {
        isInvincible = true;
    }

    private void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    public void ForceDeath()
    {
        currentHealth = 0;
        Die();
    }
}