using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyspawner : MonoBehaviour
{
    public GameObject player;
    public int difficulty = 2;
    public GameObject[] enemys;
    public float[] mult;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnloop());
        StartCoroutine(difficultyloop());
    }
    IEnumerator spawnloop()
    {
        while (true)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("aicar");
            var vec = player.transform.position;
            vec.x = Mathf.Round(vec.x / 60) * 60;
            vec.y = Mathf.Round(vec.y / 60) * 60;
            vec.z = Mathf.Round(vec.z / 60) * 60;
            mult[0] = 1; 
            mult[1] = -1;
            
            if (objects.Length < difficulty)
            {
                int random = Random.Range(0, 2);
                Debug.Log(random);
                GameObject jeff = Instantiate(enemys[random],new Vector3(vec.x+mult[Random.Range(0,1)]*70, enemys[random].transform.position.y, vec.z + mult[Random.Range(0, 1)] * 70),transform.rotation);
                jeff.transform.LookAt(player.transform.position);
                try { jeff.GetComponent<Rigidbody>().velocity = jeff.transform.forward * 2; } catch { }
            }
            int corner;

            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator difficultyloop()
    {
        while (true)
        {
            yield return new WaitForSeconds(60);
            difficulty += 2;
        }
    }
}
