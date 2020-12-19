using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public ParticleSystem coinEffect;
	public ParticleSystem ghostEffect;

	private GameObject playerManager;
    private GameObject healthManager;
    private CharacterController controller;
    private Vector3 startPosition;
    private Vector3 playerVelocity = Vector3.zero;
    private Vector3 combinedPlayerVelocity = Vector3.zero;
    private bool groundedPlayer;
    private float gravityValue = -3.0f*9.81f;
    private float yaw = 0f;

    void Start() {
        playerManager = GameObject.Find("PlayerManager");
        healthManager = GameObject.Find("HealthManager");

        controller = gameObject.GetComponent<CharacterController>();
    }

    // use a trigger based on capsule collider collision, this will manage 
    // character and enemy collisions
    void OnTriggerEnter(Collider other) {
		// its me, had to change to the calculation wouldnt be continious
		if (other.gameObject.tag == "Enemy") {
			SoundManager.ins.PlayDamaged();
			healthManager.GetComponent<HealthManager>().ApplyDamage(25);
			Instantiate(ghostEffect, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
			Destroy(other.gameObject);
		}
		else if (other.gameObject.tag == "Ascension")
		{
			//Teleport Here
			SoundManager.ins.PlayTeleport();
			playerManager.GetComponent<PlayerManager>().InvokeLevelFinishedEvent();
		}
		else if (other.gameObject.tag == "Coin")
		{
			playerManager.GetComponent<PlayerManager>().InvokeIncrementScoreEvent();
			Instantiate(coinEffect, other.transform.position, Quaternion.identity);

			SoundManager.ins.PlayCoin();
			//other.GetComponent<CoinController>().DestroyMe();
			Destroy(other.gameObject);
		}
	}

    void Update() {
        if (Time.timeScale == 0) return;
        
        CheckFallToDeath();
        groundedPlayer = controller.isGrounded;
        
        // check for elevator
        Vector3 bottom = this.transform.localPosition - new Vector3(0.0f, 0.12f, 0.0f);
        
        RaycastHit hit;
        Vector3 elevatorVelocity = Vector3.zero;
        // cast a ray downwards 
        if (Physics.Raycast(bottom, new Vector3(0.0f, -1.0f, 0.0f), out hit, 0.3f)) {
            if (hit.transform.gameObject.tag == "Elevator") {
                elevatorVelocity = hit.transform.gameObject.GetComponent<ElevatorController>().GetVelocity();
            }
        }

        // disable controller to rotate around y axis
        controller.enabled = false;
        yaw = Input.GetAxis("Mouse X") * 360.0f * GlobalOptions.sensitivity * Time.deltaTime;
        Quaternion addRot = Quaternion.identity;
        addRot.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
        this.transform.rotation *= addRot;
        controller.enabled = true;

        // // don't fall through ground
        if (groundedPlayer && playerVelocity.y < 0)
            playerVelocity.y = 0f;
        
        // get horizontal and forward backward movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        WASD(x, z);
        controller.Move(new Vector3(elevatorVelocity.x, 0.0f, elevatorVelocity.z) * Time.deltaTime);
        
        // changes the height position of the player..
        if (groundedPlayer && Input.GetButtonDown("Jump")) {
            Jump();
        }

        // applies gravity
        playerVelocity.y += (elevatorVelocity.y+gravityValue) * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void WASD(float x, float z) {
        Vector3 move = this.transform.TransformDirection(x, 0.0f, z);
        move = (move * GlobalOptions.playerSpeed) * Time.deltaTime;
        controller.Move(move);
        combinedPlayerVelocity = (move)/Time.deltaTime;
    }

    public void Jump() {
        playerVelocity.y += Mathf.Sqrt(GlobalOptions.jumpHeight * -3.0f * gravityValue);
    }
    
    void CheckFallToDeath() {
        if (Time.deltaTime !=0 && this.transform.position.y < 38.0f) {
			SoundManager.ins.PlayDamaged();
			playerManager.GetComponent<PlayerManager>().InvokePlayerDiedEvent();
        }
    }

    public void ResetPosition() {
        controller.enabled = false;
        this.transform.position = startPosition;
        this.transform.rotation = Quaternion.identity;
        controller.enabled = true;
    }

    public Vector3 GetPlayerVelocity() {
        return this.combinedPlayerVelocity;
    }

    public void SetStartPosition(Vector3 position) {
        this.startPosition = position;
    }
}
