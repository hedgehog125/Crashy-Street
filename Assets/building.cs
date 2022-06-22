using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class building : MonoBehaviour
{

    // Start is called before the first frame update
    public float seed;
    void Start()
    {
        seed = GameObject.FindGameObjectWithTag("world generator").GetComponent<Wgen>().seed;
        string joe = Mathf.PerlinNoise(seed + transform.position.x, seed + transform.position.z).ToString();
        string jonathan = $"{joe[5]}";
        string joseph = $"{joe[3]}";
        int index = int.Parse(jonathan);
        int i = 0;
        foreach(Transform t in transform)
        {
            if (i == index)
            {
                if (int.Parse(joseph) > 0.5f) ;
                t.gameObject.SetActive(true);
            }
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
