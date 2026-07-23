using UnityEngine;
using UnityEngine.Events;

public class MeleeAttackBehaviour : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float windupDuration = 0.4f;
    [SerializeField] private float activeDuration = 0.2f;
    [SerializeField] private float recoveryDuration = 0.5f;

    [Header("Attack")]
    [SerializeField] private int damage = 1;
    [SerializeField] private Collider2D attackHitbox;

    [Header("Events")]
    public UnityEvent OnWindupStart;
    public UnityEvent OnAttackStart;
    public UnityEvent OnRecoveryStart;
    public UnityEvent OnAttackFinished;

    private bool isAttacking;
    public bool IsAttacking => isAttacking;



    private void Awake()
    {
        attackHitbox.enabled = false;
    }

    public void TryAttack()
    {
        if (IsAttacking) return;
        StartCoroutine(AttackRoutine());
    }

    private System.Collections.IEnumerator AttackRoutine()
    {
        this.isAttacking = true;

        OnWindupStart?.Invoke();
        Debug.Log("STARTED WINDUP");
        yield return new WaitForSeconds(windupDuration);

        EnableHitbox();
        OnAttackStart?.Invoke();
        Debug.Log("STARTED ATTACK HITBOX");

        yield return new WaitForSeconds(activeDuration);

        DisableHitbox();
        OnRecoveryStart?.Invoke();
        yield return new WaitForSeconds(recoveryDuration);

        this.isAttacking = false;
        OnAttackFinished?.Invoke();
    }

    public void EnableHitbox()
    {
        attackHitbox.enabled = true;
    }

    public void DisableHitbox()
    {
       attackHitbox.enabled = false;
    }

   //private void OnTriggerEnter2D(Collider2D other)
   //{
   //    if (!attackHitbox.enabled) return;
   //
   //    if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
   //    {
   //        damageable.TakeDamage(damage);
   //    }
   //}
}