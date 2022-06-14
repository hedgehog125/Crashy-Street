using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarMovement : MonoBehaviour {
    [Header("Movement")]
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float reverseAcceleration;
    [SerializeField] private float reverseMaxSpeed;

    [Header("Turning")]
    [SerializeField] private float turnAcceleration;
    [SerializeField] private float reverseTurnAcceleration;
    [SerializeField] private float maxTurnSpeed;
    [SerializeField] private float reverseMaxTurnSpeed;


    private Rigidbody rb;

    private bool moveLeft;
    private bool moveRight;

    private bool reversing;
    private bool reverseDir;

    private void OnMoveLeft(InputValue input) {
        moveLeft = input.isPressed;
    }
    private void OnMoveRight(InputValue input) {
        moveRight = input.isPressed;
    }

	private void Awake() {
        rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate() {
        Vector3 vel = rb.velocity;
        float turnVel = rb.angularVelocity.y;

        Vector3 dir = transform.forward;
        dir.y = 0; // No flying

        if (moveLeft && moveRight) {
            if (! reversing) {
                reverseDir = Random.Range(0, 2) == 1;
                reversing = true;
			}
		}
        else {
            reversing = false;
            if (moveLeft) {
                turnVel -= turnAcceleration;
            }
            else if (moveRight) {
                turnVel += turnAcceleration;
			}
		}
        if (reversing) {
            dir *= -1;
            turnVel += reverseTurnAcceleration * (reverseDir? 1 : -1);
        }

        vel += dir * (reversing? reverseAcceleration : acceleration);
        float max = reversing? reverseMaxSpeed : maxSpeed;
        vel = Tools.LimitXZ(vel, max);

        max = reversing? reverseMaxTurnSpeed : maxTurnSpeed;
        turnVel = Tools.LimitSigned(turnVel, max);

        rb.velocity = vel;
        rb.angularVelocity = new Vector3(rb.angularVelocity.x, turnVel, rb.angularVelocity.z);
	}
}
