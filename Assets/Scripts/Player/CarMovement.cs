using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarMovement : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;


    private Rigidbody rb;

    private bool moveLeft;
    private bool moveRight;

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
        Vector3 turnVel = rb.angularVelocity;

        Vector3 dir = transform.forward;
        dir.y = 0; // No flying
        if (moveLeft && moveRight) {
            dir *= -1;
		}
        else {
            if (moveLeft) {
                turnVel.y -= turnSpeed;
            }
            else if (moveRight) {
                turnVel.y += turnSpeed;
			}
		}
        vel += dir * speed;

        rb.velocity = vel;
        rb.angularVelocity = turnVel;
	}
}
