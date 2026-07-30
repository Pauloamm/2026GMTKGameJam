using UnityEngine;

public class ForwardLaunchProjectileBehaviour : MonoBehaviour, IProjectileLauncher
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float launchSpeed = 6f;

    public void Fire(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)firePoint.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        if (projectile.TryGetComponent<Projectile>(out Projectile projectileScript))
        {
            projectileScript.IgnoreColliders(GetComponentsInChildren<Collider2D>());
        }

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * launchSpeed;
    }
}