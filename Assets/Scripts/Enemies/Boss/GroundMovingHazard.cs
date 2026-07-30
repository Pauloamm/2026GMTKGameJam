using System;
using System.Collections.Generic;
using UnityEngine;

public class GroundMovingHazard : MonoBehaviour
{
    [Header("Lifetime")]
    [SerializeField] private float lifetime = 5f;

    [Header("Destroy Conditions")]
    [SerializeField] private LayerMask destroyOnLayers;

    [Header("Hitbox")]
    [SerializeField] private AttackHitbox attackHitbox;

    public event Action OnDestroyed;

    private void Awake()
    {
        attackHitbox.OnHitboxTriggered += HandleHitboxTriggered;
    }

    private void Start()
    {
        Invoke(nameof(DestroyHazard), lifetime);
    }

    private void HandleHitboxTriggered(Collider2D other)
    {
        bool hitEnvironment = (destroyOnLayers.value & (1 << other.gameObject.layer)) != 0;

        if (hitEnvironment)
        {
            DestroyHazard();
        }
    }
    public void IgnoreColliders(IEnumerable<Collider2D> colliders) => attackHitbox.IgnoreColliders(colliders);
    private void DestroyHazard()
    {
        CancelInvoke(nameof(DestroyHazard));
        OnDestroyed?.Invoke();
        Destroy(gameObject);
    }
}