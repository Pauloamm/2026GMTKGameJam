using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameWinUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyBase bossReference;
    [SerializeField] private GameObject winScreenRoot;
    [SerializeField] private Text messageText;
    [SerializeField] private Text countdownText;

    [Header("Settings")]
    [SerializeField] private string winMessage = "Victory";
    [SerializeField] private int restartCountdownSeconds = 3;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void Awake()
    {
        winScreenRoot.SetActive(false);
        bossReference.OnDeath += HandleBossDeath;
    }

    private void HandleBossDeath(EnemyBase enemy)
    {
        StartCoroutine(WinSequenceRoutine());
    }

    private IEnumerator WinSequenceRoutine()
    {
        winScreenRoot.SetActive(true);
        messageText.text = winMessage;

        int secondsRemaining = restartCountdownSeconds;

        while (secondsRemaining > 0)
        {
            countdownText.text = secondsRemaining.ToString();
            yield return new WaitForSeconds(1f);
            secondsRemaining--;
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }
}