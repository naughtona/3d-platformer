using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level1EndScreensController : MonoBehaviour
{
    public Text attempt1Text;
    public Text attemptsTotalText;

    void Start() {
        Application.targetFrameRate = 30; // constant stable frame rate
        
        this.attempt1Text.text = "Level 1 Attempts: " + Level1Controller.nAttempts;

        int total = Level1Controller.nAttempts;
        this.attemptsTotalText.text = "Total Attempts: " + total;
    }

    public void OnNextButtonPressed() {
        SceneManager.LoadScene("Level2");
    }

    public void OnQuitButtonPressed() {
        GlobalOptions.ResetStatics();
        SceneManager.LoadScene("MainMenu");
    }
}
