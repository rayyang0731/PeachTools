using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Test : MonoBehaviour {

	public bool logEnabled = false;
	// Use this for initialization
	void Start () {
		List<int> list = new List<int> ();
		int[] array = new int[1000];
		for (int i = 0; i < 1000; i++) {
			list.Add (i);
			array[i] = i;
		}
		// Debug.unityLogger.logEnabled = logEnabled;
		Stopwatch watch = new Stopwatch ();
		watch.Start ();
		list.ForEach ((x) => x++);
		watch.Stop ();
		UnityEngine.Debug.LogFormat ("list ForEach:{0}", watch.Elapsed.Ticks);

		watch.Reset ();

		watch.Start ();
		for (int i = 0; i < 1000; i++) {
			list[i]++;
		}
		watch.Stop ();
		UnityEngine.Debug.LogFormat ("list for:{0}", watch.Elapsed.Ticks);

		watch.Reset ();

		watch.Start ();
		for (int i = 0; i < 1000; i++) {
			array[i]++;
		}
		watch.Stop ();
		UnityEngine.Debug.LogFormat ("array:{0}", watch.Elapsed.Ticks);
	}

	// Update is called once per frame
	void Update () {
		// for (int i = 0; i < 100; i++) {
		// 	// Debug.Log ("Show");
		// }
	}
}