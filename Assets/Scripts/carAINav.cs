using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class carAINav : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] public float acceleration;
    [SerializeField] public float maxSpeed;
    [SerializeField] private float reverseAcceleration;
    [SerializeField] private float reverseMaxSpeed;
    [SerializeField] private float MaxAngle;


    [Header("Turning slowdown")]
    [SerializeField] private float turningMoveSpeedMaintenance;
    [SerializeField] private int turningSlowdownTime;

    [Header("Turning")]
    [SerializeField] public float turnAcceleration;
    [SerializeField] private float reverseTurnAcceleration;
    [SerializeField] private float maxTurnSpeed;
    [SerializeField] private float reverseMaxTurnSpeed;
    [SerializeField] private int reverseTurnDelay;

    public bool moveLeft;
    public bool moveRight;

    private bool reversing;
    private bool reverseDir;
    private int reverseTurnTick;
    private int turningSlowdownTick;

    [Header("ai stuff")]
    public float maxdistance;
    public float deletey;
    GameObject player;
    public NavMeshAgent target;
    public GameObject pointer;
    public Quaternion thecodemaster;

    [Header("debug values")]


    Rigidbody rb;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Car");
        rb = GetComponent<Rigidbody>();
        maxSpeed *= PlayerPrefs.GetInt("speed", 2);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.position.y < -deletey || transform.position.y > deletey)
        {
            Destroy(this.gameObject);
        }
        if (Vector3.Distance(player.transform.position, transform.position) > maxdistance)
        {
            this.gameObject.layer = 9;
        }
        else
        {
            this.gameObject.layer = 8;
        }
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, player.transform.position, NavMesh.AllAreas, path);
        try 
        { 
            pointer.transform.LookAt(path.corners[1]);

        }
        catch 
        {
            pointer.transform.LookAt(player.transform.position);

        }
        if(pointer.transform.localEulerAngles.y > 255)
        {
            moveLeft = true;
            moveRight = false;
        }
        else if (pointer.transform.localEulerAngles.y < 80)
        {
            moveLeft = false;
            moveRight = true;
        }
        else
        {
            moveLeft = true;
            moveRight = true;
        }
        

        Vector3 vel = rb.velocity;
        float turnVel = rb.angularVelocity.y;

        Vector3 dir = transform.forward;
        dir.y = 0; // No flying

        if (moveLeft && moveRight)
        {
            if (!reversing)
            {
                rb.velocity = new Vector3(0, 0, 0);
                rb.angularVelocity = new Vector3(0, 0, 0);
                reverseDir = Random.Range(0, 2) == 1;

                reversing = true;
                reverseTurnTick = 0;

                turningSlowdownTick = 0;
            }
        }
        else
        {
            reversing = false;
            if (moveLeft || moveRight)
            {
                float turnSpeed = turnAcceleration;
                if (moveLeft)
                {
                    turnSpeed *= -1;
                }
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
            else
            {
                turningSlowdownTick = 0;
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
        Collider[] collisions = Physics.OverlapBox(transform.position, Vector3.Scale(transform.lossyScale, new Vector3(1.1f, 1.1f, 1.1f)));
        max = reversing ? reverseMaxTurnSpeed : maxTurnSpeed;
        turnVel = Tools.LimitSigned(turnVel, max);
        if (WrapAngle(transform.eulerAngles.x) < MaxAngle && WrapAngle(transform.eulerAngles.x) > -MaxAngle && WrapAngle(transform.eulerAngles.z) < MaxAngle && WrapAngle(transform.eulerAngles.z) > -MaxAngle && collisions.Length >= 2)
        {
            rb.angularVelocity = new Vector3(rb.angularVelocity.x, turnVel, rb.angularVelocity.z); ;
            rb.velocity = vel;
        }
        else
        {
            if (rb.velocity.magnitude < 0.1f)
            {
                if (moveLeft || moveRight)
                {
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                    transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
                }
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
