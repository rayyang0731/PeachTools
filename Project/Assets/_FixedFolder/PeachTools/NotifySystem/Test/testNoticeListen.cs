using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testNoticeListen : MonoBehaviour {

	private void Start () {
		NotifySystem.Listen (10001, ListenOne);
		NotifySystem.Listen (10002, ListenTwo);
		NotifySystem.Listen (10003, ListenThree);

		NotifySystem.Listen (10001, ListenOne2);
		NotifySystem.Listen (10001, ListenOne3);
	}

	private void ListenOne3 (INoticeObj obj) {
		Debug.LogFormat ("Listen - 10001 - 3");
	}

	private void ListenOne2 (INoticeObj obj) {
		Debug.LogFormat ("Listen - 10001 - 2");
	}

	private void ListenThree (INoticeObj obj) {
		Debug.LogFormat ("Listen - 10003:{0}", (string) obj.Body);
	}

	private void ListenTwo (INoticeObj obj) {
		Debug.LogFormat ("Listen - 10002:{0}", (string) obj.Body);
	}

	private void ListenOne (INoticeObj obj) {
		Debug.Log ("Listen - 10001");
	}

	private void OnGUI () {
		GUILayout.Space (150);

		if (GUILayout.Button ("Remove 10001")) {
			NotifySystem.RemoveListen (10001, ListenOne2);
		}

		if (GUILayout.Button ("Remove All 10001")) {
			NotifySystem.RemoveListen (10001);
		}
	}
}