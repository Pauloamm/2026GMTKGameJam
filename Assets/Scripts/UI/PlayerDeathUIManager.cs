using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDeathUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerLifeManager playerLifeManager;
    [SerializeField] private GameObject deathScreenRoot;
    [SerializeField] private Text messageText;
    [SerializeField] private Text countdownText;

    [Header("Settings")]
    [SerializeField] private string deathMessage = "Go Back to Work";
    [SerializeField] private int restartCountdownSeconds = 3;

    private void Awake()
    {
        deathScreenRoot.SetActive(false);
        playerLifeManager.OnDeath += HandlePlayerDeath;
    }

    private void HandlePlayerDeath()
    {
        StartCoroutine(DeathSequenceRoutine());
    }

    private IEnumerator DeathSequenceRoutine()
    {
        deathScreenRoot.SetActive(true);
        messageText.text = deathMessage;

        int secondsRemaining = restartCountdownSeconds;

        while (secondsRemaining > 0)
        {
            countdownText.text = secondsRemaining.ToString();
            yield return new WaitForSeconds(1f);
            secondsRemaining--;
        }

        RestartLevel();
    }

    private void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}