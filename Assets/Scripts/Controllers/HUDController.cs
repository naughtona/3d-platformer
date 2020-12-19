using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public HealthManager healthManager;
    public Text scoreText;
    public Text yourCoinsText;
    public Text temperatureTitle;

    public GameObject pauseMenu;
    public GameObject shopMenu;
    public GameObject controlsMenu;
    public GameObject inGamePanel;

    public Button tempTickButton;
    public Button tempUpgradeButton;
    public Button speedTickButton;
    public Button speedUpgradeButton;
    public Button jumpTickButton;
    public Button jumpUpgradeButton;

    private bool isPaused = false;
    private bool isShop = false;
    private bool isControls = false;
    private bool isDisabled = false;

    void Update () {
        if (isDisabled) return;

        if (Input.GetKeyDown(KeyCode.Tab) || (isPaused && !isControls && Input.GetKeyDown(KeyCode.Escape)))
            Pause();
        
        if (Input.GetKeyDown(KeyCode.LeftShift) || (isShop && Input.GetKeyDown(KeyCode.Escape)))
            Shop();

        if (isControls && Input.GetKeyDown(KeyCode.Escape))
            Controls();

        // Update score text field
        this.scoreText.text = "Coins: " + GlobalOptions.score;
        // Update temp text field
        temperatureTitle.text = GlobalOptions.tempText;
    }

    public void Pause() {
        if (isShop || isControls) return;

        if (isPaused == true) {
            Time.timeScale = 1;
            isPaused = false;
            pauseMenu.SetActive(false);
            inGamePanel.SetActive(true);
        }
        else {
            Time.timeScale = 0;
            isPaused = true;
            inGamePanel.SetActive(false);
            pauseMenu.SetActive(true);
            
        }
    }

    public void Shop() {
        if (isPaused || isControls || isDisabled) return;

        if (isShop == true) {
            Time.timeScale = 1;
            isShop = false;
            shopMenu.SetActive(false);
            inGamePanel.SetActive(true);
        }
        else {
            Time.timeScale = 0;
            isShop = true;
            inGamePanel.SetActive(false);
            shopMenu.SetActive(true);
            UpdateCoinsText();
            UpdateUpgrades();
        }
    }

    public void Controls() {
        if (isControls == true) {
            isControls = false;
            controlsMenu.SetActive(false);
            pauseMenu.SetActive(true);
        }
        else {
            isControls = true;
            pauseMenu.SetActive(false);
            controlsMenu.SetActive(true);
        }
    }

    private void UpdateCoinsText() {
        yourCoinsText.text = "Your Coins: " + GlobalOptions.score;
    }

    private void UpdateUpgrades() {
        SwitchTempButton(GlobalOptions.tempUpgrade);
        SwitchSpeedButton(GlobalOptions.speedUpgrade);
        SwitchJumpButton(GlobalOptions.jumpUpgrade);
    }

    private void SwitchButton(Button old_b, Button new_b) {
        old_b.gameObject.SetActive(false);
        new_b.gameObject.SetActive(true);
    }

    public void SwitchTempButton(bool off) {
        if (off) SwitchButton(tempUpgradeButton, tempTickButton);
        else SwitchButton(tempTickButton, tempUpgradeButton);
    }

    public void SwitchSpeedButton(bool off) {
        if (off) SwitchButton(speedUpgradeButton, speedTickButton);
        else SwitchButton(speedTickButton, speedUpgradeButton);
    }

    public void SwitchJumpButton(bool off) {
        if (off) SwitchButton(jumpUpgradeButton, jumpTickButton);
        else SwitchButton(jumpTickButton, jumpUpgradeButton);
    }

    public void DisableUpdate(bool disable) {
        isDisabled = disable;
    }

    public void UpdateTempUpgrade() {
        GlobalOptions.tempText += " [X2]";

        GlobalOptions.maxTemp *= 2;
        GlobalOptions.tempUpgrade = true;
        healthManager.ResetHealthToStarting();
    }

    public void ApplyTempUpgrade() {
        // check coin balance against price
        if (GlobalOptions.score >= 100) {
            GlobalOptions.score -= 100;
            UpdateCoinsText();
            SwitchTempButton(true);
            UpdateTempUpgrade();
        }
    }

    public void ApplySpeedUpgrade() {
        // check coin balance against price
        if (GlobalOptions.score >= 75) {
            GlobalOptions.score -= 75;
            UpdateCoinsText();
            SwitchSpeedButton(true);
            GlobalOptions.playerSpeed *= 2;
            GlobalOptions.speedUpgrade = true;
        }
    }

    public void ApplyJumpUpgrade() {
        // check coin balance against price
        if (GlobalOptions.score >= 50) {
            GlobalOptions.score -= 50;
            UpdateCoinsText();
            SwitchJumpButton(true);
            GlobalOptions.jumpHeight *= 2;
            GlobalOptions.jumpUpgrade = true;
        }
    }
}
