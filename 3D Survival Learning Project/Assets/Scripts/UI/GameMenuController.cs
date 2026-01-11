using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class GameMenuController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _pauseMenuPanel;
    [SerializeField] private GameObject _controlsPanel;

    [Header("Settings")]
    [SerializeField] private string _mainMenuSceneName = "Menu";

    public static bool IsPaused = false;

    public void Awake()
    {
        _pauseMenuPanel.SetActive(false);
        _controlsPanel.SetActive(false);
        IsPaused = false;
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) 
        {
            if(_controlsPanel.activeSelf)
            {
               CloseControls();
            }
            else
            {
                if (IsPaused)
                    Resume();
                else
                    Pause();
            }
        }
    }

    public void Resume()
    {
        _pauseMenuPanel.SetActive(false);
        _controlsPanel.SetActive(false);
        Time.timeScale = 1.0f;
        IsPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pause()
    {
        _pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OpenControls()
    {
        _pauseMenuPanel.SetActive(false);
        _controlsPanel.SetActive(true);
    }

    public void CloseControls()
    {
        _controlsPanel.SetActive(false);
        _pauseMenuPanel.SetActive(true);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(_mainMenuSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
