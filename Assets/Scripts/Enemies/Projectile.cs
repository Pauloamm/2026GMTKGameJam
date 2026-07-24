using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [Header("Lifetime")]
    [SerializeField] private float lifetime = 5f;

    [Header("Destroy Conditions")]
    [SerializeField] private LayerMask destroyOnLayers;

    public UnityEvent OnDestroyed;

    private Collider2D enemyToIgnore;
    public void SetEnemyColliderToIgnore(Collider2D enemyThatShot) => enemyToIgnore = enemyThatShot;

    private void Start()
    {
        Invoke(nameof(DestroyProjectile), lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("O PROJETIL COLIDIU COM" + other.gameObject.name);
        if (other == enemyToIgnore) return;

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