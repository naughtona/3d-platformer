using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
	public GameObject coinsTemplate;
    public GameObject ghostsTemplate;
    public GameObject trackTemplate;

    private GameObject coins;
    private GameObject ghosts;
    private GameObject track;

    public void GenerateActors() {
        coins = GameObject.Instantiate<GameObject>(coinsTemplate);
        ghosts = GameObject.Instantiate<GameObject>(ghostsTemplate);
    }

    public void GenerateTrack() {
		track = GameObject.Instantiate<GameObject>(trackTemplate);
	}

    public void ResetActors() {
        if (coins != null) Destroy(coins);
        if (ghosts != null) Destroy(ghosts);
        
        GenerateActors();
    }
}
