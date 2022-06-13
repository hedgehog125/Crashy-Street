using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfMass : MonoBehaviour {
    [SerializeField] private Vector3 center;

	private void Awake() {
		GetComponent<Rigidbody>().centerOfMass = center;
	}
}
