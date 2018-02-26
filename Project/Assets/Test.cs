using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Test : MonoBehaviour {

	public bool logEnabled = false;
	// Use this for initialization
	GameObject prefab_white;
	GameObject go;
	void Start () {
		// List<int> list = new List<int> ();
		// int[] array = new int[1000];
		// for (int i = 0; i < 1000; i++) {
		// 	list.Add (i);
		// 	array[i] = i;
		// }
		// // Debug.unityLogger.logEnabled = logEnabled;
		// Stopwatch watch = new Stopwatch ();
		// watch.Start ();
		// list.ForEach ((x) => x++);
		// watch.Stop ();
		// UnityEngine.Debug.LogFormat ("list ForEach:{0}", watch.Elapsed.Ticks);

		// watch.Reset ();

		// watch.Start ();
		// for (int i = 0; i < 1000; i++) {
		// 	list[i]++;
		// }
		// watch.Stop ();
		// UnityEngine.Debug.LogFormat ("list for:{0}", watch.Elapsed.Ticks);

		// watch.Reset ();

		// watch.Start ();
		// for (int i = 0; i < 1000; i++) {
		// 	array[i]++;
		// }
		// watch.Stop ();
		// UnityEngine.Debug.LogFormat ("array:{0}", watch.Elapsed.Ticks);
		prefab_white = Resources.Load<GameObject> ("test_PoolManager/White_Sphere");
		go = GameObject.Instantiate (prefab_white);
	}

	// Update is called once per frame
	void Update () {
		// for (int i = 0; i < 100; i++) {
		// 	// Debug.Log ("Show");
		// }
		if (Input.GetKeyDown (KeyCode.A)) {
			ProxyManager.Instance.Get<TestProxy> ();
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			ProxyManager.Instance.Remove<TestProxy> ();
		}
	}
}

public class TestProxy : DataProxy {
	public override void OnRegister () {
		UnityEngine.Debug.Log ("Test Proxy Register");
	}

	public override void OnRemove () {
		UnityEngine.Debug.Log ("Test Proxy Remove");
	}
}