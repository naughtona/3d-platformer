using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour {
    // common params
    public float movementLength = 2f;
    private float halfMovementLength;
    public float movementSpeed = 1f;
    public string[] movements = new[] {
        "back and forth",
        "square movement",
        "water wheel"
    };
    [Range(1, 3)]
    public int movementModel;
    private Vector3 velocity = Vector3.zero;

    // back and forth params
    public Vector3 movementDirection = new Vector3(0.0f,1.0f,0.0f);
    private int dir = 1;
    private Vector3 startPosition;
    private Vector3 midpoint;

    // square movement params
    public bool clockwise;
    public int initialIndex = 0;
    private Vector3[] squareMovements;
    private int squareMovementIndex=0;
    private Vector3 squareStartPoint;

    // water wheel params
    public float radius;
    private Vector3 pivotPointInWorldSpace;
    private Vector3 axisInWorldSpace;
    private float radiusSquared;
    public float rotationSpeed = 90f;
    private float currentAngle = 0.0f;

    void Start() {
        halfMovementLength = movementLength/2;
        startPosition = this.transform.localPosition;
        Vector3.Normalize(movementDirection);
        midpoint = startPosition + halfMovementLength * movementDirection;

        // define square direction vectors
        if (clockwise) {
            squareMovements = new [] { 
            new Vector3(0f,0f,1f),
            new Vector3(1f,0f,0f), 
            new Vector3(0f,0f,-1f),
            new Vector3(-1f,0f,0f) };
        } else {
            squareMovements = new [] { 
            new Vector3(1f,0f,0f), 
            new Vector3(0f,0f,1f),
            new Vector3(-1f,0f,0f),
            new Vector3(0f,0f,-1f) };
        }

        squareMovementIndex = initialIndex;

        squareStartPoint = startPosition;
        radiusSquared = this.radius*this.radius;
        pivotPointInWorldSpace = startPosition + Vector3.forward * this.radius;
        axisInWorldSpace = Vector3.right;
    }
    
    void Update () {
        if (Time.timeScale == 0) return;

        if (movementModel == 1) {
            this.velocity = dir * movementSpeed * movementDirection;
            BackAndForth();
        } else if (movementModel == 2)  {
            this.velocity = movementSpeed * squareMovements[squareMovementIndex];
            SquareMovement();
        } else {
            this.velocity = rotationSpeed * GetClockwiseWaterWheelTangent();
            WaterWheel();
        }
    }

    Vector3 GetClockwiseWaterWheelTangent() {
        Vector3 tangent = Vector3.zero;
        float localZ = this.transform.position.z-pivotPointInWorldSpace.z;
        if (localZ==this.radius) {
            tangent = new Vector3(0f, -1f, 0f);
        } else if (-localZ==this.radius) {
            tangent = new Vector3(0f, 1f, 0f);
        } else {
            float derivative = localZ/Mathf.Sqrt(this.radiusSquared-localZ*localZ);
            if (this.transform.localPosition.y-pivotPointInWorldSpace.y > 0) {
                tangent = new Vector3(0.0f,-derivative,1.0f).normalized;
            } else {
                tangent = new Vector3(0.0f, derivative,1.0f).normalized;
            }
       }
       return tangent;
    }

    // platform moves back and forth in given direction
    void BackAndForth() {
        float oldDistance = Vector3.Distance(this.transform.localPosition, midpoint);

        this.transform.localPosition += this.velocity * Time.deltaTime;
        float newDistance = Vector3.Distance(this.transform.localPosition, midpoint);
        if ((oldDistance <= halfMovementLength) && (newDistance > halfMovementLength)) {
            dir = -dir;    
        } 
    }

    // platform moves in a square pattern
    void SquareMovement() {
        this.transform.localPosition += velocity * Time.deltaTime;
        float distance = Vector3.Distance(this.transform.localPosition, squareStartPoint);
        if (distance > movementLength) {
            squareStartPoint += movementLength*squareMovements[squareMovementIndex];
            squareMovementIndex = squareMovementIndex == 3 ? 0 : squareMovementIndex+1;
        }
    }

    // platform moves in water wheel motion
    void WaterWheel() {
        this.transform.localPosition = startPosition;
        this.transform.RotateAround(pivotPointInWorldSpace, axisInWorldSpace, this.currentAngle);

        // unrotate the object itself
        this.transform.localRotation = Quaternion.identity;

        this.currentAngle += Time.deltaTime * this.rotationSpeed;
    }

    public Vector3 GetVelocity() {
        return this.velocity;
    }

}
