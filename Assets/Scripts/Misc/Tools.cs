using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools { // From Backfire if that matters
	public static float LimitSigned(float value, float limit) {
		return Mathf.Min(Mathf.Abs(value), limit) * (value > 0 ? 1 : -1);
	}
	public static Vector3 LimitXZ(Vector3 vec, float limit) {
		Vector2 vec2 = new Vector2(vec.x, vec.z);
		if (vec2.magnitude > limit) vec2 = vec2.normalized * limit;

		return new Vector3(vec2.x, vec.y, vec2.y);
	}

	public static float AngleToPoint(Vector2 position, Vector2 target) { // https://answers.unity.com/questions/1350050/how-do-i-rotate-a-2d-object-to-face-another-object.html
		return Mathf.Atan2(target.y - position.y, target.x - position.x) * Mathf.Rad2Deg;
	}
}