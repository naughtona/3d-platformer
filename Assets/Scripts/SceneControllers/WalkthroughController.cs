using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WalkthroughController : MonoBehaviour
{
    public Text currentLevelText;
    public Text displayText;
    public Text playerDiedText;
    public GameObject playerHitPanel;
    public GameObject inGamePanel;
    public HUDController HUDController;
    public PlayerManager playerManager;
    public ActorManager actorManager;
    public Vector3 playerStartPosition;

    private GameObject player;
    private Coroutine runTutorial;

    void Start () {
        Application.targetFrameRate = 30; // constant stable frame rate

        player = playerManager.GeneratePlayer();
        player.transform.localPosition = playerStartPosition;
        player.GetComponent<PlayerController>().SetStartPosition(playerStartPosition);

        actorManager.GenerateActors();
        actorManager.GenerateTrack();

        currentLevelText.text = "Tutorial\r\n\n" +
                                "Reach The Green Portal";
        StartCoroutine(ShowCurrentLevelText());
        runTutorial = StartCoroutine(RunTutorial());
    }

    public IEnumerator RunTutorial() {
        yield return StartCoroutine("TeachJump");   
        yield return StartCoroutine("TeachForward");
        yield return StartCoroutine("TeachYaw");
        yield return StartCoroutine("TeachPause");
        yield return StartCoroutine("TeachScoring");
        yield return StartCoroutine("TeachShop");
        yield return StartCoroutine("TeachTemp");
        yield return StartCoroutine("WarnOfGhosts");
        yield return StartCoroutine("WarnOfEdge");
    }

    IEnumerator TeachJump() {
        yield return new WaitForSeconds(2);
        HUDController.DisableUpdate(true);
        Time.timeScale = 0;
        displayText.text="Press Space To Jump";
        displayText.gameObject.SetActive(true);
        yield return StartCoroutine(WaitForKeyDown(KeyCode.Space));
        displayText.gameObject.SetActive(false);
        Time.timeScale = 1;
        HUDController.DisableUpdate(false);
        player.GetComponent<PlayerController>().Jump();
        yield return new WaitForSeconds(2);
    }

    IEnumerator TeachForward() {
        HUDController.DisableUpdate(true);
        Time.timeScale = 0;
        displayText.text =  "WASD or Arrow Keys To Move\r\n\n" +
                            "Press W To Move Forward";
        displayText.gameObject.SetActive(true);
        yield return StartCoroutine(WaitForKeyDown(KeyCode.W));
        displayText.gameObject.SetActive(false);
        Time.timeScale = 1;
        HUDController.DisableUpdate(false);
        player.GetComponent<PlayerController>().WASD(0.0f,1.0f);
        yield return new WaitForSeconds(2);
    }

    IEnumerator TeachYaw() {
        HUDController.DisableUpdate(true);
        Time.timeScale = 0;
        displayText.text="Move Mouse To Change Direction";
        displayText.gameObject.SetActive(true);
        yield return StartCoroutine(WaitForMouseMovement());
        displayText.gameObject.SetActive(false);
        Time.timeScale = 1;
        HUDController.DisableUpdate(false);
        yield return new WaitForSeconds(2);
    }

    IEnumerator TeachPause() {
        HUDController.DisableUpdate(true);
        Time.timeScale = 0;
        displayText.text = "Press Tab To Pause/Unpause Game";
        displayText.gameObject.SetActive(true);
        yield return StartCoroutine(WaitForKeyDown(KeyCode.Tab));
        displayText.gameObject.SetActive(false);
        Time.timeScale = 1;
        HUDController.DisableUpdate(false);
        HUDController.Pause();
        yield return new WaitForSeconds(2);
    }

    IEnumerator TeachScoring() {
        HUDController.DisableUpdate(true);
        Time.timeScale = 0;
        displayText.text =  "Collect Coins For 5 Points Each\r\n\n" +
                            "Press CapsLock To Continue";
        displayText.gameObject.SetActive(true);
        yield return StartCoroutine(WaitForKeyDown(KeyCode.CapsLock));
        displayText.gameObject.SetActive(false);
        Time.timeScale = 1;
        HUDController.DisableUpdate(false);
        yield return new WaitForSeconds(2);
    }

    
    IEnumerator TeachShop() {
        HUDController.DisableUpdate(true);
        Time.timeScale = 0;
        displayText.text = "Press Shift To Show/Hide Shop";
        displayText.gameObject.SetActive(true);
        yield return StartCoroutine(WaitForKeyDown(KeyCode.LeftShift));
        displayText.gameObject.SetActive(false);
        Time.timeScale = 1;
        HUDController.DisableUpdate(false);
        HUDController.Shop();
        yield return new WaitForSeconds(2);
    }

    IEnumerator TeachTemp() {
        HUDController.DisableUpdate(true);
        Time.timeScale = 0;
        displayText.text =  "If Temperature Gets To Zero, You MELT!\r\n\n" +
                            "Press CapsLock To Continue";
        displayText.gameObject.SetActive(true);
        yield return StartCoroutine(WaitForKeyDown(KeyCode.CapsLock));
        displayText.gameObject.SetActive(false);
        Time.timeScale = 1;
        HUDController.DisableUpdate(false);
        yield return new WaitForSeconds(2);
    }

    IEnumerator WarnOfGhosts() {
        HUDController.DisableUpdate(true);
        Time.timeScale = 0;
        displayText.text =  "Contact With Ghosts & Fireballs Melt You!\r\n\n" +
                            "Press CapsLock To Continue";
        displayText.gameObject.SetActive(true);
        yield return StartCoroutine(WaitForKeyDown(KeyCode.CapsLock));
        displayText.gameObject.SetActive(false);
        Time.timeScale = 1;
        HUDController.DisableUpdate(false);
        yield return new WaitForSeconds(2);
    }

    IEnumerator WarnOfEdge() {
        HUDController.DisableUpdate(true);
        Time.timeScale = 0;
        displayText.text =  "Fall Off The Track & You Will MELT!\r\n\n" +
                            "Press CapsLock To Continue";
        displayText.gameObject.SetActive(true);
        yield return StartCoroutine(WaitForKeyDown(KeyCode.CapsLock));
        displayText.gameObject.SetActive(false);
        Time.timeScale = 1;
        HUDController.DisableUpdate(false);
        yield return new WaitForSeconds(2);
    }

    IEnumerator WaitForMouseMovement() {
        while (Input.GetAxis("Mouse X")==0)
            yield return null;
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

    public void QuitGame() {
        GlobalOptions.ResetStatics();
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayerWon() {
        GlobalOptions.score=0;
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame() {
        StopCoroutine(runTutorial);
        playerManager.ResetPlayer();
        actorManager.ResetActors();
        currentLevelText.text = "Tutorial\r\n\n" +
                                "Reach The Green Portal";
        StartCoroutine(ShowCurrentLevelText());
        runTutorial = StartCoroutine(RunTutorial());
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
        inGamePanel.SetActive(true);
        RestartGame();
    }
}
