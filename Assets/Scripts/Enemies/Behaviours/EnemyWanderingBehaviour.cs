using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyWanderingBehaviour : MonoBehaviour
{
    [Header("Wander Points")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Wander Settings")]
    [SerializeField] private float wanderSpeed = 1.5f;
    [SerializeField] private float arrivalThreshold = 0.1f;

    private Rigidbody2D rb;
    private Transform currentTarget;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTarget = pointB;
    }

    private void FixedUpdate()
    {

        if (Vector2.Distance(transform.position, currentTarget.position) <= arrivalThreshold)
        {
            currentTarget = currentTarget == pointA ? pointB : pointA;
        }

        float direction = Mathf.Sign(currentTarget.position.x - transform.position.x);
        rb.linearVelocityX = wanderSpeed * direction;

        FaceDirection(direction);
    }

    private void FaceDirection(float direction)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    private void OnDrawGizmos()
    {
        if (pointA == null || pointB == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(pointA.position, pointB.position);
    }
}