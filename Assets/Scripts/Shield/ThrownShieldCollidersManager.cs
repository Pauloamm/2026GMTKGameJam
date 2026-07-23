using UnityEngine;
using UnityEngine.Events;

public class ThrownShieldCollidersManager : MonoBehaviour
{
    [SerializeField] ShieldManager shieldManager;
    [SerializeField] Collider2D shieldCollider;
    [SerializeField] Collider2D shieldTrigger;

    public UnityEvent OnShieldCloseToPlayerWhileRecalling;


    private void Awake()
    {
        ResetShieldCollidersForNextThrow();
        shieldManager.OnShieldStartingRecall.AddListener(PrepareCollidersWhenStartingRecall);

    }
    void PrepareCollidersWhenStartingRecall()
    {
        shieldCollider.enabled = false;
        shieldTrigger.enabled = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        OnShieldCloseToPlayerWhileRecalling.Invoke();
        ResetShieldCollidersForNextThrow();

    }

    void ResetShieldCollidersForNextThrow()
    {
        shieldCollider.enabled = true;
        shieldTrigger.enabled = false;
    }
}
