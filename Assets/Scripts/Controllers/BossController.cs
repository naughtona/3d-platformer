using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BossController : MonoBehaviour
{
    public float attackSpeed = 0.5f;
    public float attackRadius = 5f;
    public float checkDist;
    public Transform cylinderTransform; 

    public GameObject projectileTemplate;
    public float timeToIntercept = 1f;
    public float fireRate = 0.4f;
    private bool canShoot = true;
    private Vector3 playerVelocity;
    private Vector3 projectileVelocity;

    private GameObject player;
    private CharacterController controller;
    private Vector3 enemyVelocity;
    private bool jumpBool=true;
    private Vector3 initCylinderScale = new Vector3(0.1f,0.1f,0.1f);

    private bool attackMode;
    private int attackType = 0;
    private bool isCoroutineStarted = false;

    private bool groundedEnemy;
    private float gravityValue = 10*-9.81f;
    private Vector3 attackVector = Vector3.zero;
    private Vector3 normalizedAttackVector = Vector3.zero;
	bool bBossSound = false;


	void Start() {
        controller = GetComponent<CharacterController>();
        player = GameObject.Find("ShooterManager").GetComponent<ShooterManager>().GetPlayer();
    }

    void Update() {
        if (Time.timeScale == 0) return;

        CheckFallToDeath();

        // don't let enemy fall through floor
        groundedEnemy = controller.isGrounded;
        if  (groundedEnemy && enemyVelocity.y < 0) {
            enemyVelocity.y = 0f;
        }
        
        attackMode = CheckAttack();
		//Debug.Log(attackMode + ":" + attackType);

		//MapBGM -> Boss BGM....
		if(!SoundManager.ins.bossSoundOn && attackMode && attackType == 0 && !bBossSound)
		{
			bBossSound = true;
			SoundManager.ins.PlayBoss();
		}


		if (attackMode) {
            // rotate enemy to look at character
            normalizedAttackVector.y = 0;               
            transform.rotation = Quaternion.LookRotation(normalizedAttackVector);

			if (attackType==0) {
                // chase attack
                if (!isCoroutineStarted) StartCoroutine(SwitchMode(5f));
                
                // rotate enemy to look at character
                // normalizedAttackVector.y = 0;               
                // transform.rotation = Quaternion.LookRotation(normalizedAttackVector);

                // move towards character in direction of attackVector
                Vector3 move = normalizedAttackVector*attackSpeed*Time.deltaTime;
                controller.Move(move);

            } else if (attackType==1) {
                // jump attack, jump first
                if (!isCoroutineStarted) StartCoroutine(SwitchMode(0.8f));
                if (jumpBool) {
                    enemyVelocity.y += Mathf.Sqrt(8.0f * -3.0f * gravityValue);
                    jumpBool = false;
                }
            } else if (attackType==2) {
                // jump attack, shockwave
                if (!isCoroutineStarted) StartCoroutine(SwitchMode(2f));
                float speed = 7f;
                Vector3 scaleChange = new Vector3 (speed,0f,speed);
                cylinderTransform.localScale += scaleChange*Time.deltaTime;
            } else {
                // shoot attack, player doesn't move but shoots bullets;
                if (!isCoroutineStarted) StartCoroutine(SwitchMode(5f));

                // rotate enemy to look at character
                // normalizedAttackVector.y = 0;
                // transform.rotation = Quaternion.LookRotation(normalizedAttackVector);

                // shoot fireballs
                if (canShoot == true) {
                    playerVelocity = PlayerVelocity();
                    // solve projectile trajectory to intercept player
                    projectileVelocity = attackVector/timeToIntercept + playerVelocity + Vector3.up*2.0f;
                    ShootProjectile();
                    canShoot = false;
                    StartCoroutine(Reload());
                }

            } 
        }

        // enforce gravity
        enemyVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(enemyVelocity * Time.deltaTime);
    }

    IEnumerator Reload() {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    Vector3 PlayerVelocity() {
        return this.player.GetComponent<ShooterController>().GetPlayerVelocity();
    }

    void ShootProjectile() {
        GameObject projectile = GameObject.Instantiate<GameObject>(projectileTemplate);
        Physics.IgnoreCollision(projectile.GetComponent<Collider>(), this.GetComponent<Collider>());
        projectile.transform.position = this.transform.position;
        projectile.GetComponent<GargoyleProjectileController>().SetProjectileVelocity(projectileVelocity);
    }

    IEnumerator SwitchMode(float seconds) {
        isCoroutineStarted = true;
        yield return new WaitForSeconds(seconds);
        attackType = (attackType+1) % 4;
        isCoroutineStarted = false;
        jumpBool = true;
        cylinderTransform.localScale = initCylinderScale;
    }

    // finds the distance between enemy and player
    bool CheckAttack() {
        this.attackVector = player.transform.position - this.transform.position;
        float distance = this.attackVector.magnitude;
        this.normalizedAttackVector = attackVector.normalized; 
        return (distance < this.attackRadius && distance > this.checkDist);
    }

    void CheckFallToDeath() {
        if (this.transform.position.y < 70.0f)
            Destroy(this.gameObject);
    }
}
