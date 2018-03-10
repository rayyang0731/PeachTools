using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 设置窗口
/// </summary>
public class SettingWindow : EditorWindow {
	private static string configPath { get { return string.Format ("{0}/{1}", Application.dataPath, "_FixedFolder/PeachTools/Resources/config.json"); } }

	[MenuItem ("Tools/设置")]
	private static void ShowWindow () {
		SettingWindow window = GetWindow<SettingWindow> ("配置文件", true);
		window.Open (configPath);
		window.position = new Rect (0, 0, 600, 500);
		window.minSize = new Vector2 (600, 500);
		window.Show ();
	}

	/// <summary>
	/// 数据
	/// </summary>
	/// <returns></returns>
	private Dictionary<string, string> data = new Dictionary<string, string> ();

	private List<string> data_Keys = new List<string> ();

	private void OnGUI () {
		ShowToggle ();
		ShowArticle ();
		NewArticle ();
	}

	/// <summary>
	/// 显示条目
	/// </summary>
	private void ShowArticle () {
		foreach (var key in data_Keys) {
			GUILayout.BeginHorizontal ("box");
			GUILayout.Label (key, GUILayout.Width (180));
			GUILayout.Label (":", GUILayout.Width (10));
			data[key] = GUILayout.TextField (data[key]);
			if (GUILayout.Button ("删除", GUILayout.Width (50))) {
				data.Remove (key);
				data_Keys = new List<string> (data.Keys);
				Save ();
				break;
			}
			GUILayout.EndHorizontal ();
		}
	}

	private string newKey;
	private string newValue;
	/// <summary>
	/// 单条条目
	/// </summary>
	private void NewArticle () {
		GUILayout.BeginHorizontal ("box");
		newKey = GUILayout.TextField (newKey, GUILayout.Width (180));
		GUILayout.Label (":", GUILayout.Width (10));
		newValue = GUILayout.TextField (newValue);
		if (GUILayout.Button ("添加", GUILayout.Width (50)) &&
			!string.IsNullOrEmpty (newKey) &&
			!string.IsNullOrEmpty (newValue)) {
			data.Add (newKey, newValue);
			newKey = string.Empty;
			newValue = string.Empty;

			data_Keys = new List<string> (data.Keys);

			Save ();
		}
		GUILayout.EndHorizontal ();
	}

	private bool UseStreamingAssets = false;
	private bool IsSimulationMode = false;

	private void ShowToggle () {
		GUILayout.BeginHorizontal ("box");
		if (EditorGUILayout.ToggleLeft ("是否使用 StreamingAssets", UseStreamingAssets) != UseStreamingAssets) {
			UseStreamingAssets = !UseStreamingAssets;
			data["UseStreamingAssets"] = UseStreamingAssets.ToString ();
			data_Keys = new List<string> (data.Keys);
			Save ();
		}
		if (EditorGUILayout.ToggleLeft ("是否使用虚拟加载模式", IsSimulationMode) != IsSimulationMode) {
			IsSimulationMode = !IsSimulationMode;
			data["IsSimulationMode"] = IsSimulationMode.ToString ();
			data_Keys = new List<string> (data.Keys);
			Save ();
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
			FileUtilities.WriteText (path, "{\"AssetFolder\": \"Original Resources\",\"AssetBundleExtName\": \".assetbundle\",\"BundleFolder\": \"Bundles\",\"ManifestName\": \"AssetBundleManifest\",\"UseStreamingAssets\": \"true\",\"IsSimulationMode\": \"true\"}");
		}

		string json = FileUtilities.ReadText (path);
		data = JsonConvert.DeserializeObject<Dictionary<string, string>> (json);
		data_Keys = new List<string> (data.Keys);

		UseStreamingAssets = System.Convert.ToBoolean (data["UseStreamingAssets"]);
		IsSimulationMode = System.Convert.ToBoolean (data["IsSimulationMode"]);
	}
}