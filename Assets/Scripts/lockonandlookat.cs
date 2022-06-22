using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockonandlookat : MonoBehaviour
{
    public GameObject lockon;
    public GameObject lookat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = lockon.transform.position;
        transform.LookAt(lookat.transform.position);
    }
}
