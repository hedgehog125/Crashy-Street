using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {
    [Header("Objects and references")]
    [SerializeField] private GameObject brokenPrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioSource explosionSound;

    [Header("Explosion")]
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionDamage;

    [Header("")]
    [SerializeField] private float breakThreshold;
    [SerializeField] private int cost;

    private Rigidbody rb;
    private Vector3 lastVel;

	private void Awake() {
        rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate() {
        float acceleration = (lastVel - rb.velocity).magnitude;
        if (acceleration >= breakThreshold) Break();

        if (rb.velocity.magnitude >= breakThreshold) Break();
    }

    private void Break() {
        if (explosionPrefab != null) {
            GameObject prefab = Instantiate(explosionPrefab, transform.position, transform.rotation);
		}
        if (explosionSound != null) explosionSound.Play();

        // Replace this with a broken version. Do it now so it's affected by the explosion
        Instantiate(brokenPrefab, transform.position, transform.rotation);
        Destroy(gameObject);

        Explode();
    }

    private void Explode() {
        Collider[] affectedObs = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider ob in affectedObs) {
            if(ob.transform.tag == "Car")
            {
                ob.GetComponent<CarMovement>().health -= explosionDamage/4;
                ob.GetComponent<CarMovement>().money += cost/4;
            }
            if (ob.gameObject == gameObject) continue;

            Rigidbody obRb = ob.GetComponent<Rigidbody>();
             
            if (obRb != null) {
                obRb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
    }
}
