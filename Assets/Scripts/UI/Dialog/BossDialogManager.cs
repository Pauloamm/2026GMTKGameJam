using UnityEngine;

public class BossDialogManager : MonoBehaviour
{
    [SerializeField] private DialogBoxUI dialogBox;
    [SerializeField] private string bossText;
    [SerializeField] private BossController boss;
    [SerializeField] private float hideDelay = 2f;

    private void Awake()
    {
        boss.OnBossEngaged.AddListener(HandleBossEngaged);
    }

    private void HandleBossEngaged()
    {
        dialogBox.Show(bossText);
        dialogBox.HideAfterSeconds(hideDelay);
    }
}