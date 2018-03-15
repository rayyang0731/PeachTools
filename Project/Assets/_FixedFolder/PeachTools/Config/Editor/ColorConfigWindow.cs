using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 配置窗口
/// </summary>
public class ColorConfigWindow : EditorWindow {
	private static string configPath { get { return string.Format ("{0}/{1}", Application.dataPath, "_FixedFolder/PeachTools/Resources/color.json"); } }

	[MenuItem ("Tools/配置/颜色配置文件")]
	private static void ShowWindow () {
		ColorConfigWindow window = GetWindow<ColorConfigWindow> ("配置文件", true);
		window.Open (configPath);
		window.position = new Rect (0, 0, 600, 500);
		window.minSize = new Vector2 (600, 500);
		window.Show ();
	}

	/// <summary>
	/// 数据
	/// </summary>
	/// <returns></returns>
	private Dictionary<string, string> data;

	private List<string> data_Keys = new List<string> ();
	private List<Color> data_Values = new List<Color> ();

	private Vector2 scroll = Vector2.zero;
	private void OnGUI () {
		scroll = GUILayout.BeginScrollView (scroll);
		ShowArticle ();
		NewArticle ();
		GUILayout.EndScrollView ();
	}

	/// <summary>
	/// 显示条目
	/// </summary>
	private void ShowArticle () {
		for (int i = 0; i < data_Keys.Count; i++) {
			GUILayout.BeginHorizontal ("box");
			GUILayout.Label (data_Keys[i], GUILayout.Width (180));
			GUILayout.Label (":", GUILayout.Width (10));
			data_Values[i] = EditorGUILayout.ColorField (data_Values[i]);
			if (GUILayout.Button ("删除", GUILayout.Width (50))) {
				data.Remove (data_Keys[i]);
				data_Keys.RemoveAt (i);
				data_Values.RemoveAt (i);
				Save ();
				break;
			}
			GUILayout.EndHorizontal ();
		}
	}

	private string newKey;
	private Color newValue = Color.white;
	/// <summary>
	/// 单条条目
	/// </summary>
	private void NewArticle () {
		GUILayout.BeginHorizontal ("box");
		newKey = GUILayout.TextField (newKey, GUILayout.Width (180));
		GUILayout.Label (":", GUILayout.Width (10));
		newValue = EditorGUILayout.ColorField (newValue);
		if (GUILayout.Button ("添加", GUILayout.Width (50)) &&
			!string.IsNullOrEmpty (newKey)) {
			if (data.ContainsKey (newKey)) {
				ShowNotification (new GUIContent (string.Format ("此 Key 已存在:{0}", newKey)));
			} else {
				data.Add (newKey, newValue.ToString ());
				data_Keys.Add (newKey);
				data_Values.Add (newValue);
				newKey = string.Empty;
				newValue = Color.white;

				Save ();
			}
		}
		GUILayout.EndHorizontal ();
	}

	private void Save () {
		if (FileUtilities.WriteText (configPath, JsonConvert.SerializeObject (data))) {
			AssetDatabase.Refresh ();
			ShowNotification (new GUIContent ("保存配置成功"));
		} else {
			ShowNotification (new GUIContent ("保存配置成功"));
		}
	}

	private void Open (string path) {
		if (!FileUtilities.CheckFile (path)) {
			FileUtilities.WriteText (path, string.Empty);
			AssetDatabase.Refresh ();
		}

		string json = FileUtilities.ReadText (path);
		data = JsonConvert.DeserializeObject<Dictionary<string, string>> (json);
		if (data != null) {
			data_Keys = new List<string> (data.Keys);
			string[] values = new string[data.Values.Count];
			data.Values.CopyTo (values, 0);
			data_Values = new List<Color> (ParseUtilities.ToColor (values));
		} else {
			data_Keys = new List<string> ();
			data_Values = new List<Color> ();
			data = new Dictionary<string, string> ();
		}
	}
}