using UnityEngine;

public class ArcLaunchProjectileBehaviour : MonoBehaviour, IProjectileLauncher
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float flightTime = 1f;

    public void Fire(Vector2 targetPosition)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        Vector2 displacement = targetPosition - (Vector2)firePoint.position;
        float gravity = Physics2D.gravity.y * rb.gravityScale;

        float velocityX = displacement.x / flightTime;
        float velocityY = (displacement.y - 0.5f * gravity * flightTime * flightTime) / flightTime;

        rb.linearVelocity = new Vector2(velocityX, velocityY);
    }
}