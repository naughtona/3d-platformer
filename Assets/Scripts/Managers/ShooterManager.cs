using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShooterManager : MonoBehaviour
{
    public GameObject playerTemplate;
    public UnityEvent levelFinishedEvent;
    public UnityEvent incrementScoreEvent;
    public UnityEvent resetHealthAndScoreEvent;
    public UnityEvent playerDiedEvent;

    private GameObject player;

    public GameObject GeneratePlayer() {
        player = Instantiate(playerTemplate);
		return player;
    }

    public void ResetPlayer() {
        InvokeResetHealthandScoreEvent();
        player.GetComponent<ShooterController>().ResetPosition();
    }

    public GameObject GetPlayer() {
        return this.player;
    }

    public void InvokeLevelFinishedEvent() {
        this.levelFinishedEvent.Invoke();
    }

    public void InvokeIncrementScoreEvent() {
        this.incrementScoreEvent.Invoke();
    }

    public void InvokeResetHealthandScoreEvent() {
        this.resetHealthAndScoreEvent.Invoke();
    }

    public void InvokePlayerDiedEvent() {
        this.playerDiedEvent.Invoke();
    }
}
