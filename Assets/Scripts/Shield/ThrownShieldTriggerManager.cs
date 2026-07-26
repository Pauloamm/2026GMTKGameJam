using UnityEngine;
using UnityEngine.Events;

public class ThrownShieldTriggerManager : MonoBehaviour
{
    public UnityEvent OnShieldRicochet;
    public UnityEvent OnShieldCloseToPlayerWhileRecalling;

    private bool isRecalling;

    public void SetRecalling(bool recalling)
    {
        isRecalling = recalling;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Shield trigger entered by: {other.gameObject.name} (tag: {other.tag}, layer: {LayerMask.LayerToName(other.gameObject.layer)})");

        if (other.CompareTag("Player"))
        {
            if (isRecalling)
            {
                isRecalling = false;
                OnShieldCloseToPlayerWhileRecalling?.Invoke();
            }

            return;
        }

        if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
            damageable.TakeDamage(1);

        OnShieldRicochet?.Invoke();
    }
}