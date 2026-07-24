using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundMovingHazard : MonoBehaviour
{
    [Header("Lifetime")]
    [SerializeField] private float lifetime = 5f;

    [Header("Destroy Conditions")]
    [SerializeField] private LayerMask destroyOnLayers;

    public UnityEvent OnDestroyed;

    private readonly List<Collider2D> collidersToIgnore = new List<Collider2D>();

    public void IgnoreColliders(IEnumerable<Collider2D> colliders) => collidersToIgnore.AddRange(colliders);

    private void Start()
    {
        Invoke(nameof(DestroyHazard), lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collidersToIgnore.Contains(other)) return;

        bool hitEnvironment = (destroyOnLayers.value & (1 << other.gameObject.layer)) != 0;

        if (hitEnvironment)
        {
            DestroyHazard();
        }
    }

    private void DestroyHazard()
    {
        CancelInvoke(nameof(DestroyHazard));
        OnDestroyed?.Invoke();
        Destroy(gameObject);
    }
}