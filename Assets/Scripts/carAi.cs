using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carAi : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float reverseAcceleration;
    [SerializeField] private float reverseMaxSpeed;
    [SerializeField] private float MaxAngle;


    [Header("Turning slowdown")]
    [SerializeField] private float turningMoveSpeedMaintenance;
    [SerializeField] private int turningSlowdownTime;

    [Header("Turning")]
    [SerializeField] private float turnAcceleration;
    [SerializeField] private float reverseTurnAcceleration;
    [SerializeField] private float maxTurnSpeed;
    [SerializeField] private float reverseMaxTurnSpeed;
    [SerializeField] private int reverseTurnDelay;

    [Header("misc")]
    public float maxdistance;
    public int collisioncap = 2;
    public GameObject pointer;
    public GameObject agent;

    [Header("Debug Values")]
    public Collider[] collisions;
    private Rigidbody rb;

    private bool moveLeft;
    private bool moveRight;

    private bool reversing;
    private bool reverseDir;
    private int reverseTurnTick;
    private int turningSlowdownTick;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    bool canTurn;


    private void FixedUpdate()
    {
        pointer.GetComponent<lockonandlookat>().lookat = agent;
        Vector3 vel = rb.velocity;
        float turnVel = rb.angularVelocity.y;
        Vector3 dir = transform.forward;
        dir.y = 0; // No flying
        if (pointer.transform.localEulerAngles.y<255&& pointer.transform.localEulerAngles.y > 105)
        {
            if (!reversing)
            {
                reverseDir = Random.Range(0, 2) == 1;

                reversing = true;
                reverseTurnTick = 0;

                turningSlowdownTick = 0;
            }
        }
        else
        {
            reversing = false;
            float turnSpeed = turnAcceleration;

            if (pointer.transform.localEulerAngles.y > 255)
            {
                turnSpeed *= -1;
            }
            if(pointer.transform.localEulerAngles.y > 1 && pointer.transform.localEulerAngles.y < 359)
                turnVel += turnSpeed;

            if (turningSlowdownTick == turningSlowdownTime)
            {
                vel.x *= turningMoveSpeedMaintenance;
                vel.z *= turningMoveSpeedMaintenance;
            }
            else
            {
                turningSlowdownTick++;
            }
        }
        if (reversing)
        {
            dir *= -1;
            if (reverseTurnTick == 0)
            {
                if (vel.x > 0 == dir.x > 0 && vel.y > 0 == dir.y > 0)
                {
                    reverseTurnTick = 1;
                }
            }
            if (reverseTurnTick != 0)
            {
                if (reverseTurnTick == reverseTurnDelay)
                {
                    turnVel += reverseTurnAcceleration * (reverseDir ? 1 : -1);
                }
                else
                {
                    reverseTurnTick++;
                }
            }
        }

        vel += dir * (reversing ? reverseAcceleration : acceleration);
        float max = reversing && reverseTurnTick != 0 ? reverseMaxSpeed : maxSpeed;
        vel = Tools.LimitXZ(vel, max);
        collisions = Physics.OverlapBox(transform.position, Vector3.Scale(transform.lossyScale, new Vector3(1.1f, 1.1f, 1.1f)));
        max = reversing ? reverseMaxTurnSpeed : maxTurnSpeed;
        turnVel = Tools.LimitSigned(turnVel, max);
        if (WrapAngle(transform.eulerAngles.x) < MaxAngle && WrapAngle(transform.eulerAngles.x) > -MaxAngle && WrapAngle(transform.eulerAngles.z) < MaxAngle && WrapAngle(transform.eulerAngles.z) > -MaxAngle && collisions.Length >= 2)
        {
            rb.angularVelocity = new Vector3(rb.angularVelocity.x, turnVel, rb.angularVelocity.z); ;
            rb.velocity = vel;
        }
        else
        {
            if (rb.velocity.magnitude < 1f)
            {
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
            }
        }
    }
    private static float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }
}
