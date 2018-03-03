using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionDetection {
	/// <summary>
	/// 扇形（球形）检测
	/// </summary>
	/// <param name="point">检测中心点</param>
	/// <param name="dir">方向</param>
	/// <param name="radius">检测半径</param>
	/// <param name="angleDegree">检测弧度</param>
	/// <param name="callback">回调</param>
	/// <param name="sort">是否进行排序</param>
	/// <param name="mask">碰撞层</param>
	/// <returns></returns>
	public static bool DoArcCollisionDetection (Vector3 point, Vector3 dir, float radius, float angleDegree, System.Action<Collider[]> callback,
		bool sort = false, int mask = -1) {
		bool r = false;

		Collider[] collids = Physics.OverlapSphere (point, radius, mask);

		if (sort)
			System.Array.Sort (collids,
				(x, y) => Vector3.Distance (x.transform.position, point).CompareTo (Vector3.Distance (y.transform.position, point)));

		float cos = Mathf.Cos (angleDegree * Mathf.Deg2Rad * 0.5f);

		List<Collider> hitColliders = new List<Collider> ();
		foreach (Collider c in collids) {
			Vector3 dirCollider = c.transform.position - point;
			float dot = Vector3.Dot (dirCollider.normalized, dir);
			if (dot >= cos) {
				r = true;
				hitColliders.Add (c);
			}
		}

		if (hitColliders.Count > 0)
			callback (hitColliders.ToArray ());

		return r;
	}
}