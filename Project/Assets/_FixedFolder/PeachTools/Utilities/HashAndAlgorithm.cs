using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

/// <summary>
/// 计算 Hash MD5 SHA1
/// </summary>
public static class HashAndAlgorithm {
	/// <summary>
	/// 计算哈希值
	/// </summary>
	/// <param name="hashAlgorithm">哈希类</param>
	/// <param name="buffer">数据</param>
	/// <param name="convert">需大写填参数"X2",需小写填参数"x2"</param>
	/// <param name="simple">是否简化值,简化值只有16位,完整值32位</param>
	/// <returns>HASH值</returns>
	public static string ComputeHASH (HashAlgorithm hashAlgorithm, byte[] buffer, string convert, bool simple = false) {
		StringBuilder sb = new StringBuilder ();
		using (hashAlgorithm) {
			byte[] result = hashAlgorithm.ComputeHash (buffer);
			int i, length;
			if (simple) //简化版16位是取中间8位,抛弃前4位与后4位
			{
				i = 4;
				length = 12;
			} else {
				i = 0;
				length = result.Length;
			}
			for (; i < length; i++) {
				sb.Append (result[i].ToString (convert));
			}
		}
		return sb.ToString ();
	}

	#region MD5
	/// <summary>
	/// 计算MD5值16位,convert参数不写默认大写,需小写填参数"x2"
	/// </summary>
	/// <returns>MD5值(大写)</returns>
	public static string ComputeMD5Bit16 (string str, string convert = "X2") {
		return ComputeHASH (MD5.Create (), Encoding.UTF8.GetBytes (str), convert, true);
	}

	/// <summary>
	/// 计算MD5值32位,convert参数不写默认大写,需小写填参数"x2"
	/// </summary>
	/// <returns>MD5值(大写)</returns>
	public static string ComputeMD5 (string str, string convert = "X2") {
		return ComputeHASH (MD5.Create (), Encoding.UTF8.GetBytes (str), convert);
	}

	/// <summary>
	/// 计算MD5值32位,convert参数不写默认大写,需小写填参数"x2"
	/// </summary>
	/// <returns>MD5值(大写)</returns>
	public static string ComputeMD5 (System.IO.Stream stream, string convert = "X2") {
		return ComputeHASH (MD5.Create (), FileUtilities.StreamToByte (stream), convert);
	}

	/// <summary>
	/// 计算MD5值32位,convert参数不写默认大写,需小写填参数"x2"
	/// </summary>
	/// <returns>MD5值(大写)</returns>
	public static string ComputeMD5 (byte[] buffer, string convert = "X2") {
		return ComputeHASH (MD5.Create (), buffer, convert);
	}

	/// <summary>
	/// 字符串数组转换MD5,convert参数不写默认为大写,需小写填参数"x2"
	/// </summary>
	/// <param name="strs">转换参数</param>
	/// <param name="convert">转换参数</param>
	/// <returns>MD5键值对集合，键为字符串，值为MD5</returns>
	public static Dictionary<string, string> ComputeStringArrayMd5 (string[] strs, string convert = "X2") {
		Dictionary<string, string> dirs = new Dictionary<string, string> ();
		using (MD5 md5 = MD5.Create ()) {
			for (int i = 0, y = strs.Length; i < y; i++) {
				StringBuilder sb = new StringBuilder ();
				byte[] hashBytes = md5.ComputeHash (System.Text.Encoding.UTF8.GetBytes (strs[i]));
				for (int x = 0; x < hashBytes.Length; x++)
					sb.Append (hashBytes[x].ToString (convert));
				dirs.Add (strs[i], sb.ToString ());
			}
		}
		return dirs;
	}

	/// <summary>
	/// 验证值与Md5值是否匹配
	/// </summary>
	/// <returns>匹配返回true,否则返回false</returns>
	public static bool VerifyMD5 (string str, string md5Str) {
		return string.Compare (ComputeMD5 (str), md5Str, true) == 0;
	}

	/// <summary>
	/// 验证值与Md5值是否匹配
	/// </summary>
	/// <returns>匹配返回true,否则返回false</returns>
	public static bool VerifyMD5 (System.IO.Stream stream, string md5Str) {
		return string.Compare (ComputeMD5 (stream), md5Str, true) == 0;
	}

	/// <summary>
	/// 验证值与Md5值是否匹配
	/// </summary>
	/// <returns>匹配返回true,否则返回false</returns>
	public static bool VerifyMD5 (byte[] buffer, string md5Str) {
		return string.Compare (ComputeMD5 (buffer), md5Str, true) == 0;
	}
	#endregion

	#region SHA1
	/// <summary>
	/// 计算SHA1值,convert参数不写默认大写,需小写填参数"x2"
	/// </summary>
	/// <returns>SHA1值(大写)</returns>
	public static string ComputeSHA1 (string str, string convert = "X2") {
		return ComputeHASH (SHA1.Create (), Encoding.UTF8.GetBytes (str), convert);
	}

	/// <summary>
	/// 计算SHA1值,convert参数不写默认大写,需小写填参数"x2"
	/// </summary>
	/// <returns>SHA1值(大写)</returns>
	public static string ComputeSHA1 (System.IO.Stream stream, string convert = "X2") {
		return ComputeHASH (SHA1.Create (), FileUtilities.StreamToByte (stream), convert);
	}

	/// <summary>
	/// 计算SHA1值,convert参数不写默认大写,需小写填参数"x2"
	/// </summary>
	/// <returns>SHA1值(大写)</returns>
	public static string ComputeSHA1 (byte[] buffer, string convert = "X2") {
		return ComputeHASH (SHA1.Create (), buffer, convert);
	}

	/// <summary>
	/// 验证值与SHA1值是否匹配
	/// </summary>
	/// <returns>匹配返回true,否则返回false</returns>
	public static bool VerifySHA1 (string str, string sha1Str) {
		return string.Compare (ComputeSHA1 (str), sha1Str, true) == 0;
	}

	/// <summary>
	/// 验证值与SHA1值是否匹配
	/// </summary>
	/// <returns>匹配返回true,否则返回false</returns>
	public static bool VerifySHA1 (System.IO.Stream stream, string sha1Str) {
		return string.Compare (ComputeSHA1 (stream), sha1Str, true) == 0;
	}

	/// <summary>
	/// 验证值与SHA1值是否匹配
	/// </summary>
	/// <returns>匹配返回true,否则返回false</returns>
	public static bool VerifySHA1 (byte[] buffer, string sha1Str) {
		return string.Compare (ComputeSHA1 (buffer), sha1Str, true) == 0;
	}
	#endregion
}