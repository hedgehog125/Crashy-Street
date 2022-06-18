using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableBroken : MonoBehaviour {
    [SerializeField] private int fadeStartTime = 150;
    [SerializeField] private int fadeTime = 25;

	private Transform[] parts;
	private Vector3[] fadeSpeeds;

    private int fadeStartTick;
    private int fadeTick;


	private void Awake() {
		parts = new Transform[transform.childCount];
		fadeSpeeds = new Vector3[transform.childCount];

		for (int i = 0; i < parts.Length; i++) {
			Transform tran = transform.GetChild(i).transform;
			parts[i] = tran;
			fadeSpeeds[i] = tran.localScale / (fadeTime + 1); // + 1 so it doesn't end up at 0
		}
	}

	private void FixedUpdate() {
		if (fadeStartTick == fadeStartTime) {
			if (fadeTick == fadeTime) {
				Destroy(gameObject);
			}
			else {
				for (int i = 0; i < parts.Length; i++) {
					parts[i].localScale -= fadeSpeeds[i];
				}

				fadeTick++;
			}
		}
		else {
			fadeStartTick++;
		}
	}
}
