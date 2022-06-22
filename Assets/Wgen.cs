using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wgen : MonoBehaviour
{
    public GameObject player;
    public NavMeshSurface surface;
    public float seed;
    public GameObject[] roadpatterns;
    public Vector3[] posses = new Vector3[9];
    // Start is called before the first frame update
    void Start()
    {
        seed = Random.Range(1000f, 10000f);
    }

    // Update is called once per frame
    Vector3 prevvec;
    void Update()
    {
        
        int i = 0;
        var vec = player.transform.position;
        vec.x = Mathf.Round(vec.x / 60) * 60;
        vec.y = Mathf.Round(vec.y / 60) * 60;
        vec.z = Mathf.Round(vec.z / 60) * 60;
        posses[0] = new Vector3(vec.x - 60, 0, vec.z + 60);
        posses[1] = new Vector3(vec.x, 0, vec.z + 60);
        posses[2] = new Vector3(vec.x+60, 0, vec.z + 60);
        posses[3] = new Vector3(vec.x - 60, 0, vec.z);
        posses[4] = new Vector3(vec.x, 0, vec.z);
        posses[5] = new Vector3(vec.x + 60, 0, vec.z);
        posses[6] = new Vector3(vec.x - 60, 0, vec.z-60);
        posses[7] = new Vector3(vec.x, 0, vec.z-60);
        posses[8] = new Vector3(vec.x + 60, 0, vec.z-60);
        GameObject[] patterns = GameObject.FindGameObjectsWithTag("roadpattern");
        foreach (GameObject objects in patterns)
        {
            bool inposses = false;
            foreach (Vector3 vectors in posses)
            {
                if (vectors == objects.transform.position) inposses = true;
            }
            if (!inposses)
            {
                Destroy(objects);
            }
        }
        foreach (Vector3 vectors in posses)
        {
            bool inpatterns = false;
            foreach (GameObject Objects in patterns)
            {
                if (Objects.transform.position == vectors) inpatterns = true;
            }
            string joe = Mathf.PerlinNoise(seed+vectors.x, seed+vectors.z).ToString();
            string jonathan = $"{joe[5]}";
            if (!inpatterns) Instantiate(roadpatterns[int.Parse(jonathan)], vectors, transform.rotation);
        }
        if (vec != prevvec)
        {
            Debug.Log("buildnav");
            surface.BuildNavMesh();
        }
        prevvec = vec;
    }
}
