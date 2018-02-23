using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPool : MonoBehaviour {

	private void Start () {
		GameObject prefab_white = Resources.Load<GameObject> ("test_PoolManager/White_Sphere");
		GameObject prefab_Red = Resources.Load<GameObject> ("test_PoolManager/Red_Sphere");
		GameObject prefab_Cube = Resources.Load<GameObject> ("test_PoolManager/Cube");
		PoolManager.Instance.AddLoadInfo (prefab_white, 10, true, "sphere");
		PoolManager.Instance.AddLoadInfo (prefab_Red, 10, true, "sphere");
		PoolManager.Instance.AddLoadInfo (prefab_Cube, 10, true);
		PoolManager.Instance.StartLoadAllInfos ();
	}

	private void OnGUI () {
		if (GUILayout.Button ("Active Object(White_Sphere)")) {
			UnityObjectPool pool = PoolManager.Instance.GetPool ("White_Sphere");
			GameObject go = pool.Get ();
			go.SetActive (true);
		}
		if (GUILayout.Button ("Active Object(Red_Sphere)")) {
			UnityObjectPool pool = PoolManager.Instance.GetPool ("Red_Sphere");
			GameObject go = pool.Get ();
			go.SetActive (true);
			Timer.Startup (3, (t) => {
				pool.Recycle (go);
			});
		}
		if (GUILayout.Button ("Active Object(Cube)")) {
			UnityObjectPool pool = PoolManager.Instance.GetPool ("Cube");
			GameObject go = pool.Get ();
			go.SetActive (true);
			Timer.Startup (3, (t) => {
				pool.Recycle (go);
			});
		}
		if (GUILayout.Button ("Release Pool(Group:sphere)")) {
			PoolManager.Instance.ReleasePoolsByGroup ("sphere", true);
		}
		if (GUILayout.Button ("Release Pool(Group:Default)")) {
			PoolManager.Instance.ReleasePoolsByDefaultGroup ();
		}
	}

	private void OnDestroy () {
		PoolManager.Instance.ReleaseAllPools ();
	}
}