using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 解析用自定义规则
/// 例子:
///  if (value.Contains ("%")) {
/// 	string s_Trans = value.Split ('%') [0];
/// 	if (s_Trans.Contains (".")) {
/// 		float v = float.Parse (s_Trans);
/// 		return (v * 0.01f).ToString ();
/// 	} else {
/// 		int v = int.Parse (s_Trans);
/// 		return (v * 0.01f).ToString ();
/// 	}
/// } else
/// 	return value;
/// </summary>
/// <param name="val"></param>
/// <returns></returns>
public delegate string ParseCustomRule (string val);
/// <summary>
/// 解析工具
/// </summary>
public static class ParseUtilities {

	const string DebugFormat = "<color=#9400D3>[ParseUtilities]</color> {0} : {1}";
	/// <summary>
	/// 解析Vector3
	/// </summary>
	/// <param name="val">要解析的字符串</param>
	/// <param name="separator">分隔符</param>
	/// <returns></returns>
	public static Vector3 ToVector3 (string val, char separator = ',') {
		if (string.IsNullOrEmpty (val) || val == "0")
			return Vector3.zero;

		val = RemoveParenthesis (val);

		string[] xyz = val.Split (separator);

		if (xyz.Length != 3) return Vector3.zero;

		float x, y, z;

		if (!float.TryParse (xyz[0], out x)) { Debug.LogErrorFormat (DebugFormat, "Vector3_X 无法解析", xyz[0]); }
		if (!float.TryParse (xyz[1], out y)) { Debug.LogErrorFormat (DebugFormat, "Vector3_Y 无法解析", xyz[1]); }
		if (!float.TryParse (xyz[2], out z)) { Debug.LogErrorFormat (DebugFormat, "Vector3_Z 无法解析", xyz[2]); }

		return new Vector3 (x, y, z);

	}

	/// <summary>
	/// 解析Vector2
	/// </summary>
	/// <param name="val">要解析的字符串</param>
	/// <param name="separator">分隔符</param>
	/// <returns></returns>
	public static Vector2 ToVector2 (string val, char separator = ',') {
		if (string.IsNullOrEmpty (val) || val.Equals ("0"))
			return Vector2.zero;

		val = RemoveParenthesis (val);

		string[] xy = val.Split (separator);

		if (xy.Length != 2) return Vector2.zero;

		float x, y;

		if (!float.TryParse (xy[0], out x)) { Debug.LogErrorFormat (DebugFormat, "Vector2_X 无法解析", xy[0]); }
		if (!float.TryParse (xy[1], out y)) { Debug.LogErrorFormat (DebugFormat, "Vector2_Y 无法解析", xy[1]); }

		return new Vector2 (x, y);
	}

	/// <summary>
	/// 解析Quaternion
	/// </summary>
	/// <param name="val">要解析的字符串</param>
	/// <param name="separator">分隔符</param>
	/// <returns></returns>
	public static Quaternion ToQuaternion (string val, char separator = ',') {
		if (string.IsNullOrEmpty (val) || val == "0")
			return Quaternion.identity;

		val = RemoveParenthesis (val);

		string[] xyzw = val.Split (separator);

		if (xyzw.Length != 4) return Quaternion.identity;

		float x, y, z, w;

		if (!float.TryParse (xyzw[0], out x)) { Debug.LogErrorFormat (DebugFormat, "Quaternion_X 无法解析", xyzw[0]); }
		if (!float.TryParse (xyzw[1], out y)) { Debug.LogErrorFormat (DebugFormat, "Quaternion_Y 无法解析", xyzw[1]); }
		if (!float.TryParse (xyzw[2], out z)) { Debug.LogErrorFormat (DebugFormat, "Quaternion_Z 无法解析", xyzw[2]); }
		if (!float.TryParse (xyzw[3], out w)) { Debug.LogErrorFormat (DebugFormat, "Quaternion_W 无法解析", xyzw[3]); }

		return new Quaternion (x, y, z, w);
	}

	/// <summary>
	/// 解析Color
	/// </summary>
	/// <param name="val">要解析的字符串</param>
	/// <param name="separator">分隔符</param>
	/// <returns></returns>
	public static Color ToColor (string val, char separator = ',') {
		if (string.IsNullOrEmpty (val) || val == "0")
			return Color.white;

		val = RemoveParenthesis (val);
		string[] rgba = val.Split (separator);

		if (rgba.Length != 4) return Color.white;

		float r, g, b, a;

		if (!float.TryParse (rgba[0], out r)) { Debug.LogErrorFormat (DebugFormat, "Color_R 无法解析", rgba[0]); }
		if (!float.TryParse (rgba[1], out g)) { Debug.LogErrorFormat (DebugFormat, "Color_G 无法解析", rgba[1]); }
		if (!float.TryParse (rgba[2], out b)) { Debug.LogErrorFormat (DebugFormat, "Color_B 无法解析", rgba[2]); }
		if (!float.TryParse (rgba[3], out a)) { Debug.LogErrorFormat (DebugFormat, "Color_A 无法解析", rgba[3]); }

		return new Color (r, g, b, a);
	}

	/// <summary>
	/// 移除括号及括号外的字符
	/// </summary>
	/// <param name="val">要处理的字符串</param>
	/// <returns></returns>
	private static string RemoveParenthesis (string val) {
		val = val.Remove (0, val.IndexOf ("(") + 1);
		val = val.Remove (val.IndexOf (")")).Trim ();
		Debug.LogFormat (DebugFormat, "原始数据", val);
		return val;
	}

	/// <summary>
	/// 尝试将字符串转换成数字的字符串
	/// </summary>
	/// <param name="value">要解析的字符串</param>
	/// <param name="customRule">自定义规则</param>
	/// <returns></returns>
	public static string TryToNumberString (string value, ParseCustomRule customRule = null) {
		if (string.IsNullOrEmpty (value))
			return "0";
		else {
			if (customRule != null)
				return customRule (value);
		}
		return "0";
	}

	/// <summary>
	/// 拆分 string 返回指定类型数组
	/// </summary>
	/// <param name="str">要拆分的字符串</param>
	/// <param name="separator">分隔符</param>
	/// <returns></returns>
	public static T[] SplitString<T> (string str, char separator = ',') {
		T[] arr_t = null;
		if (str.Contains (separator.ToString ())) {
			string[] _s = str.Split (separator);
			arr_t = new T[_s.Length];
			for (int i = 0; i < _s.Length; i++) {
				arr_t[i] = (T) System.Convert.ChangeType (_s[i], typeof (T));
			}
		} else {
			arr_t = new T[1];
			arr_t[0] = (T) System.Convert.ChangeType (str, typeof (T));
		}
		return arr_t;
	}
}