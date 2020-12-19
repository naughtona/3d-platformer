using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class InstructionsController : MonoBehaviour
{
    void Start() {
        Application.targetFrameRate = 30; // constant stable frame rate
    }
    
    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
