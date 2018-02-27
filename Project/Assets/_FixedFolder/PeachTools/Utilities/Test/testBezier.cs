using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBezier : MonoBehaviour {
	Bezier bezier = null;

	public Transform P1, P2, P3, P4;

	private void OnDrawGizmos () {
		bezier = new Bezier (P1.position, P2.position, P3.position, P4.position);
		Vector3[] path = bezier.GetPath (20);
		Gizmos.color = Color.red;
		for (int i = 0; i < path.Length - 1; i++) {
			Gizmos.DrawLine (path[i], path[i + 1]);
		}
		Gizmos.color = Color.blue;
		Gizmos.DrawLine (P1.position, P2.position);
		Gizmos.DrawLine (P3.position, P4.position);
	}
}