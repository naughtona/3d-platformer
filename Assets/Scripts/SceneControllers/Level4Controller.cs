using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level4Controller : MonoBehaviour {
    public Text currentLevelText;
    public Text playerDiedText;
    public Text temperatureTitle;
    public Text attemptNoText;
    public GameObject playerHitPanel;
    public GameObject inGamePanel;
    public ShooterManager shooterManager;
    public ActorManager actorManager;
    public Vector3 playerStartPosition;
    private Coroutine teachSnowball;
    public HUDController HUDController;
    public Text displayText;
    private GameObject player;

    public static int nAttempts;

	void Start() {
        Application.targetFrameRate = 30; // constant stable frame rate

		player = shooterManager.GeneratePlayer();
		player.transform.localPosition = playerStartPosition;
		player.GetComponent<ShooterController>().SetStartPosition(playerStartPosition);

        actorManager.GenerateActors();
        actorManager.GenerateTrack();

        temperatureTitle.text = GlobalOptions.tempText;
        currentLevelText.text = "Level 4\r\n\n" +
                                "Reach The Green Portal";
        StartCoroutine(ShowCurrentLevelText());
        nAttempts = 1;
        attemptNoText.text = "Attempt " + nAttempts;
        StartCoroutine(ShowNAttempts());
    
        teachSnowball = StartCoroutine(TeachSnowball());
    }

    IEnumerator TeachSnowball() {
        yield return new WaitForSeconds(3.0f);
        HUDController.DisableUpdate(true);
        Time.timeScale = 0;
        displayText.text = "Snowballs UNLOCKED!\r\n\n" +
                           "Left Click To Shoot!";
        displayText.gameObject.SetActive(true);
        yield return StartCoroutine(WaitForKeyDown(KeyCode.Mouse0));
        displayText.gameObject.SetActive(false);
        Time.timeScale = 1;
        HUDController.DisableUpdate(false);
        player.GetComponent<ShooterController>().ShootSnowball();
    }

    IEnumerator WaitForKeyDown(KeyCode keyCode) {
        while (!Input.GetKeyDown(keyCode))
            yield return null;
    }

    IEnumerator ShowCurrentLevelText() {
        currentLevelText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        currentLevelText.gameObject.SetActive(false);
    }

    IEnumerator ShowNAttempts() {
        attemptNoText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        attemptNoText.gameObject.SetActive(false);
    }

    public void QuitGame() {
        GlobalOptions.ResetStatics();
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayerWon() {
        SceneManager.LoadScene("Level4Ended");
    }

    public void RestartGame() {
        StopCoroutine(teachSnowball);
        GlobalOptions.ResetStatics();
        player.GetComponent<ShooterController>().bossDamage = 25;
        shooterManager.ResetPlayer();
        actorManager.ResetActors();
        teachSnowball = StartCoroutine(TeachSnowball());
    }

    public void ResetGame() {
        Time.timeScale = 0;
        inGamePanel.SetActive(false);
        playerDiedText.gameObject.SetActive(true);
        playerHitPanel.SetActive(true);
        StartCoroutine(WaitForRealSeconds(3));
    }

    IEnumerator WaitForRealSeconds(float seconds) {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup-startTime < seconds) {
            yield return null;
        }
        ResumeReset();
    }

    void ResumeReset() {
        Time.timeScale = 1;
        playerHitPanel.SetActive(false);
        playerDiedText.gameObject.SetActive(false);
        GlobalOptions.ResetStatics();
        player.GetComponent<ShooterController>().bossDamage = 25;
        shooterManager.ResetPlayer();
        actorManager.ResetActors();
        inGamePanel.SetActive(true);
        nAttempts++;
        attemptNoText.text = "Attempt " + nAttempts;
        StartCoroutine(ShowNAttempts());
    }
}

