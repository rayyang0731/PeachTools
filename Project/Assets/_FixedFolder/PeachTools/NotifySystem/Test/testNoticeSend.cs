using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testNoticeSend : MonoBehaviour {
	private void OnGUI () {
		if (GUILayout.Button ("Send Msg:(NotifySystem.Send (10001))")) {
			NotifySystem.Send (10001);
		}
		if (GUILayout.Button ("Send Msg:(NotifySystem.Send (10002, \"Notify System Test...\"))")) {
			NotifySystem.Send (10002, "Notify System Test...");
		}
		if (GUILayout.Button ("Send Msg:(NotifySystem.Send (noticeObj))")) {
			INoticeObj noticeObj = new NoticeObj (10003, "Notice Object Test...");
			NotifySystem.Send (noticeObj);
		}
	}
}