using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    private ShooterController controller;
    private Transform shooterTransform;

    void Start() {
        ShooterManager shooterManager = GameObject.Find("ShooterManager").GetComponent<ShooterManager>();
        shooterTransform = shooterManager.GetPlayer().transform;
        controller = shooterTransform.GetComponent<ShooterController>();
    }

    private bool stagePassed = false;
    void Update() {
        if (stagePassed && this.transform.position.y < 200f) {
			controller.SetDisabled(true);
            Vector3 translation = 10.0f * Vector3.up * Time.deltaTime;
            this.transform.Translate(translation);
            shooterTransform.Translate(translation);
        } else if (this.transform.position.y >= 200f) {
            controller.SetDisabled(false);
        }
    }

    public void StagePassed() {
        stagePassed = true;
	}
}
