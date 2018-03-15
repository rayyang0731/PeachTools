using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// 语言文字管理
/// </summary>
public static class Language {
    private const string DebugFormat = "<color=#9400D3>[Language]</color> {0} : {1}";
    private static string prevMD5;

    private static IDictionary<string, string> m_map = new Dictionary<string, string> ();

    /// <summary>
    /// 加载本地化文件
    /// </summary>
    /// <param name="language">语言种类</param>
    public static void Load (string language) {
        TextAsset text = AssetLoader.Load<TextAsset> (GetLocalizeFilePath (language), AssetType.TXT);
        if (text != null)
            LoadContent (text.text);
        else {
#if UNITY_EDITOR
                Debug.LogErrorFormat (DebugFormat, "该语言的本地化文件不存在,请先创建", GetLocalizeFilePath (language));
#endif
            Debug.LogErrorFormat (DebugFormat, "Load AssetBundle Error", "读取本地化文件失败！请检查。");

        }
    }

    /// <summary>
    /// 加载本地化内容
    /// </summary>
    /// <param name="content">内容</param>
    public static void LoadContent (string content) {
        if (!string.IsNullOrEmpty (content)) {
            m_map = ParseLocalizeFile (content);
            Debug.LogFormat (DebugFormat, "Load Content", "加载本地化文件..");
        }
    }

    /// <summary>
    /// 惰性加载
    /// </summary>
    public static void LazyLoad (string language) {
        string path = GetLocalizeFilePath (language);
#if UNITY_EDITOR
        TextAsset txt = AssetLoader.LoadInEditor<TextAsset> (path, AssetType.TXT);
#else
        TextAsset txt = AssetLoader.Load<TextAsset> (path, AssetType.TXT);
#endif
        if (txt != null) {
            string md5 = HashAndAlgorithm.ComputeMD5 (txt.text);
            if (m_map == null || string.IsNullOrEmpty (prevMD5) || !prevMD5.Equals (md5)) {
                prevMD5 = md5;
                LoadContent (txt.text);
            }
        } else {
            Debug.LogErrorFormat (DebugFormat, "读取本地化文件失败", path);
        }
    }

    /// <summary>
    /// 卸载本地化内容
    /// </summary>
    public static void Unload () {
        if (m_map != null) {
            m_map.Clear ();
            m_map = null;
        }
    }

    /// <summary>
    /// 获取本地化文件路径
    /// </summary>
    private static string GetLocalizeFilePath (string language) {
        return string.Format ("Languages/{0}", language);
    }

    /// <summary>
    /// 解析文本
    /// </summary>
    private static Dictionary<string, string> ParseLocalizeFile (string content) {
        Dictionary<string, string> map = new Dictionary<string, string> ();

        //丢弃注释
        content = Regex.Replace (content, @"/\*[\s\S]*\*/", string.Empty);

        //拆分
        string[] lines = content.Split (new string[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0, y = lines.Length; i < y; i++) {
            string item = lines[i];
            //丢弃注释
            if (item.StartsWith ("//")) continue;

            //检查格式
            int splitPos = item.IndexOf ('=');
            if (splitPos < 0) {
                Debug.LogErrorFormat (DebugFormat, "不正常的文本格式", item);
                continue;
            }

            //截取键值对
            string key = item.Substring (0, splitPos).Trim ();
            string val = item.Substring (splitPos + 1).Trim ().Replace ("\\r\\", "\r\n");

            if (!map.ContainsKey (key))
                map[key] = val;
            else
                Debug.LogErrorFormat (DebugFormat, "重复的key", string.Format ("{0} - 第{1}行.", key, i.ToString ()));
        }
        return map;
    }

    /// <summary>
    /// 判断是否存在这个key
    /// </summary>
    public static bool HasKey (string key) {
        return m_map != null && m_map.ContainsKey (key);
    }
    /// <summary>
    /// 获取值
    /// </summary>
    public static string GetValue (string key) {
        string value;
        if (m_map != null && m_map.TryGetValue (key, out value)) {
            return value;
        }
        return string.Empty;
    }

    /// <summary>
    /// 获取值
    /// </summary>
    private static string[] GetValues (string[] keys) {
        string[] args = new string[keys.Length];
        //获取格式参数
        for (int i = 0, count = keys.Length; i < count; ++i) {
            args[i] = GetValue (keys[i]);
        }
        return args;
    }

    /// <summary>
    /// 格式化字符串，参数会自动使用key对应的value
    /// 例:
    /// enemyName = 哥布林
    /// Format("击杀50个{0}",enemyName)
    /// </summary>
    public static string Format (string format, string key) {
        string value;
        if (m_map.TryGetValue (key, out value)) {
            return string.Format (format, value);
        }
        return format;
    }

    /// <summary>
    /// 格式化字符串，参数会自动使用keys对应的value
    /// 例:
    /// enemyName1 = 哥布林
    /// enemyName2 = 史莱姆
    /// Format("击杀50个{0},20个{1}",enemyName1,enemyName2)
    /// </summary>
    public static string Format (string format, params string[] keys) {
        return string.Format (format, GetValues (keys));
    }

    /// <summary>
    /// 使用key来格式化第一个key的值
    /// 例:
    /// log = {0}打败{1}
    /// enemyName1 = 哥布林
    /// enemyName2 = 史莱姆
    /// UseKeyFormat(log,enemyName1,enemyName2)
    /// </summary>
    public static string UseKeyFormat (string key, params string[] keys) {
        string value;
        if (m_map.TryGetValue (key, out value)) {
            //格式化
            return string.Format (value, GetValues (keys));
        }
        return string.Empty;
    }

    /// <summary>
    /// 获取格式化本地字符串
    /// </summary>
    /// <param name="key">本地键</param>
    /// <param name="arg">格式化参数</param>
    public static string FormatValue (string key, params object[] arg) {
        string value;
        if (m_map.TryGetValue (key, out value)) {
            return string.Format (value, arg);
        }
        return string.Empty;
    }

    /// <summary>
    /// 获取格式化本地字符串
    /// </summary>
    /// <param name="key">本地键</param>
    /// <param name="arg">格式化参数</param>
    public static string FormatValue (string key, object arg) {
        string value;
        if (m_map.TryGetValue (key, out value)) {
            return string.Format (value, arg);
        }
        return string.Empty;
    }

    /// <summary>
    /// 获取格式化本地字符串
    /// </summary>
    /// <param name="key">本地键</param>
    /// <param name="arg">格式化参数</param>
    public static string FormatValue (string key, object arg1, object arg2) {
        string value;
        if (m_map.TryGetValue (key, out value)) {
            return string.Format (value, arg1, arg2);
        }
        return string.Empty;
    }
}