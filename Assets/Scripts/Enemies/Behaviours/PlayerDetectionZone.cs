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
        Gizmos.DrawSphere(this.transform.position, this.GetComponent<CircleCollider2D>().radius);
    }
}