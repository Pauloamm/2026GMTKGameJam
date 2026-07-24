using System;
using UnityEngine;

public class ForwardLaunchProjectileBehaviour : MonoBehaviour, IProjectileLauncher
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float launchSpeed = 6f;

    [SerializeField] private Collider2D enemyColliderToIgnore;
    public void Fire(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)firePoint.position).normalized;


        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetEnemyColliderToIgnore(enemyColliderToIgnore);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * launchSpeed;
    }
}