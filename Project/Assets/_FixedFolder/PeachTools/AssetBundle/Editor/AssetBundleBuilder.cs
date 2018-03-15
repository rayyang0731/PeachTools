using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
/// <summary>
/// AssetBundle 生成器
/// </summary>
public class AssetBundleBuilder {

    private const string DebugFormat = "<color=#9400D3>[AssetBundleBuilder]</color> {0}:{1}";
    /// <summary>
    /// 标记要打包的 AssetBundle 文件
    /// </summary>
    [MenuItem ("Tools/AssetBundle/Set AssetBundleName", false, 0)]
    private static void MarkAssetBundleFiles () {
        List<string> markFile = GetWannaMarkFiles ();
        List<string> dependencies = GetDependencies (markFile);
        SetAssetBundleName (markFile, dependencies);
    }

    /// <summary>
    /// 获得要标记的文件
    /// </summary>
    /// <returns></returns>
    private static List<string> GetWannaMarkFiles () {
        string _rootPath = string.Format ("{0}/{1}", Application.dataPath, Config.Get<string> ("AssetFolder"));
        if (FileUtilities.CheckDirectory (_rootPath)) {
            string[] filePaths = Directory.GetFiles (_rootPath, "*.*", SearchOption.AllDirectories);
            List<string> allFiles = new List<string> ();
            for (int i = 0; i < filePaths.Length; i++) {
                if (JudgeExtension (filePaths[i])) {
                    string _filePath = filePaths[i].Substring (filePaths[i].IndexOf ("Assets"));
                    allFiles.Add (_filePath);
                    Debug.LogFormat (DebugFormat, "MarkFiles", _filePath);
                }
            }
            return allFiles;
        } else
            throw new Exception (string.Format ("资源文件夹不存在:{0}", _rootPath));
    }

    /// <summary>
    /// 判断后缀名
    /// </summary>
    /// <param name="filePath">文件地址</param>
    /// <returns></returns>
    private static bool JudgeExtension (string filePath) {
        return filePath.ToLower ().EndsWith (".prefab") ||
            filePath.ToLower ().EndsWith (".mp3") ||
            filePath.ToLower ().EndsWith (".ogg") ||
            filePath.ToLower ().EndsWith (".asset") ||
            filePath.ToLower ().EndsWith (".controller") ||
            filePath.ToLower ().EndsWith (".txt");
    }

    /// <summary>
    /// 获得依赖项
    /// </summary>
    /// <param name="markFiles">被标记为 AssetBundle 的文件</param>
    /// <returns></returns>
    private static List<string> GetDependencies (List<string> markFiles) {
        Dictionary<string, int> dic = new Dictionary<string, int> ();
        for (int i = 0; i < markFiles.Count; i++) {
            string[] singleDependencies = AssetDatabase.GetDependencies (markFiles[i]);
            for (int j = 0; j < singleDependencies.Length; j++) {
                if (dic.ContainsKey (singleDependencies[j]))
                    dic[singleDependencies[j]] += 1;
                else
                    dic.Add (singleDependencies[j], 1);
            }
        }
        List<string> dependencies = new List<string> ();
        foreach (var item in dic) {
            if (item.Value > 1 &&
                !markFiles.Contains (item.Key) &&
                !item.Key.ToLower ().EndsWith (".cs"))
                dependencies.Add (item.Key);
        }

        return dependencies;
    }

    /// <summary>
    /// 设置 Asset 文件名称
    /// </summary>
    /// <param name="markFiles">要标记的主对象</param>
    /// <param name="dependencies">要标记的依赖项</param>
    private static void SetAssetBundleName (List<string> markFiles, List<string> dependencies) {
        AssetImporter ai;
        string name;

        string bundleExtName = Config.Get<string> ("AssetBundleExtName");

        for (int i = 0; i < markFiles.Count; i++) {
            ai = AssetImporter.GetAtPath (markFiles[i]);
            string path = markFiles[i].Replace (string.Format ("{0}/{1}/", "Assets", Config.Get<string> ("AssetFolder")), string.Empty);
            path = path.Substring (0, path.LastIndexOf ('.'));
            name = string.Format ("{0}{1}", path, bundleExtName);
            if (string.IsNullOrEmpty (ai.assetBundleName) || ai.assetBundleName != name) {
                ai.assetBundleName = name;
                Debug.LogFormat (DebugFormat, "Main Bundle", markFiles[i]);
            }
        }

        for (int i = 0; i < dependencies.Count; i++) {
            ai = AssetImporter.GetAtPath (dependencies[i]);
            name = string.Format ("{0}{1}", AssetDatabase.AssetPathToGUID (dependencies[i]), bundleExtName);
            if (string.IsNullOrEmpty (ai.assetBundleName) || ai.assetBundleName != name) {
                ai.assetBundleName = name;
                Debug.LogFormat (DebugFormat, "Dependency", string.Format ("({0}){1}", name, dependencies[i]));
            }
        }
    }

    /// <summary>
    /// 清除 AssetBundleName
    /// </summary>
    [MenuItem ("Tools/AssetBundle/Clear AssetBundleName", false, 1)]
    private static void ClearAssetBundleName () {
        string[] all = AssetDatabase.GetAllAssetBundleNames ();
        for (int i = 0; i < all.Length; i++) {
            AssetDatabase.RemoveAssetBundleName (all[i], true);
        }
    }

    /// <summary>
    /// 当前打包平台
    /// </summary>
    /// <returns></returns>
    private static BuildTarget CurBuildTarget {
        get {
            BuildTarget curBuildTarget = BuildTarget.Android;

#if UNITY_IOS
            curBuildTarget = BuildTarget.iOS;
#elif UNITY_ANDROID
            curBuildTarget = BuildTarget.Android;
#elif UNITY_STANDALONE_WIN
            curBuildTarget = BuildTarget.StandaloneWindows;
#elif UNITY_STANDALONE_OSX
            curBuildTarget = BuildTarget.StandaloneOSX;
#endif
            return curBuildTarget;
        }
    }

    /// <summary>
    /// 打包资源文件
    /// </summary>
    [MenuItem ("Tools/AssetBundle/Build AssetBundles", false, 2)]
    private static void BuildAssetBundles () {
        string path = string.Format ("{0}/{1}",
            Directory.GetParent (Application.dataPath).FullName,
            Config.Get<string> ("BundleFolder"));
        Build (path, Config.Get<string> ("BundleFolder"), Config.Get<string> ("ManifestName"));
    }

    /// <summary>
    /// 打包资源文件
    /// </summary>
    [MenuItem ("Tools/AssetBundle/Build to StreamingAssets", false, 3)]
    private static void BuildAssetBundlesToStreamingAssets () {
        string path = Application.streamingAssetsPath;
        Build (path, "StreamingAssets", Config.Get<string> ("ManifestName"));
    }

    /// <summary>
    /// 打包
    /// </summary>
    /// <param name="path">资源地址</param>
    /// <param name="originManifestName">原始总配置文件名称</param>
    /// <param name="curManifestName">要使用的总配置文件名称</param>
    private static void Build (string path, string originManifestName, string curManifestName) {
        MarkAssetBundleFiles ();
        if (FileUtilities.CheckDirectory (path, true)) {
            BuildPipeline.BuildAssetBundles (
                path,
                BuildAssetBundleOptions.ChunkBasedCompression |
                BuildAssetBundleOptions.DeterministicAssetBundle,
                CurBuildTarget);
            string originManifestPath = string.Format ("{0}/{1}", path, originManifestName);
            string manifestPath = string.Format ("{0}/{1}", path, curManifestName);
            if (File.Exists (originManifestPath)) {
                if (File.Exists (manifestPath)) {
                    File.Delete (manifestPath);
                    File.Delete (string.Format ("{0}.manifest", manifestPath));
                }
                File.Move (originManifestPath, manifestPath);
                File.Move (string.Format ("{0}.manifest", originManifestPath), string.Format ("{0}.manifest", manifestPath));

            }
        }
        AssetDatabase.Refresh ();
    }
}