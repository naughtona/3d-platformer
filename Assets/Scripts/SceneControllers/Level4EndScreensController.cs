using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level4EndScreensController : MonoBehaviour
{
    public Text attempt1Text;
    public Text attempt2Text;
    public Text attempt3Text;
    public Text attempt4Text;
    public Text attemptsTotalText;

    void Start() {
        Application.targetFrameRate = 30; // constant stable frame rate

        this.attempt1Text.text = "Level 1 Attempts: " + Level1Controller.nAttempts;
        this.attempt2Text.text = "Level 2 Attempts: " + Level2Controller.nAttempts;
        this.attempt3Text.text = "Level 3 Attempts: " + Level3Controller.nAttempts;
        this.attempt4Text.text = "Level 4 Attempts: " + Level4Controller.nAttempts;

        int total = Level1Controller.nAttempts +
                    Level2Controller.nAttempts +
                    Level3Controller.nAttempts +
                    Level4Controller.nAttempts;
        this.attemptsTotalText.text = "Total Attempts: " + total;
    }

    public void OnNextButtonPressed() {
        SceneManager.LoadScene("GameComplete");
    }
}

