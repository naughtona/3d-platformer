using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostController : MonoBehaviour
{
    public float attackRadius = 5f;

    private GameObject player;
    private CharacterController controller;
    private Vector3 enemyVelocity;
    public bool attackMode;
    private bool isCoroutineFinished;
    private bool groundedEnemy;
    private float jumpHeight = 2.0f;
    private float jumpThreshold = 0.01f;
    private float gravityValue = -9.81f;
    private Vector3 attackVector = Vector3.zero;

    void Start() {
        controller = GetComponent<CharacterController>();
        player = GameObject.Find("PlayerManager").GetComponent<PlayerManager>().GetPlayer();
    }

    void Update() {
        if (Time.timeScale == 0) return;

        CheckFallToDeath();

        // don't let enemy fall through floor
        groundedEnemy = controller.isGrounded;
        if  (groundedEnemy && enemyVelocity.y < 0) {
            enemyVelocity.y = 0f;
        }
        
        if (!isCoroutineFinished) {
            StartCoroutine(CheckForPlayer());
        }

        if (attackMode) {
            Vector3 positionBefore = this.transform.position; 
            Vector3 groundedAttackVector = attackVector;
            groundedAttackVector.y = 0;

            // rotate enemy to look at character
            Vector3.Normalize(groundedAttackVector);
            transform.rotation = Quaternion.LookRotation(groundedAttackVector);

            if  (groundedEnemy) attackVector = groundedAttackVector;

            // move towards character in direction of attackVector
            Vector3 move = attackVector*GlobalOptions.ghostSpeed*Time.deltaTime;
            controller.Move(move);

            float distanceClosedSinceLastMove = Vector3.Distance(positionBefore,this.transform.position);
            // if no closer since last update then jump 
            if  (groundedEnemy && distanceClosedSinceLastMove<this.jumpThreshold)
                enemyVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        // enforce gravity
        enemyVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(enemyVelocity * Time.deltaTime);
    }

    // only check for player once per second for efficiency
    IEnumerator CheckForPlayer() {
        isCoroutineFinished = true;
        attackMode = CheckAttack();
        yield return new WaitForSeconds(0.25f);
        isCoroutineFinished = false;
    }

    // finds the distance between enemy and player
    bool CheckAttack() {
        this.attackVector = player.transform.position - this.transform.position;
        float distance = Vector3.Magnitude(this.attackVector); 
        this.attackVector = this.attackVector.normalized;
        return (distance < this.attackRadius);
    }

    void CheckFallToDeath() {
        if (this.transform.position.y < 70.0f)
            Destroy(this.gameObject);
    }
}
