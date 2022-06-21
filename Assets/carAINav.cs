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
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Car");
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position,car.transform.position) > Maxdistance)
        {
            transform.localPosition = car.transform.position + (car.transform.forward * 3);
        }
        agent.SetDestination(player.transform.position);
    }
}
