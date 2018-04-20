using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (UnityEditor.DefaultAsset))]
public class LuaInspector : Editor {
	public override void OnInspectorGUI () {
		string path = AssetDatabase.GetAssetPath (target);
		if (path.EndsWith (".lua")) {
			GUI.enabled = true;
			GUI.backgroundColor = new Color (63, 63, 63);

			string lua = File.ReadAllText (path);

			GUILayout.TextArea (lua);
		}
	}
}