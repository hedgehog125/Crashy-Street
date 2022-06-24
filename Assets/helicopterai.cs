using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class helicopterai : MonoBehaviour
{
    public float speed;
    public float launchspeed;
    public float destroyY;
    public GameObject barrel;
    public Transform helimodel;
    GameObject player;
    Vector3 startingpos;
    bool goback;
    // Start is called before the first frame update
    void Start()
    {
        speed *= PlayerPrefs.GetInt("maxspeed", 1);
        startingpos = transform.position;
        player = GameObject.FindGameObjectWithTag("Car");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > destroyY)
        {
            Destroy(this.gameObject);
        }
        if (!goback)
        {
            NavMeshPath path = new NavMeshPath();
            Vector3 target;
            if (NavMesh.CalculatePath(player.transform.position, player.transform.position, NavMesh.AllAreas, path))
            {
                target = path.corners[0];
            }
            else
            {
                target = player.transform.position;
            }
            
            target.y = transform.position.y;
            transform.LookAt(target);
            transform.position += transform.forward * speed * Time.deltaTime;
            if (Vector3.Distance(player.transform.position, transform.position) < 1f)
            {
                GameObject barrl = Instantiate(barrel, helimodel.transform.position, transform.rotation);
                barrl.GetComponent<Destructible>().explodeonimpact = true;
                barrl.transform.LookAt(player.transform.position);
                barrl.GetComponent<Rigidbody>().AddForce(barrl.transform.forward * (launchspeed*PlayerPrefs.GetInt("maxspeed",1)));
                goback = true;
            }
        }
        else
        {
            NavMeshPath path = new NavMeshPath();
            Vector3 target;
            if (NavMesh.CalculatePath(player.transform.position, startingpos, NavMesh.AllAreas, path))
            {
                target = path.corners[1];
            }
            else
            {
                target = startingpos;
            }
            target.y = transform.position.y;
            transform.LookAt(target+new Vector3(0,20,0));

            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}
