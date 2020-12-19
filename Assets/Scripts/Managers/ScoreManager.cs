using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {
    
	public void ResetScore() {
        GlobalOptions.score = 0;
    }

    public void IncrementScore() {
        GlobalOptions.score += 5;
    }
}
