using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 碰撞判断
/// </summary>
public static class CollisionDetection {
	/// <summary>
	/// 直线检测
	/// </summary>
	/// <param name="point">起始点</param>
	/// <param name="dir">方向</param>
	/// <param name="lineWidth">检测宽度</param>
	/// <param name="distance">检测距离</param>
	/// <param name="callback">回调</param>
	/// <param name="sort">是否进行排序</param>
	/// <param name="mask">碰撞层</param>
	/// <returns>true - 代表碰撞到了碰撞体</returns>
	public static bool DoLine (Vector3 point, Vector3 dir, float lineWidth, float distance, System.Action<Collider[]> callback, bool sort = false, int mask = -1) {
		Collider[] _colliders = null;

		bool r = DoLine (point, dir, lineWidth, distance, out _colliders, sort, mask);

		if (r)
			callback (_colliders);

		return r;
	}
	/// <summary>
	/// 直线检测
	/// </summary>
	/// <param name="point">起始点</param>
	/// <param name="dir">方向</param>
	/// <param name="lineWidth">检测宽度</param>
	/// <param name="distance">检测距离</param>
	/// <param name="colliders">碰撞到的物体</param>
	/// <param name="sort">是否进行排序</param>
	/// <param name="mask">碰撞层</param>
	/// <returns>true - 代表碰撞到了碰撞体</returns>
	public static bool DoLine (Vector3 point, Vector3 dir, float lineWidth, float distance, out Collider[] colliders, bool sort = false, int mask = -1) {
		bool r = false;
		colliders = null;

		RaycastHit[] hitInfo = Physics.SphereCastAll (point, lineWidth, dir, distance, mask);

		if (hitInfo.Length > 0) {
			r = true;

			if (sort)
				System.Array.Sort (hitInfo, (x, y) => x.distance - y.distance > 0.0f? 1: -1);

			List<Collider> _colliders = new List<Collider> ();
			foreach (RaycastHit hit in hitInfo)
				_colliders.Add (hit.collider);

			colliders = _colliders.ToArray ();
		}

		return r;
	}

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
	/// <returns>true - 代表碰撞到了碰撞体</returns>
	public static bool DoArc (Vector3 point, Vector3 dir, float radius, float angleDegree, System.Action<Collider[]> callback, bool sort = false, int mask = -1) {
		Collider[] _colliders = null;

		bool r = DoArc (point, dir, radius, angleDegree, out _colliders, sort, mask);

		if (r)
			callback (_colliders);

		return r;
	}
	/// <summary>
	/// 扇形（球形）检测
	/// </summary>
	/// <param name="point">检测中心点</param>
	/// <param name="dir">方向</param>
	/// <param name="radius">检测半径</param>
	/// <param name="angleDegree">检测弧度</param>
	/// <param name="colliders">碰撞到的物体</param>
	/// <param name="sort">是否进行排序</param>
	/// <param name="mask">碰撞层</param>
	/// <returns>true - 代表碰撞到了碰撞体</returns>
	public static bool DoArc (Vector3 point, Vector3 dir, float radius, float angleDegree, out Collider[] colliders, bool sort = false, int mask = -1) {
		bool r = false;
		colliders = null;

		Collider[] _colliders = Physics.OverlapSphere (point, radius, mask);

		if (sort)
			System.Array.Sort (_colliders,
				(x, y) => Vector3.Distance (x.transform.position, point).CompareTo (Vector3.Distance (y.transform.position, point)));

		float cos = Mathf.Cos (angleDegree * Mathf.Deg2Rad * 0.5f);

		List<Collider> hitColliders = new List<Collider> ();
		foreach (Collider c in _colliders) {
			Vector3 dirCollider = c.transform.position - point;
			float dot = Vector3.Dot (dirCollider.normalized, dir);
			if (dot >= cos) {
				r = true;
				hitColliders.Add (c);
			}
		}

		if (hitColliders.Count > 0)
			colliders = hitColliders.ToArray ();

		return r;
	}
}