using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GargoyleProjectileController : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag != "Enemy" && collider.gameObject.tag != "Boss")
            Destroy(this.gameObject);
    }

    void Update() {
        if (Time.timeScale == 0) return;
        this.transform.position += (velocity) * Time.deltaTime;
    }

    public void SetProjectileVelocity(Vector3 velocity) {
        this.velocity = velocity;
    }
}
