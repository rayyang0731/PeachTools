using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testTimer : MonoBehaviour {

	long timerGUID = 0;
	private void Start () {
		// Timer.Startup (2, (t) => { Debug.Log ("Timer test"); });
		// Timer.Startup (2, (t) => { Debug.LogFormat ("StartCount:{0}|FinishCount:{1}", t.StartCount, t.FinishCount); }, 0, true);
		// Timer.Startup (2, (t) => { Debug.LogFormat ("StartCount:{0}|FinishCount:{1}", t.StartCount, t.FinishCount); }, 1, true);

	}
	private void OnGUI () {
		if (timerGUID == 0) {
			if (GUILayout.Button ("新建计时器")) {
				Timer timer = Timer.Create (5, (t) => Debug.Log ("Timer Test"), 0, true);
				timer.AddStartCallback ((t) => Debug.Log ("Timer Start"));
				timer.AddEndCallback ((t) => Debug.Log ("Timer End"));
				timer.SetParams ("Param 0", 1);
				timer.AddUpdateCallback ((t) => Debug.LogFormat ("Update:已用时间({0})|参数0({1})|参数1({2})", t.ElapsedTime, t.GetParam<string> (), t.GetParam<int> (1)));
				timer.AddPauseCallback ((t) => Debug.Log ("Timer Pause"));
				timer.AddResumeCallback ((t) => Debug.Log ("Timer Resume"));
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
		}
	}
}