using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 解析工具
/// </summary>
public static class ParseUtilities {
	/// <summary>
	/// 解析Vector3
	/// </summary>
	/// <param name="val">要解析的字符串</param>
	/// <param name="separator">分隔符</param>
	/// <returns></returns>
	public static Vector3 ParseVector3 (string val, char separator = ',') {
		if (string.IsNullOrEmpty (val) || val == "0")
			return Vector3.zero;

		val = val.Replace ("(", string.Empty)
			.Replace (")", string.Empty);

		string[] xyz = val.Split (separator);

		if (xyz.Length != 3) return Vector3.zero;

		float x = (float) System.Convert.ChangeType (xyz[0], typeof (float));
		float y = (float) System.Convert.ChangeType (xyz[1], typeof (float));
		float z = (float) System.Convert.ChangeType (xyz[2], typeof (float));

		return new Vector3 (x, y, z);
	}

	/// <summary>
	/// 解析Vector2
	/// </summary>
	/// <param name="val">要解析的字符串</param>
	/// <param name="separator">分隔符</param>
	/// <returns></returns>
	public static Vector2 ParseVector2 (string val, char separator = ',') {
		if (string.IsNullOrEmpty (val) || val.Equals ("0"))
			return Vector2.zero;

		val = val.Replace ("(", string.Empty)
			.Replace (")", string.Empty);

		string[] xy = val.Split (separator);

		if (xy.Length != 2) return Vector2.zero;

		float x = (float) System.Convert.ChangeType (xy[0], typeof (float));
		float y = (float) System.Convert.ChangeType (xy[1], typeof (float));

		return new Vector2 (x, y);
	}

	/// <summary>
	/// 解析Color
	/// </summary>
	/// <param name="val">要解析的字符串</param>
	/// <param name="separator">分隔符</param>
	/// <returns></returns>
	public static Color ParseColor (string val, char separator = ',') {
		if (string.IsNullOrEmpty (val) || val == "0")
			return Color.white;

		val = val.Replace ("(", string.Empty).Replace (")", string.Empty).Replace ("RGBA", "");
		string[] rgba = val.Split (separator);
		float r = (float) System.Convert.ChangeType (rgba[0], typeof (float));
		float g = (float) System.Convert.ChangeType (rgba[1], typeof (float));
		float b = (float) System.Convert.ChangeType (rgba[2], typeof (float));
		float a = (float) System.Convert.ChangeType (rgba[3], typeof (float));

		return new Color (r, g, b, a);
	}
}