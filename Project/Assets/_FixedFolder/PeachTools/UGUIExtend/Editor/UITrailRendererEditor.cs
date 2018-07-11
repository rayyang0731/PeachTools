using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor (typeof (UITrailRenderer))]
public class UITrailRendererEditor : Editor {
	UITrailRenderer _target;
	void OnEnable () {
		_target = (UITrailRenderer) target;
	}
	void OnSceneGUI () {
		if (_target.rt == null)
			_target.anchor = Handles.PositionHandle (_target.anchor, Quaternion.identity);
	}
}