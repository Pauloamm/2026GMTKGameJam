using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject controlsPanel;

    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button quitButton;

    [Header("Scene To Load")]
    [SerializeField] private string gameSceneName;

    [Header("Click Feedback Delay")]
    [SerializeField] private float startButtonClickDelay = 0.4f;

    private void Awake()
    {
        startButton.onClick.AddListener(OnStartPressed);
        controlsButton.onClick.AddListener(OnControlsPressed);
        backButton.onClick.AddListener(OnBackFromControlsPressed);
        quitButton.onClick.AddListener(OnQuitPressed);

        ShowMainMenu();
    }

    private void OnStartPressed()
    {
        StartCoroutine(LoadGameSceneAfterDelay());
    }

    private IEnumerator LoadGameSceneAfterDelay()
    {
        yield return new WaitForSeconds(startButtonClickDelay);

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(gameSceneName);
    }

    private void OnControlsPressed()
    {
        StartCoroutine(OpenControlsMenuAfterDelay());

    }

    private IEnumerator OpenControlsMenuAfterDelay()
    {
        yield return new WaitForSeconds(startButtonClickDelay);

        mainMenuPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }
    private void OnBackFromControlsPressed()
    {
        StartCoroutine(CloseControlsMenuAfterDelay());

    }

    private IEnumerator CloseControlsMenuAfterDelay()
    {
        yield return new WaitForSeconds(startButtonClickDelay);
        ShowMainMenu();

    }
    private void OnQuitPressed()
    {
        StartCoroutine(QuitGameAfterDelay());

    }

    private IEnumerator QuitGameAfterDelay()
    {
        yield return new WaitForSeconds(startButtonClickDelay);

        Debug.Log("Quitting game");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void ShowMainMenu()
    {
        controlsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}