using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testTimer : MonoBehaviour {

	string timerGUID = string.Empty;
	private void Start () {
		// Timer.Startup (2, (t) => { Debug.Log ("Timer test"); });
		// Timer.Startup (2, (t) => { Debug.LogFormat ("StartCount:{0}|FinishCount:{1}", t.StartCount, t.FinishCount); }, 0, true);
		// Timer.Startup (2, (t) => { Debug.LogFormat ("StartCount:{0}|FinishCount:{1}", t.StartCount, t.FinishCount); }, 1, true);

	}
	private void OnGUI () {
		if (string.IsNullOrEmpty (timerGUID)) {
			if (GUILayout.Button ("新建计时器")) {
				Timer timer = Timer.Create (5, (t) => Debug.Log ("Timer Test"), 0, true);
				timer.AddStartCallback ((t) => Debug.Log ("Timer Start"));
				timer.AddStopCallback ((t) => Debug.Log ("Timer End"));
				timer.SetParams ("Param 0", 1);
				timer.AddUpdateCallback ((t) => Debug.LogFormat ("Update:已用时间({0})|参数0({1})|参数1({2})", t.ElapsedTime, t.GetParam<string> (), t.GetParam<int> (1)));
				timer.AddPauseCallback ((t) => Debug.Log ("Timer Pause"));
				timer.AddResumeCallback ((t) => Debug.Log ("Timer Resume"));
				timer.AddCancelCallback ((t) => Debug.Log ("Timer Cancel"));
				timerGUID = timer.GUID;
				timer.Startup ();
			}
		} else {
			if (GUILayout.Button ("Pause")) {
				Timer timer = TimerManager.Instance.GetTimer (timerGUID);
				timer.Pause ();
			}
			if (GUILayout.Button ("Resume")) {
				Timer timer = TimerManager.Instance.GetTimer (timerGUID);
				timer.Renumes ();
			}
			if (GUILayout.Button ("Stop")) {
				Timer timer = TimerManager.Instance.GetTimer (timerGUID);
				timer.Stop ();
				timerGUID = string.Empty;
			}
			if (GUILayout.Button ("Cancel")) {
				Timer timer = TimerManager.Instance.GetTimer (timerGUID);
				timer.Cancel ();
				timerGUID = string.Empty;
			}
		}
	}
}