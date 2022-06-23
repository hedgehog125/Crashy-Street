using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class building : MonoBehaviour
{

    // Start is called before the first frame update
    public float seed;
    public GameObject blackbox;
    void Start()
    {
        seed = GameObject.FindGameObjectWithTag("world generator").GetComponent<Wgen>().seed;
        string joe = Mathf.PerlinNoise(seed + transform.position.x, seed + transform.position.z).ToString();
        string jonathan = $"{joe[5]}";
        int index = int.Parse(jonathan);
        int i = 0;
        foreach(Transform t in transform)
        {
            if (i == index)
            {
                if (t.transform.name != "Cube")
                    t.gameObject.SetActive(true);
                else
                    t.gameObject.SetActive(false);
            }
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
