using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// 配置管理器
/// </summary>
public static class Config {
	private static Dictionary<string, string> config = null;
	private static Dictionary<string, string> colorConfig = null;

	/// <summary>
	/// 获取配置
	/// </summary>
	/// <param name="key">配置数据的 Key</param>
	/// <returns></returns>
	public static T Get<T> (string key) {
		if (config == null) {
			string json = Resources.Load<TextAsset> ("config").text;
			config = JsonConvert.DeserializeObject<Dictionary<string, string>> (json);
		}
		if (config.ContainsKey (key))
			try {
				return (T) System.Convert.ChangeType (config[key], typeof (T));
			} catch (System.Exception e) {
				Debug.LogError (e);
				return default (T);
			}

		else
			throw new System.Exception (string.Format ("配置中不存在这个 Key:{0}", key));
	}

	/// <summary>
	/// 获取颜色配置
	/// </summary>
	/// <param name="key">颜色配置数据的 Key</param>
	/// <returns></returns>
	public static Color GetColor (string key) {
		if (colorConfig == null) {
			string json = Resources.Load<TextAsset> ("color").text;
			colorConfig = JsonConvert.DeserializeObject<Dictionary<string, string>> (json);
		}
		if (colorConfig.ContainsKey (key))
			return ParseUtilities.ToColor (colorConfig[key]);
		else
			throw new System.Exception (string.Format ("颜色配置中不存在这个 Key:{0}", key));
	}

}