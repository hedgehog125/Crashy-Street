using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxScript : MonoBehaviour
{
    public float explosionrad;
    public float explosionforce;
    public GameObject whole;
    public GameObject broken;
    public ParticleSystem boom;
    public AudioSource boombs;
    bool bools;

    void Start()
    {
        whole.SetActive(true);
        broken.SetActive(false);
    }


    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Car" && !bools)
        {
            anything(false);
        }
    }

    public void anything(bool exp)
    {
        whole.SetActive(false);
        broken.SetActive(true);
        if (boom != null) boom.Play();
        if (boombs != null) boombs.Play();
        if (!exp) Invoke("explode", 0.01f);
        bools = true;
        Invoke("destroy", 3);

    }

    private void destroy()
    {
        Destroy(this.gameObject);
    }
    private void explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionrad);
        foreach (Collider gay in hitColliders)
        {
            if (gay.GetComponent<Rigidbody>() != null && boom != null) gay.GetComponent<Rigidbody>().AddExplosionForce(explosionforce, transform.position, explosionrad);
            if (gay.GetComponent<boxScript>() != null) gay.GetComponent<boxScript>().anything(true);
        }

    }
}
