// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class RollingBallPlayerController : MonoBehaviour
// {
//     public float playerSpeed = 18.0f;

//     private GameObject playerManager;
//     private GameObject healthManager;
//     private Rigidbody rb;
//     private Vector3 combinedPlayerVelocity = Vector3.zero;
//     private float yaw = 0f;

//     void Start() {
//         playerManager = GameObject.Find("PlayerManager");
//         healthManager = GameObject.Find("HealthManager");

//         rb = this.gameObject.GetComponent<Rigidbody>();
        
//         healthManager.GetComponent<HealthManager>().ResetHealthToStarting();
//         playerManager.GetComponent<PlayerManager>().InvokeResetScoreEvent();
//     }

//     // use a trigger based on capsule collider collision, this will manage 
//     // character and enemy collisions
//     void OnTriggerEnter(Collider other) {
//         if (other.gameObject.tag == "Enemy") {
//             healthManager.GetComponent<HealthManager>().ApplyDamage(50);
//             Destroy(other.gameObject);
//         }
//         if (other.gameObject.tag == "Ascension")
//             playerManager.GetComponent<PlayerManager>().InvokeLevelFinishedEvent();
        
//         if (other.gameObject.tag == "Coin")
//             playerManager.GetComponent<PlayerManager>().InvokeIncrementScoreEvent();
//             Destroy(other.gameObject);
//     }

//     void Update() {
//         CheckFallToDeath();

//         float x = Input.GetAxis("Horizontal");
//         float z = Input.GetAxis("Vertical");

//         Vector3 movement = new Vector3(x, 0.0f, z);

//         rb.AddForce(movement * playerSpeed);

//         bool isGrounded = Physics.Raycast(transform.position, Vector3.down, 2.0f);

//         if (isGrounded && Input.GetButton("Jump"))
//             rb.AddForce(Vector3.up * 2.0f, ForceMode.Impulse);

//         yaw = Input.GetAxis("Mouse X") * 360.0f * Time.deltaTime;
//         Quaternion addRot = Quaternion.identity;
//         addRot.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
//         this.transform.rotation *= addRot;

//         combinedPlayerVelocity = rb.velocity;
//     }
    
//     void CheckFallToDeath() {
//         if (this.transform.position.y < 0.0f)
//             playerManager.GetComponent<PlayerManager>().InvokePlayerDiedEvent();
//     }

//     public Vector3 GetPlayerVelocity() {
//         return this.combinedPlayerVelocity;
//     }
// }
