using TMPro;
using UnityEngine;

public class ShieldExplosionCountdownTextManager : MonoBehaviour
{
    [SerializeField] private ShieldCountdownExplosionManager explosionManager;
    [SerializeField] private TMP_Text countdownText;

    private void Awake()
    {
        explosionManager.OnCountdownTick += UpdateCountdownText;
    }

    private void UpdateCountdownText(float timeRemaining)
    {
        countdownText.text = Mathf.Max(timeRemaining, 0f).ToString("F2");
    }
}

