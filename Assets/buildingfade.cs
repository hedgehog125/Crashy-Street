using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingfade : MonoBehaviour
{
    public LayerMask mask;
    public Transform camera;
    public float a;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera.position);
        
        Vector3 pos = new Vector3();
        RaycastHit hit;
        if(Physics.Raycast(transform.position,transform.forward,out hit, 1000, mask))
        {
            pos = hit.transform.position;
        }
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("building");
        foreach(GameObject building in buildings)
        {
            foreach (Transform t in building.transform)
            {
                t.gameObject.SetActive(true);
            }
            if (Mathf.RoundToInt(building.transform.position.x) == Mathf.RoundToInt(pos.x) && Mathf.RoundToInt(building.transform.position.z) == Mathf.RoundToInt(pos.z))
            {
                foreach(Transform t in building.transform)
                {
                    t.gameObject.SetActive(false);
                }
            }
        }
        
    }
}
