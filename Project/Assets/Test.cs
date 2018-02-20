using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	public bool logEnabled = false;
	// Use this for initialization
	void Start () {
		Debug.unityLogger.logEnabled = logEnabled;
	}

	// Update is called once per frame
	void Update () {
		for (int i = 0; i < 100; i++) {
			Debug.Log ("Show");
		}
	}
}