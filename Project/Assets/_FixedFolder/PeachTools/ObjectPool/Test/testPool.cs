using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPool : MonoBehaviour {
	public GameObject Sphere;
	public GameObject Cube;
	public GameObject Capsule;
	private void Start () {
		PoolManager.Instance.AddLoadInfo (Sphere, 10, true, "NoCapsule");
		PoolManager.Instance.AddLoadInfo (Cube, 10, true, "NoCapsule");
		PoolManager.Instance.AddLoadInfo (Capsule, 10, true);
		PoolManager.Instance.StartLoadAllInfos ();
	}

	private void OnGUI () {
		if (GUILayout.Button ("Active Object(Sphere)")) {
			UnityObjectPool pool = PoolManager.Instance.GetPool ("Sphere");
			GameObject go = pool.Get ();
			go.SetActive (true);
		}
		if (GUILayout.Button ("Active Object(Cube)")) {
			UnityObjectPool pool = PoolManager.Instance.GetPool ("Cube");
			GameObject go = pool.Get ();
			go.SetActive (true);
			Timer.Startup (3, (t) => {
				pool.Recycle (go);
			});
		}
		if (GUILayout.Button ("Active Object(Capsule)")) {
			UnityObjectPool pool = PoolManager.Instance.GetPool ("Capsule");
			GameObject go = pool.Get ();
			go.SetActive (true);
			Timer.Startup (3, (t) => {
				pool.Recycle (go);
			});
		}
		if (GUILayout.Button ("Release Pool(Group:NoCapsule)")) {
			PoolManager.Instance.ReleasePoolsByGroup ("NoCapsule", true);
		}
		if (GUILayout.Button ("Release Pool(Group:Default)")) {
			PoolManager.Instance.ReleasePoolsByDefaultGroup ();
		}
	}

	private void OnDestroy () {
		PoolManager.Instance.ReleaseAllPools ();
	}
}