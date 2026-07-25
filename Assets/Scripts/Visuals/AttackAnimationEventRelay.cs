using UnityEngine;

public class AttackAnimationEventRelay : MonoBehaviour
{
    [SerializeField] private MeleeAttackBehaviour meleeAttack;

    public void OnAnimationHitboxOn()
    {
        meleeAttack.EnableHitbox();
    }

    public void OnAnimationHitboxOff()
    {
        meleeAttack.DisableHitbox();
    }
}