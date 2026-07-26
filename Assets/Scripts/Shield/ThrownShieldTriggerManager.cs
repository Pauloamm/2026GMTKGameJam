using UnityEngine;
using UnityEngine.Events;

public class ThrownShieldTriggerManager : MonoBehaviour
{
    [SerializeField] private LayerMask layersToIgnore;

    public UnityEvent OnShieldRicochet;
    public UnityEvent OnShieldCloseToPlayerWhileRecalling;

    private bool isRecalling;

    public void SetRecalling(bool recalling)
    {
        isRecalling = recalling;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((layersToIgnore.value & (1 << other.gameObject.layer)) != 0) return;

        if (other.CompareTag("Player"))
        {
            if (isRecalling)
            {
                CatchShield();
            }

            return;
        }

        if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
            damageable.TakeDamage(1);

        OnShieldRicochet?.Invoke();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isRecalling && other.CompareTag("Player"))
        {
            CatchShield();
        }
    }

    private void CatchShield()
    {
        isRecalling = false;
        OnShieldCloseToPlayerWhileRecalling?.Invoke();
    }
}