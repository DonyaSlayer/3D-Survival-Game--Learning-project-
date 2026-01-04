using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public int firstScene;
    public GameObject settingsPanel;
    public Animator blackScreen;
    public void NewGame()
    {
        blackScreen.Play("BlackScreenOn");
        Invoke(nameof(LoadScene), 0.5f);
    }

    public void SettingsSetState(bool state)
    {
        settingsPanel.SetActive(state);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(firstScene);
    }
}
