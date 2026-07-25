using UnityEngine;
using UnityEngine.Events;

public class MeleeAttackBehaviour : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float windupDuration = 0.4f;
    [SerializeField] private float activeDuration = 0.2f;
    [SerializeField] private float recoveryDuration = 0.5f;

    [Header("Attack")]
    [SerializeField] private AttackHitbox attackHitbox;
    [SerializeField] private Collider2D hitboxCollider;

    [Header("Animation")]
    [SerializeField] private bool isAnimationDriven;

    [Header("Events")]
    public UnityEvent OnWindupStart;
    public UnityEvent OnAttackStart;
    public UnityEvent OnRecoveryStart;
    public UnityEvent OnAttackFinished;

    private bool isAttacking;
    public bool IsAttacking => isAttacking;

    private void Awake()
    {
        hitboxCollider.enabled = false;
        attackHitbox.IsParryable = false;
    }

    public void TryAttack()
    {
        if (IsAttacking) return;
        StartCoroutine(AttackRoutine());
    }

    private System.Collections.IEnumerator AttackRoutine()
    {
        isAttacking = true;

        OnWindupStart?.Invoke();
        yield return new WaitForSeconds(windupDuration);

        if (!isAnimationDriven) EnableHitbox();
        OnAttackStart?.Invoke();

        yield return new WaitForSeconds(activeDuration);

        if (!isAnimationDriven) DisableHitbox();
        OnRecoveryStart?.Invoke();
        yield return new WaitForSeconds(recoveryDuration);

        isAttacking = false;
        OnAttackFinished?.Invoke();
    }

    public void EnableHitbox()
    {
        hitboxCollider.enabled = true;
        attackHitbox.IsParryable = true;
    }

    public void DisableHitbox()
    {
        hitboxCollider.enabled = false;
        attackHitbox.IsParryable = false;
    }
}