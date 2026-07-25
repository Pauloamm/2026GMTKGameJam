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
        SceneManager.LoadScene(gameSceneName);
    }

    private void OnControlsPressed()
    {
        mainMenuPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    private void OnBackFromControlsPressed()
    {
        ShowMainMenu();
    }

    private void OnQuitPressed()
    {
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