using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AttackHitbox : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private int damage = 1;
    [SerializeField] private bool isParryable;

    [SerializeField] private ColliderIgnoreList ignoreList;

    public event Action OnParried;
    public event Action<Collider2D> OnHitboxTriggered;

    public bool IsParryable
    {
        get => isParryable;
        set => isParryable = value;
    }

    public void IgnoreColliders(IEnumerable<Collider2D> colliders) => ignoreList.Add(colliders);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ignoreList.Contains(other)) return;

        if (!other.CompareTag("Player"))
        {
            if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }

            OnHitboxTriggered?.Invoke(other);
            return;
        }

        PlayerParrySystem playerParrySystem = other.GetComponentInChildren<PlayerParrySystem>();
        ShieldCountdownExplosionManager shieldExplosionManager = other.GetComponentInChildren<ShieldCountdownExplosionManager>();
        IDamageable playerLifeSystem = other.GetComponentInChildren<IDamageable>();

        if (isParryable && playerParrySystem.IsParryWindowActive)
        {
            shieldExplosionManager.OnParrySuccess();
            OnParried?.Invoke();
        }
        else
        {
            playerLifeSystem.TakeDamage(damage);
        }

        OnHitboxTriggered?.Invoke(other);
    }
}