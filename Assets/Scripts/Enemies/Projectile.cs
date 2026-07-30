using System;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Lifetime")]
    [SerializeField] private float lifetime = 5f;

    [Header("Destroy Conditions")]
    [SerializeField] private LayerMask destroyOnLayers;

    [Header("Rotation")]
    [SerializeField] private bool rotateToFaceDirection;

    [Header("Hitbox")]
    [SerializeField] private AttackHitbox attackHitbox;

    public event Action OnDestroyed;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        attackHitbox.OnHitboxTriggered += HandleHitboxTriggered;
    }

    private void Start()
    {
        Invoke(nameof(DestroyProjectile), lifetime);
    }

    private void Update()
    {
        if (rotateToFaceDirection && rb.linearVelocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg + 180f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
    public void IgnoreColliders(IEnumerable<Collider2D> colliders) => attackHitbox.IgnoreColliders(colliders);
    private void HandleHitboxTriggered(Collider2D other)
    {
        bool hitPlayer = other.CompareTag("Player");
        bool hitEnvironment = (destroyOnLayers.value & (1 << other.gameObject.layer)) != 0;

        if (hitPlayer || hitEnvironment)
        {
            DestroyProjectile();
        }
    }

    private void DestroyProjectile()
    {
        CancelInvoke(nameof(DestroyProjectile));
        OnDestroyed?.Invoke();
        Destroy(gameObject);
    }
}