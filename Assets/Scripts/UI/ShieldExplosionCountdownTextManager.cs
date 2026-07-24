using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ShieldExplosionCountdownTextManager : MonoBehaviour
{ [SerializeField] private ShieldCountdownExplosionManager explosionManager;
    [SerializeField] private TMP_Text countdownText;

    private void Awake()
    {
        explosionManager.OnCountdownTick.AddListener(UpdateCountdownText);
    }

    private void UpdateCountdownText(float timeRemaining)
    {
        countdownText.text = Mathf.Max(timeRemaining, 0f).ToString("F2");
    }
}

