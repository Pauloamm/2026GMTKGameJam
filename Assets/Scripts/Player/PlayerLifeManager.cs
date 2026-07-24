using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerLifeManager : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int currentHealth;

    [Header("Invincibility Frames")]
    [SerializeField] private float invincibilityDuration = 1f;
    [SerializeField] private float blinkInterval = 0.1f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool isInvincible;

    public event Action OnDeath;
    public UnityEvent<int> OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);

    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;// || currentHealth <= 0) return;

        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
            return;
        }

        StartCoroutine(InvincibilityRoutine());
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;

        float elapsed = 0f;
        while (elapsed < invincibilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        spriteRenderer.enabled = true;

        isInvincible = false;
    }

    private void Die()
    {
        OnDeath?.Invoke();

        Destroy(this.gameObject);// destroy player for now, maybe animation or soemthing later
    }
    public void ForceDeath()
    {
        //if (currentHealth <= 0) return;

        currentHealth = 0;
        Die();
    }

}