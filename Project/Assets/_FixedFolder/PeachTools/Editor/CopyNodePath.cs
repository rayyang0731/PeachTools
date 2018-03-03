using UnityEditor;
using UnityEngine;

public static partial class Tools {
    [MenuItem ("Tools/复制选中对象路径", false, 0)]
    static void CopyNodePathFunc () {
        string nodePath = "";
        GetNodePath (Selection.activeGameObject.transform, ref nodePath);
        Debug.LogFormat ("对象节点路径:{0}", nodePath);

        CopyToClipboard (nodePath);
    }

    /// <summary>
    /// 复制到剪贴板
    /// </summary>
    /// <param name="nodePath">对象节点路径</param>
    private static void CopyToClipboard (string nodePath) {
        //复制到剪贴板
        TextEditor editor = new TextEditor ();
        editor.text = nodePath;
        editor.SelectAll ();
        editor.Copy ();
    }

    /// <summary>
    /// 获取节点路径
    /// </summary>
    /// <param name="trans">要获取路径的对象</param>
    /// <param name="path">路径</param>
    static void GetNodePath (Transform trans, ref string path) {
        if (path == "") {
            path = trans.name;
        } else {
            path = trans.name + "/" + path;
        }

        if (trans.parent != null) {
            GetNodePath (trans.parent, ref path);
        }
    }
}