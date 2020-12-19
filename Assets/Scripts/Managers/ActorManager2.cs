using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager2 : MonoBehaviour
{
	public List<GameObject> coinsTemplateList = new List<GameObject>();
	public List<GameObject> ghostsTemplateList = new List<GameObject>();
	public List<GameObject> trackTemplateList = new List<GameObject>();
	
    private GameObject coins;
    private GameObject ghosts;
    private GameObject track;

    public void GenerateActors(int _mapIndex = 0) {
        coins = GameObject.Instantiate<GameObject>(coinsTemplateList[_mapIndex]);
        ghosts = GameObject.Instantiate<GameObject>(ghostsTemplateList[_mapIndex]);
	}

    public void GenerateTrack(int _mapIndex = 0)
	{
		track = GameObject.Instantiate<GameObject>(trackTemplateList[_mapIndex]);
	}

    public void ResetActors() {
        if (coins != null) Destroy(coins);
        if (ghosts != null) Destroy(ghosts);
        
        GenerateActors();
    }
}
