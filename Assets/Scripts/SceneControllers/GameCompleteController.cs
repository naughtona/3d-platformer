using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameCompleteController : MonoBehaviour {
    void Start() {
        Application.targetFrameRate = 30; // constant stable frame rate
    }

    public void OnQuitButtonPressed() {
        GlobalOptions.ResetStatics();
        SceneManager.LoadScene("MainMenu");
    }

}
