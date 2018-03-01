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
		ListKV<string, int> list = new ListKV<string, int> ();
		list.Add ("A", 382);
		list.Add ("B", 749);
		list.Add ("C", 482);
		list.Add ("D", 082);

		UnityEngine.Debug.Log (Dice.GetWinner (list, null, 0));

		int[] arr = new int[4] { 483, 3498, 4839, 23 };
		UnityEngine.Debug.Log (Dice.GetWinner (arr, null, 0));

		UnityEngine.Debug.Log (Dice.GetResult (4739, null, 0));

		// List<int> list = new List<int> ();
		// int[] array = new int[1000];
		// ListKV<string, int> kv = new ListKV<string, int> ();
		// for (int i = 0; i < 1000; i++) {
		// 	kv.Add (new KV<string, int> (i.ToString (), i));
		// }
		// // Debug.unityLogger.logEnabled = logEnabled;
		// Stopwatch watch = new Stopwatch ();
		// watch.Start ();

		// int n1 = kv["988"];
		// n1 = list.Find ((n) => n == 988);
		// list.Add (1001);
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
		// array = list.ToArray ();
		// int n2 = 0;
		// for (int i = 0; i < array.Length; i++) {
		// 	if (array[i] == 988) {
		// 		n2 = array[i];
		// 		break;
		// 	}
		// }

		// watch.Stop ();
		// UnityEngine.Debug.LogFormat ("array:{0}", watch.Elapsed.Ticks);

		// prefab_white = Resources.Load<GameObject> ("test_PoolManager/White_Sphere");
		// go = GameObject.Instantiate (prefab_white);
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