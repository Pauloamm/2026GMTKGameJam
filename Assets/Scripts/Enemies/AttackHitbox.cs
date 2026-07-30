using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class AttackHitbox : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private int damage = 1;
    [SerializeField] private bool isParryable;


    public UnityEvent OnParried;

    public bool IsParryable
    {
        get => isParryable;
        set => isParryable = value;
    }

    [SerializeField] private ColliderIgnoreList ignoreList;

    public void IgnoreColliders(IEnumerable<Collider2D> colliders) => ignoreList.Add(colliders);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ignoreList.Contains(other)) return;

        // if it doesnt hit a player at least check if it deals damage
        if (!other.CompareTag("Player"))
        {

            if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);

            }

            return;
        }

        //If it is player get necessary components
        PlayerParrySystem playerParrySystem = other.GetComponentInChildren<PlayerParrySystem>();
        ShieldCountdownExplosionManager shieldExplosionManager = other.GetComponentInChildren<ShieldCountdownExplosionManager>();
        IDamageable playerLifeSsystem = other.GetComponentInChildren<IDamageable>();


        if (isParryable && playerParrySystem.IsParryWindowActive)
        {
            shieldExplosionManager.OnParrySuccess();
            OnParried?.Invoke();
        }
        else playerLifeSsystem.TakeDamage(damage);
    }
}