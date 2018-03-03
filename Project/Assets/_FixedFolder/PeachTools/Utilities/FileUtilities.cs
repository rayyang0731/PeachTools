using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// 文件工具集
/// </summary>
public static class FileUtilities {
    /// <summary>
    /// 检查目录
    /// </summary>
    /// <param name="directory">存储目录地址</param>
    /// <param name="isCreate">如果不存在该目录是否要创建</param>
    /// <returns>true为目录存在，false为目录不存在</returns>
    public static bool CheckDirectory (string directory, bool isCreate = false) {
        bool isExisted = Directory.Exists (directory);
        if (!isExisted) {
            if (isCreate) {
                Directory.CreateDirectory (directory);
                Debug.Log ("创建目录 : " + directory);
                isExisted = true;
            } else
                Debug.LogWarningFormat ("目录不存在 : {0}", directory);
        } else
            Debug.LogFormat ("此目录存在 : {0}", directory);
        return isExisted;
    }

    /// <summary>
    /// 获取文件列表
    /// </summary>
    /// <param name="folderPath">目录地址</param>
    public static string[] GetFilesPath (string folderPath) {
        DirectoryInfo DirInfo = new DirectoryInfo (folderPath);
        List<string> paths = new List<string> ();
        GetFilesPath (DirInfo, paths);
        return paths.ToArray ();
    }

    /// <summary>
    /// 获取文件路径
    /// </summary>
    private static void GetFilesPath (FileSystemInfo info, List<string> paths) {
        if (!info.Exists)
            return;

        DirectoryInfo dir = info as DirectoryInfo;
        if (dir == null)
            return;
        FileSystemInfo[] files = dir.GetFileSystemInfos ();
        for (int i = 0; i < files.Length; i++) {
            FileInfo file = files[i] as FileInfo;
            if (file != null) {
                if (JudgeExtensions (file)) {
                    string path = file.FullName.Substring (file.FullName.IndexOf ("Assets")).Replace ("\\", "/");
                    paths.Add (path);
                }
            } else
                GetFilesPath (files[i], paths);
        }
    }

    /// <summary>
    /// 判断后缀名是否存在
    /// </summary>
    private static bool JudgeExtensions (FileInfo file) {
        if (file.FullName.ToLower ().EndsWith (".prefab") ||
            file.FullName.ToLower ().EndsWith (".mp3") ||
            file.FullName.ToLower ().EndsWith (".ogg") ||
            file.FullName.ToLower ().EndsWith (".txt") ||
            file.FullName.ToLower ().EndsWith (".png") ||
            file.FullName.ToLower ().EndsWith (".jpg") ||
            file.FullName.ToLower ().EndsWith (".tga") ||
            file.FullName.ToLower ().EndsWith (".asset") ||
            file.FullName.ToLower ().EndsWith (".controller") ||
            file.FullName.ToLower ().EndsWith (".lua")) {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获取文件名
    /// </summary>
    /// <param name="fileFullPath">文件路径</param>
    /// <param name="includeExtensions">是否包含后缀名</param>
    /// <returns></returns>
    public static string GetFileName (string fileFullPath, bool includeExtensions = false) {
        if (includeExtensions)
            return Path.GetFileName (fileFullPath);
        else
            return Path.GetFileNameWithoutExtension (fileFullPath);
    }

    /// <summary>
    /// 根据后缀名获取文件夹下的文件地址
    /// </summary>
    /// <param name="rootPath">根目录</param>
    /// <param name="extensions">扩展名</param>
    /// <returns></returns>
    public static string[] GetFilePathsWithExtension (string rootPath, string extensions = ".txt") {
        if (!string.IsNullOrEmpty (extensions))
            return Directory.GetFiles (rootPath, string.Format ("*{0}", extensions));
        else
            return null;
    }

    /// <summary>
    /// 根据后缀名获取路径下所有文件以及子文件夹中文件
    /// </summary>
    /// <param name="path">全路径根目录</param>
    /// <param name="path">后缀名</param>
    /// <returns></returns>
    public static string[] GetAllFilePathsWithExtension (string path, string extensions = ".txt") {
        List<string> _list = new List<string> ();
        GetAllFilePathsWithExtension (path, _list, extensions);
        return _list.ToArray ();
    }

    /// <summary>
    /// 根据后缀名获取路径下所有文件以及子文件夹中文件
    /// </summary>
    /// <param name="path">全路径根目录</param>
    /// <param name="FileList">获得的文件的路径</param>
    /// <param name="path">后缀名</param>
    /// <returns></returns>
    private static void GetAllFilePathsWithExtension (string path, List<string> FileList, string extensions = ".txt") {
        DirectoryInfo dir = new DirectoryInfo (path);
        FileInfo[] fileInfo = dir.GetFiles (string.Format ("*{0}", extensions));
        DirectoryInfo[] dirInfo = dir.GetDirectories ();
        if (fileInfo.Length > 0)
            foreach (FileInfo f in fileInfo) {
                FileList.Add (f.FullName); //添加文件路径到列表中
            }
        if (dirInfo.Length > 0)
            //获取子文件夹内的文件列表，递归遍历
            foreach (DirectoryInfo d in dirInfo) {
                GetAllFilePathsWithExtension (d.FullName, FileList, extensions);
            }
#if UNITY_EDITOR
        string _log = "";
        for (int i = 0; i < FileList.Count; i++)
            _log += string.Format ("{0}\n", FileList[i]);
        Debug.Log (_log);
#endif
    }

    /// <summary>
    /// 获取文件目录
    /// </summary>
    /// <param name="fileFullPath"></param>
    /// <returns></returns>
    public static string GetDirectory (string fileFullPath) {
        DirectoryInfo info = new DirectoryInfo (fileFullPath);
        return info.Parent.FullName;
    }
}