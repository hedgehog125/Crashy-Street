using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class carAINav : MonoBehaviour
{
    public GameObject car;
    public float Maxdistance;
    public GameObject player;
    public NavMeshAgent agent;
    public int countdown = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Car");
        StartCoroutine(jeff());
    }

    // Update is called once per frame
    void Update()
    {
        if (car.GetComponent<Rigidbody>().velocity.magnitude > 10)
            agent.speed = (car.GetComponent<Rigidbody>().velocity.magnitude);
        else
            agent.speed = 10;
        if (car.GetComponent<Rigidbody>().velocity.magnitude < 0.1f)
        {
            countdown = 1;
        }
        if(Vector3.Distance(transform.position,car.transform.position) > Maxdistance)
        {
            countdown = 1;
        }
        if(countdown > 0)
        {
            transform.position = car.transform.position + (car.transform.forward * -1);
        }
        else
        {
            agent.SetDestination(player.transform.position);
        }
    }
    IEnumerator jeff()
    {
        while (true)
        {
            countdown--;
            yield return new WaitForSeconds(1f);
        }
    }
}
