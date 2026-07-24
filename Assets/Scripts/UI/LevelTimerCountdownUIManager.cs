using TMPro;
using UnityEngine;

public class LevelTimerCountdownUIManager : MonoBehaviour
{
    [SerializeField] private LevelTimerManager levelTimerManager;
    [SerializeField] private TMP_Text timerText;

    private int minutes, seconds, milliseconds; // avoid creating every frame


    private void Awake()
    {
        levelTimerManager.OnTimerTick.AddListener(UpdateTimerText);
    }

    private void UpdateTimerText(float remainingTime)
    {
        remainingTime = Mathf.Max(remainingTime, 0f);

        this.minutes = Mathf.FloorToInt(remainingTime / 60f);
        this.seconds = Mathf.FloorToInt(remainingTime % 60f);
        this.milliseconds = Mathf.FloorToInt((remainingTime * 1000f) % 1000f);

        timerText.text = $"{minutes}:{seconds:D2}:{milliseconds:D3}";
    }
}