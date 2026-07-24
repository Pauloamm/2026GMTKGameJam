using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Collider2D))]
public class PlayerDetectionZone : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    public UnityEvent<Transform> OnPlayerEnter;
    public UnityEvent OnPlayerExit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            
            OnPlayerEnter?.Invoke(other.transform);
            gizmoColor = Color.indianRed;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            OnPlayerExit?.Invoke();
            gizmoColor = Color.lightGreen;
            
        }
            
    }




    private Color gizmoColor;
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        gizmoColor.a = 0.1f;
        Gizmos.color = gizmoColor;

        Collider2D col = GetComponent<Collider2D>();
        if (col == null) return;

        if (col is CircleCollider2D circleCollider)
        {
            Gizmos.DrawSphere(transform.position, circleCollider.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y));
        }
        else if (col is BoxCollider2D boxCollider)
        {
            Vector3 size = new Vector3(
                boxCollider.size.x * transform.lossyScale.x,
                boxCollider.size.y * transform.lossyScale.y,
                0.01f
            );
            Vector3 center = transform.position + (Vector3)(boxCollider.offset * transform.lossyScale);
            Gizmos.DrawCube(center, size);
        }
    }
}