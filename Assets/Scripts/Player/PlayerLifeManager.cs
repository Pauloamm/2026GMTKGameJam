using System;
using System.Collections;
using UnityEngine;

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

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || currentHealth <= 0) return;

        currentHealth -= damage;

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
    }
}