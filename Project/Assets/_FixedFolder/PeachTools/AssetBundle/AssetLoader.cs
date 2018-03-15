using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// AssetBundle资源类型
/// </summary>
public enum AssetType {
    /// <summary>
    /// 游戏对象
    /// </summary>
    GameObject,
    /// <summary>
    /// 文本
    /// </summary>
    TXT,
    /// <summary>
    /// 图片
    /// </summary>
    Texture,
    /// <summary>
    /// 声音
    /// </summary>
    Audio,
    /// <summary>
    /// 控制器
    /// </summary>
    AnimatorController,
    /// <summary>
    /// 序列化配置文件
    /// </summary>
    ScriptableObject,
}

/// <summary>
/// 已加载 AssetBundle
/// </summary>
public class LoadedAssetBundle {
    /// <summary>
    /// 资源对象
    /// </summary>
    public AssetBundle ab;
    /// <summary>
    /// 被引用数量
    /// </summary>
    public int count;

    public LoadedAssetBundle (AssetBundle assetBundle) {
        ab = assetBundle;
        count = 1;
    }
}

/// <summary>
/// 资源加载器
/// </summary>
public static class AssetLoader {

    private const string DebugFormat = "<color=#9400D3>[AssetLoader]</color> {0} : {1}";
    /// <summary>
    /// 资源包地址
    /// </summary>
    /// <returns></returns>
    private static string BundlePath {
        get {
            if (Config.Get<bool> ("UseStreamingAssets"))
                return Application.streamingAssetsPath;
            else
                return string.Format ("{0}/{1}",
                    Directory.GetParent (Application.dataPath).FullName,
                    Config.Get<string> ("BundleFolder"));
        }
    }

    /// <summary>
    /// 资源清单
    /// </summary>
    private static AssetBundleManifest _manifest;
    /// <summary>
    /// 资源清单
    /// </summary>
    /// <returns></returns>
    private static AssetBundleManifest Manifest {
        get {
            if (_manifest == null)
                _manifest = GetManifest ();
            return _manifest;
        }
    }

    /// <summary>
    /// 全部 Assetbundle 名称
    /// </summary>
    private static List<string> _allBundles;
    /// <summary>
    /// 全部 Assetbundle 名称
    /// </summary>
    private static List<string> AllBundles {
        get {
            if (_allBundles == null)
                _allBundles = new List<string> (Manifest.GetAllAssetBundles ());
            return _allBundles;
        }
    }

    /// <summary>
    /// 在缓存中的资源
    /// </summary>
    /// <returns></returns>
    private static Dictionary<string, LoadedAssetBundle> LoadedBundles = new Dictionary<string, LoadedAssetBundle> ();

    /// <summary>
    /// 获取资源清单
    /// </summary>
    /// <returns></returns>
    private static AssetBundleManifest GetManifest () {
        string manifestPath = string.Format ("{0}/{1}", BundlePath, Config.Get<string> ("ManifestName"));
        AssetBundle ab = AssetBundle.LoadFromFile (manifestPath);
        AssetBundleManifest _manifest = (AssetBundleManifest) ab.LoadAsset ("AssetBundleManifest");
        ab.Unload (false);
        return _manifest;
    }

    /// <summary>
    /// 资源加载完成回调
    /// </summary>
    /// <param name="obj">被加载出来的对象</param>
    /// <param name="customData">自定义数据</param>
    public delegate void Callback (object obj, params object[] customData);

    /// <summary>
    /// 加载AssetBundle资源
    /// </summary>
    /// <param name="assetbundleName">资源文件的AssetbundleName(无扩展名)</param>
    /// <param name="assetType">资源类型</param>
    /// <returns></returns>
    public static T Load<T> (string assetbundleName, AssetType assetType) where T : Object {
        if (!Config.Get<bool> ("IsSimulationMode")) {

            Object obj;
            if (LoadAssetBundle (assetbundleName, out obj)) {
                UnloadAssetBundle (assetbundleName);
                return obj as T;
            } else
                return null;
        } else {
            return LoadInEditor<T> (assetbundleName, assetType);
        }
    }

    private static bool LoadAssetBundle (string assetbundleName, out Object asset) {
        string assetName = Path.GetFileNameWithoutExtension (assetbundleName);

        LoadedAssetBundle loaded;
        if (LoadedBundles.TryGetValue (assetbundleName, out loaded)) {
            asset = loaded.ab.LoadAsset (assetName);
            return true;
        }

        string fullName = string.Format ("{0}{1}", assetbundleName, Config.Get<string> ("AssetBundleExtName")).ToLower ();
        if (!AllBundles.Contains (fullName)) {
            Debug.LogErrorFormat (DebugFormat, "不存在 Assetbundle", fullName);
            asset = null;
            return false;
        }

        string fullPath = string.Format ("{0}/{1}", BundlePath, fullName);

        LoadDependencies (assetbundleName);

        AssetBundle ab = AssetBundle.LoadFromFile (fullPath);

        if (ab == null) {
            Debug.LogErrorFormat (DebugFormat, "未加载到 AssetBundle", assetbundleName);
            asset = null;
            return false;
        }

        LoadedBundles.Add (assetbundleName, new LoadedAssetBundle (ab));
        asset = ab.LoadAsset (assetName);
        return true;
    }

    /// <summary>
    /// 加载AssetBundle资源进入缓存
    /// </summary>
    /// <param name="assetbundleName">资源文件的AssetbundleName(无扩展名)</param>
    /// <param name="assetType">资源类型</param>
    /// <returns></returns>
    public static T LoadInCache<T> (string assetbundleName, AssetType assetType) where T : Object {
        if (!Config.Get<bool> ("IsSimulationMode")) {
            Object obj;
            if (LoadAssetBundle (assetbundleName, out obj))
                return obj as T;
            else
                return null;
        } else {
            return LoadInEditor<T> (assetbundleName, assetType);
        }
    }

    /// <summary>
    /// 编辑器中加载
    /// </summary>
    /// <param name="assetbundleName">资源文件的AssetbundleName(无扩展名)</param>
    /// <param name="assetType">资源类型</param>
    /// <returns></returns>
    public static T LoadInEditor<T> (string assetbundleName, AssetType assetType) where T : Object {
#if UNITY_EDITOR
        string extName = GetExtName (assetType);
        string fullName = string.Format ("{0}{1}", assetbundleName, extName);
        string fullPath = string.Format ("Assets/{0}/{1}", Config.Get<string> ("AssetFolder"), fullName);

        Object obj = UnityEditor.AssetDatabase.LoadAssetAtPath (fullPath, typeof (T));
        return obj as T;
#else
        Debug.LogError ("资源读取模式错误！");
        return default (T);
#endif
    }

    /// <summary>
    /// 获得后缀名
    /// </summary>
    /// <param name="assetType">资源类型</param>
    /// <returns></returns>
    private static string GetExtName (AssetType assetType) {
        string extName = string.Empty;
        switch (assetType) {
            case AssetType.GameObject:
                extName = ".prefab";
                break;
            case AssetType.TXT:
                extName = ".txt";
                break;
            case AssetType.Texture:
                extName = ".png";
                break;
            case AssetType.Audio:
                extName = ".mp3";
                break;
            case AssetType.AnimatorController:
                extName = ".controller";
                break;
            case AssetType.ScriptableObject:
                extName = ".asset";
                break;
        }

        return extName;
    }

    /// <summary>
    /// 加载AssetBundle资源
    /// </summary>
    /// <param name="assetbundleName">资源文件的AssetbundleName(无扩展名)</param>
    /// <param name="assetType">资源类型</param>
    /// <param name="loader">资源加载完成回调</param>
    /// <param name="customData">回调使用的自定义数据</param>
    /// <returns></returns>
    public static T Load<T> (string assetbundleName, AssetType assetType, Callback loader, params object[] customData) where T : Object {
        T obj = Load<T> (assetbundleName, assetType);
        if (loader != null && obj != null)
            loader (obj, customData);
        return obj;
    }

    /// <summary>
    /// 根据 AssetBundle 实例化对象
    /// </summary>
    /// <param name="assetbundleName">资源文件的AssetbundleName(无扩展名)</param>
    /// <param name="position">实例化位置</param>
    /// <param name="rotation">实例化轴向</param>
    /// <param name="parent">父物体</param>
    /// <returns></returns>
    public static GameObject Create (string assetbundleName, Vector3 position, Quaternion rotation, Transform parent = null) {
        GameObject original = Load<GameObject> (assetbundleName, AssetType.GameObject);
        try {
            GameObject go = GameObject.Instantiate<GameObject> (original, position, rotation, parent);
            return go;
        } catch (System.Exception) {
            Debug.LogErrorFormat (DebugFormat, "创建 AssetBundle 出错", assetbundleName);
            return null;
        }

    }

    /// <summary>
    /// 根据 AssetBundle 实例化对象并且不修改位置轴向等信息
    /// </summary>
    /// <param name="assetbundleName">资源文件的AssetbundleName(无扩展名)</param>
    /// <returns></returns>
    public static GameObject Create (string assetbundleName) {
        GameObject original = Load<GameObject> (assetbundleName, AssetType.GameObject);
        try {
            GameObject go = GameObject.Instantiate<GameObject> (original);
            return go;
        } catch (System.Exception) {
            Debug.LogErrorFormat (DebugFormat, "创建 AssetBundle 出错", assetbundleName);
            return null;
        }

    }

    /// <summary>
    /// 根据 AssetBundle 实例化对象
    /// </summary>
    /// <param name="assetbundleName">资源文件的AssetbundleName(无扩展名)</param>
    /// <param name="position">实例化位置</param>
    /// <param name="rotation">实例化轴向</param>
    /// <param name="parent">父物体</param>
    /// <param name="creater">资源实例化完成回调</param>
    /// <param name="customData">回调使用的自定义数据</param>
    /// <returns></returns>
    public static GameObject Create (string assetbundleName, Vector3 position, Quaternion rotation, Transform parent, Callback creater, params object[] customData) {
        GameObject obj = Create (assetbundleName, position, rotation, parent);
        if (creater != null && obj != null)
            creater (obj, customData);
        return obj;
    }

    /// <summary>
    /// 根据 AssetBundle 实例化对象并且不修改位置轴向等信息
    /// </summary>
    /// <param name="assetbundleName">资源文件AssetbundleName(无扩展名)</param>
    /// <param name="creater">资源实例化完成回调</param>
    /// <param name="customData">回调使用的自定义数据</param>
    /// <returns></returns>
    public static GameObject Create (string assetbundleName, Callback creater, params object[] customData) {
        GameObject obj = Create (assetbundleName);
        if (creater != null && obj != null)
            creater (obj, customData);
        return obj;
    }

    /// <summary>
    /// 获取指定资源的依赖项
    /// </summary>
    /// <param name="assetbundleName">资源文件AssetbundleName(无扩展名)</param>
    /// <returns></returns>
    private static void LoadDependencies (string assetbundleName) {
        string fullName = string.Format ("{0}{1}", assetbundleName, Config.Get<string> ("AssetBundleExtName")).ToLower ();
        string[] dps = Manifest.GetAllDependencies (fullName);
        if (dps.Length > 0) {
            for (int i = 0; i < dps.Length; i++) {
                LoadedAssetBundle loaded;
                if (LoadedBundles.TryGetValue (dps[i], out loaded)) {
                    loaded.count++;
                } else {
                    string _path = string.Format ("{0}/{1}", BundlePath, dps[i]);
                    LoadedBundles.Add (dps[i], new LoadedAssetBundle (AssetBundle.LoadFromFile (_path)));
                }
            }
        }
    }

    /// <summary>
    /// 释放 AssetBundle
    /// </summary>
    /// <param name="assetbundleName">资源文件AssetbundleName(无扩展名)</param>
    /// <param name="unloadused">是否释放还在使用的资源</param>
    public static void UnloadAssetBundle (string assetbundleName, bool unloadused = false) {
        UnloadDependencies (assetbundleName, unloadused);
        Unload (assetbundleName, unloadused);
    }

    /// <summary>
    /// 释放依赖项
    /// </summary>
    /// <param name="assetbundleName">资源文件AssetbundleName(无扩展名)</param>
    /// <param name="unloadused">是否释放还在使用的资源</param>
    private static void UnloadDependencies (string assetbundleName, bool unloadused = false) {
        string fullName = string.Format ("{0}{1}", assetbundleName, Config.Get<string> ("AssetBundleExtName")).ToLower ();
        string[] dps = Manifest.GetAllDependencies (fullName);
        if (dps.Length > 0) {
            for (int i = 0; i < dps.Length; i++) {
                Unload (dps[i], unloadused);
            }
        }
    }

    /// <summary>
    /// 释放Assetbundle
    /// </summary>
    /// <param name="assetbundleName">资源文件AssetbundleName(无扩展名)</param>
    /// <param name="unloadused">是否释放还在使用的资源</param>
    private static void Unload (string assetbundleName, bool unloadused = false) {
        LoadedAssetBundle loaded;
        if (LoadedBundles.TryGetValue (assetbundleName, out loaded)) {
            if (--loaded.count == 0) {
                Unload (loaded.ab);
                LoadedBundles.Remove (assetbundleName);
            }
        }
    }

    /// <summary>
    /// 释放Assetbundle
    /// </summary>
    /// <param name="ab">要释放的 AssetBundle</param>
    /// <param name="unloadused">是否释放还在使用的资源</param>
    private static void Unload (AssetBundle ab, bool unloadused = false) {
        ab.Unload (unloadused);
    }
}