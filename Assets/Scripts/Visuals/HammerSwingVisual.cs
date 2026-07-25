using UnityEngine;

public class HammerSwingVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MeleeAttackBehaviour meleeAttack;
    [SerializeField] private GameObject hammerObject;
    [SerializeField] private Animator hammerAnimator;

    private void Awake()
    {
        meleeAttack.OnWindupStart.AddListener(ShowHammer);
        meleeAttack.OnAttackFinished.AddListener(HideHammer);

        hammerObject.SetActive(false);
    }

    private void ShowHammer()
    {
        hammerObject.SetActive(true);
        hammerAnimator.Play(0, 0, 0f);
    }

    private void HideHammer()
    {
        hammerObject.SetActive(false);
    }
}