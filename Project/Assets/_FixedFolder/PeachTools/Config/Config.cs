using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// 配置管理器
/// </summary>
public static class Config {
	private static Dictionary<string, string> config = null;

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

}