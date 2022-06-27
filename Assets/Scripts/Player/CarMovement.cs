using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CarMovement : MonoBehaviour {
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

    [Header("Particle effects")]
    [SerializeField] private GameObject halfway;
    [SerializeField] private GameObject threequarters;

    [Header("Health and destruction")]
    [SerializeField] public float health;
    [SerializeField] private Slider healthslider;
    [SerializeField] private float damagemultiplier;
    [SerializeField] private int collisioncap = 2;
    [SerializeField] private GameObject recover;
    [SerializeField] public bool dead;
    [SerializeField] private GameObject deadT;

    [Header("Debug Values")]
    [SerializeField] public int money;
    [SerializeField] private Text moneytext;

    [Header("Debug Values")]
    public Collider[] collisions;
    private Rigidbody rb;

    public bool moveLeft;
    public bool moveRight;

    private bool reversing;
    private bool reverseDir;
    private int reverseTurnTick;
    private int turningSlowdownTick;
    public void MoveLeft() {
        moveLeft = true;
    }
    public void NotMoveLeft()
    {
        moveLeft = false;
    }
    public void MoveRight()
    {
        moveRight = true;
    }
    public void NotMoveRight()
    {
        moveRight = false;
    }
    float StartHealth;
	private void Awake() {
        
        rb = GetComponent<Rigidbody>();
        money = PlayerPrefs.GetInt("money", 0);
        StartCoroutine(moneyloop());
        StartHealth = health;
        healthslider.maxValue = health;

    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.GetComponent<Rigidbody>() != null)
        {
            if(collision.tag != "donodamage")health -= damagemultiplier*(rb.velocity.magnitude + collision.transform.GetComponent<Rigidbody>().velocity.magnitude);
        }
        else
        {
            if (collision.tag != "donodamage") health -= damagemultiplier * rb.velocity.magnitude;
        }
    }
    private IEnumerator moneyloop()
    {
        while (true && !dead)
        {
            money += Mathf.RoundToInt(1*(rb.velocity.magnitude/10));
            yield return new WaitForSeconds(1);
        }
    }
    private void FixedUpdate() {
        if (rb.velocity.magnitude < 1)
        {
            health -= Time.deltaTime;
        }
        healthslider.value = health;
        moneytext.text = $"{money}";
        if(health <= 0)
        {
            dead = true;
        }
        if (!dead)
        {
            //smokeandfire
            if (health < StartHealth / 2) halfway.SetActive(true);
            else halfway.SetActive(false);
            if (health < StartHealth / 4) threequarters.SetActive(true);
            else threequarters.SetActive(false);

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
            collisions = Physics.OverlapBox(transform.position, Vector3.Scale(transform.lossyScale, new Vector3(1.1f, 1.1f, 1.1f)));
            max = reversing ? reverseMaxTurnSpeed : maxTurnSpeed;
            turnVel = Tools.LimitSigned(turnVel, max);
            recover.SetActive(false);
            if (WrapAngle(transform.eulerAngles.x) < MaxAngle && WrapAngle(transform.eulerAngles.x) > -MaxAngle && WrapAngle(transform.eulerAngles.z) < MaxAngle && WrapAngle(transform.eulerAngles.z) > -MaxAngle && collisions.Length >= collisioncap)
            {
                rb.angularVelocity = new Vector3(rb.angularVelocity.x, turnVel, rb.angularVelocity.z); ;
                rb.velocity = vel;
            }
            else
            {
                if (rb.velocity.magnitude < 0.1f)
                {
                    recover.SetActive(true);
                    if (moveLeft || moveRight)
                    {
                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                        transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
                    }
                }
            }
        }
        else
        {
            deadT.SetActive(true);
            if (moveLeft || moveRight)
            {
                PlayerPrefs.SetInt("money", money);
                SceneManager.LoadScene("Main Menu");
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
