using System.Collections;
using UnityEngine;

public class PlayerInvincibilityEffect : MonoBehaviour
{
    [SerializeField] private PlayerLifeManager playerLifeManager;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float invincibilityDuration = 1f;
    [SerializeField] private float blinkInterval = 0.1f;

    private void Awake()
    {
        playerLifeManager.OnDamaged += HandleDamageTaken;
    }

    private void HandleDamageTaken()
    {
        StartCoroutine(InvincibilityRoutine());
    }

    private IEnumerator InvincibilityRoutine()
    {
        playerLifeManager.SetInvincible(true);

        float elapsed = 0f;
        while (elapsed < invincibilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        spriteRenderer.enabled = true;
        playerLifeManager.SetInvincible(false);
    }
}