using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [Header("Lifetime")]
    [SerializeField] private float lifetime = 5f;

    [Header("Destroy Conditions")]
    [SerializeField] private LayerMask destroyOnLayers;

    public UnityEvent OnDestroyed;


    [Header("Rotation")]
    [SerializeField] private bool rotateToFaceDirection;
    private Rigidbody2D rb;


    [SerializeField] private ColliderIgnoreList ignoreList;

    public void IgnoreColliders(IEnumerable<Collider2D> colliders) => ignoreList.Add(colliders);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ignoreList.Contains(other)) return;

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