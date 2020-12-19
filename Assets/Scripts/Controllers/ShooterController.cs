using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShooterController : MonoBehaviour
{
    public ParticleSystem coinEffect;
	public ParticleSystem ghostEffect;
    public GameObject snowballTemplate;
    public int bossDamage = 25;

	private GameObject playerManager;
    private GameObject healthManager;
    private CharacterController controller;
    private Vector3 startPosition;
    private Vector3 playerVelocity = Vector3.zero;
    private Vector3 combinedPlayerVelocity = Vector3.zero;
    private bool groundedPlayer;
    private float gravityValue = -3.0f*9.81f;
    private float yaw = 0f;
    private bool isDisabled = false;
    private bool loaded = true;

    void Start() {
        playerManager = GameObject.Find("ShooterManager");
        healthManager = GameObject.Find("HealthManager");

        controller = gameObject.GetComponent<CharacterController>();
    }

    // use a trigger based on capsule collider collision, this will manage 
    // character and enemy collisions
    void OnTriggerEnter(Collider other) {
		// its me, had to change to the calculation wouldnt be continious
		if (loaded && other.gameObject.tag == "Boss") {
            loaded = false;
            StartCoroutine(ApplyDamage());
			SoundManager.ins.PlayDamaged();
		} else if (other.gameObject.tag == "Enemy") {
            healthManager.GetComponent<HealthManager>().ApplyDamage(bossDamage);
			Instantiate(ghostEffect, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
			SoundManager.ins.PlayDamaged();
		} else if (other.gameObject.tag == "Ascension") {
			playerManager.GetComponent<ShooterManager>().InvokeLevelFinishedEvent();
			SoundManager.ins.PlayTeleport();
		} else if (other.gameObject.tag == "Coin") {
			playerManager.GetComponent<ShooterManager>().InvokeIncrementScoreEvent();
			Instantiate(coinEffect, other.transform.position, Quaternion.identity);
			SoundManager.ins.PlayCoin();
			Destroy(other.gameObject);
		}
	}

    IEnumerator ApplyDamage() {
        healthManager.GetComponent<HealthManager>().ApplyDamage(bossDamage);
        Instantiate(ghostEffect, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        loaded = true;
    }

    void Update() {
        if (Time.timeScale == 0 || isDisabled) return;
        
        CheckFallToDeath();
        CheckForSpell();
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
        yaw = Input.GetAxis("Mouse X") * 360.0f * Time.deltaTime;
        Quaternion addRot = Quaternion.identity;
        addRot.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
        this.transform.rotation *= addRot;
        controller.enabled = true;

        // don't fall through ground
        if (groundedPlayer && playerVelocity.y < 0)
            playerVelocity.y = 0f;
        
        // get horizontal and forward backward movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        WASD(x, z);
        controller.Move(new Vector3(elevatorVelocity.x, 0.0f, elevatorVelocity.z) * Time.deltaTime);
        
        // changes the height position of the player..
        if (groundedPlayer && Input.GetButtonDown("Jump"))
            Jump();

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
			playerManager.GetComponent<ShooterManager>().InvokePlayerDiedEvent();
        }
    }

    void CheckForSpell() {
        if (Input.GetMouseButtonDown(0)) {
           ShootSnowball();
        }
    }

    public void ShootSnowball() {
		Vector3 _pos = this.transform.position + Vector3.up * 2.0f;
		Quaternion _rot = this.transform.rotation;

		GameObject snowball = GameObject.Instantiate<GameObject>(snowballTemplate, _pos, _rot);
		SoundManager.ins.PlayShot();
        Physics.IgnoreCollision(snowball.GetComponent<Collider>(), controller);
        //snowball.transform.position = this.transform.position + Vector3.up*2.0f;
        //snowball.GetComponent<SnowballController>().rotation = ;
        // pass player velocity to snowball
        snowball.GetComponent<SnowballController>().shooterSpeed = combinedPlayerVelocity.magnitude;
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

    public void SetDisabled(bool isDisabled) {
        this.isDisabled = isDisabled;
    }
}
