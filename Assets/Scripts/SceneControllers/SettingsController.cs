using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {

    public Slider sensitivitySlider;
    public Slider ghostSpeedSlider;
    public Slider gargoyleFireRateSlider;

    public void Start() {
        Application.targetFrameRate = 30; // constant stable frame rate

        sensitivitySlider.value = GlobalOptions.sensitivity;
        ghostSpeedSlider.value = GlobalOptions.ghostSpeed;
        gargoyleFireRateSlider.value = GlobalOptions.gargoyleFireRate;
    }

    public void OnBackButtonPressed() {
        SceneManager.LoadScene("MainMenu");
    }

    public void SensitivitySliderChanged() {
        GlobalOptions.sensitivity = sensitivitySlider.value;
    }

    public void GhostSpeedChanged() {
        GlobalOptions.ghostSpeed = ghostSpeedSlider.value;
    }

    public void GargoyleFireRateChanged() {
        GlobalOptions.gargoyleFireRate = gargoyleFireRateSlider.value;
    }
    
}

