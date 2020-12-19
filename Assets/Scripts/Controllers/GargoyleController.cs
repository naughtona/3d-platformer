using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GargoyleController : MonoBehaviour
{
    public GameObject projectileTemplate;
    public float timeToIntercept = 1f; // 1 sec for projectiles to intercept
    public float attackRadius = 10f;

    private GameObject player;
    private PlayerController playerController; 
    private Vector3 playerVelocity;
    private Vector3 projectileVelocity;
    private Vector3 enemyVelocity;
    private Vector3 attackVector = Vector3.zero;
    private bool isCoroutineFinished;
    private bool canShoot = true;
    private bool attackMode;
    private float firerate;

    void Start() {
        player = GameObject.Find("PlayerManager").GetComponent<PlayerManager>().GetPlayer();
        playerController = player.GetComponent<PlayerController>();
        firerate = 1 / GlobalOptions.gargoyleFireRate;
    }

    void Update() {
        if (Time.timeScale == 0) return;

        if (!isCoroutineFinished) {
            StartCoroutine(CheckForPlayer());
        }

        // only shoot if player is within attack radius
        if (canShoot == true && attackMode) {
            // find players velocity and use it to determine where player 
            //  will be when projectile arrives
            playerVelocity = PlayerVelocity();
            // solve projectile trajectory to intercept player
            projectileVelocity = attackVector/timeToIntercept + playerVelocity;
            ShootProjectile();
            canShoot = false;
            StartCoroutine(Reload());
        }
    }

    // only check for player once per second for efficiency
    IEnumerator CheckForPlayer() {
        isCoroutineFinished = true;
        attackMode = CheckAttack();
        yield return new WaitForSeconds(0.5f);
        isCoroutineFinished = false;
    }

    IEnumerator Reload() {
        yield return new WaitForSeconds(firerate);
        canShoot = true;
    }

    void ShootProjectile() {
        GameObject projectile = GameObject.Instantiate<GameObject>(projectileTemplate);
        Physics.IgnoreCollision(projectile.GetComponent<Collider>(), this.GetComponent<Collider>());
        projectile.transform.position = this.transform.position;
        projectile.GetComponent<GargoyleProjectileController>().SetProjectileVelocity(projectileVelocity);
    }

     // finds the distance between enemy and player
    bool CheckAttack() {
        this.attackVector = player.transform.position - this.transform.position;
        float distance = Vector3.Magnitude(this.attackVector); 
        return (distance < this.attackRadius);
    }

    Vector3 PlayerVelocity() {
        return this.playerController.GetPlayerVelocity();
    }

}
