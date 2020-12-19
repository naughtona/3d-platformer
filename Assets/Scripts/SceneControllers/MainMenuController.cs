using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    void Start() {
        Application.targetFrameRate = 30; // constant stable frame rate
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OpenTutorial()
    {
        SceneManager.LoadScene("Walkthrough");
    }

    public void OpenInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings");
    }
}
