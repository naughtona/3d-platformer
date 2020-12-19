using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level3Controller : MonoBehaviour {
    public Text currentLevelText;
    public Text playerDiedText;
    public Text temperatureTitle;
    public Text attemptNoText;
    public GameObject playerHitPanel;
    public GameObject inGamePanel;
    public PlayerManager playerManager;
    public ActorManager actorManager;
    public Vector3 playerStartPosition;

    public static int nAttempts;

    private bool puzzleHint = false;
    private GameObject player;

	void Start() {
        Application.targetFrameRate = 30; // constant stable frame rate

		player = playerManager.GeneratePlayer();
		player.transform.localPosition = playerStartPosition;
		player.GetComponent<PlayerController>().SetStartPosition(playerStartPosition);

        actorManager.GenerateActors();
        actorManager.GenerateTrack();

        temperatureTitle.text = GlobalOptions.tempText;
        currentLevelText.text = "Level 3\r\n\n" +
                                "Reach The Green Portal";
        StartCoroutine(ShowCurrentLevelText());
        nAttempts = 1;
        attemptNoText.text = "Attempt " + nAttempts;
        StartCoroutine(ShowNAttempts());
    }

    void Update() {
        if (!puzzleHint && player.transform.localPosition.x >= 63f) {
            puzzleHint = true;
            currentLevelText.text = "Solve the puzzle\r\n" +
                                    "To call the Boatman";
            StartCoroutine(ShowCurrentLevelText());
        }
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
        SceneManager.LoadScene("Level3Ended");
    }

    public void RestartGame() {
        GlobalOptions.ResetStatics();
        playerManager.ResetPlayer();
        actorManager.ResetActors();
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
        playerManager.ResetPlayer();
        actorManager.ResetActors();
        inGamePanel.SetActive(true);
        nAttempts++;
        attemptNoText.text = "Attempt " + nAttempts;
        StartCoroutine(ShowNAttempts());
    }
}

