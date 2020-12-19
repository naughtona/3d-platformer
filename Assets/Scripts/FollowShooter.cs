using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowShooter : MonoBehaviour
{
    public ShooterManager playerManager;
    private GameObject player;

    void Update() {
        // camera stays on same rotation as player
        if (player != null) {
            if (player.transform.position.y > 88.0f) {
                this.transform.position = player.transform.position;
                this.transform.rotation = player.transform.rotation;
                this.transform.Translate(new Vector3(0,5.0f,-10.0f), Space.Self);
            }
            this.transform.LookAt(player.transform.position + Vector3.up * 3.0f);
        } else {
            player = playerManager.GetPlayer();
        }
    } 
}
