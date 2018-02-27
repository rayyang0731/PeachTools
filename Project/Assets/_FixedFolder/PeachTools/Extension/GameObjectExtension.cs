using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameObject 扩展
/// </summary>
public static class GameObjectExtension {

    /// <summary>
    /// 是否对象有绑定Rigidbody
    /// </summary>
    public static bool HasRigidbody (this GameObject go) {
        Rigidbody rigid = go.GetComponent<Rigidbody> ();
        return (rigid != null) && !rigid.Equals (null);
    }

    /// <summary>
    /// 是否对象有绑定Animation
    /// </summary>
    public static bool HasAnimation (this GameObject go) {
        Animation anim = go.GetComponent<Animation> ();
        return (anim != null) && !anim.Equals (null);
    }

    /// <summary>
    /// 是否对象有绑定Animator
    /// </summary>
    public static bool HasAnimator (this GameObject go) {
        Animator anim = go.GetComponent<Animator> ();
        return (anim != null) && !anim.Equals (null);
    }

    /// <summary>
    /// 设置对象的层
    /// </summary>
    /// <param name="layer">层</param>
    /// <param name="includeChild">是否改变子物体的层</param>
    /// <param name="includeInactive">是否改变未激活的子物体层</param>
    public static void SetLayer (this GameObject go, int layer, bool includeChild = false, bool includeInactive = true) {
        if (includeChild) {
            Transform[] arr = go.transform.GetComponentsInChildren<Transform> (includeInactive);

            for (int i = 0, imax = arr.Length; i < imax; ++i) {
                arr[i].gameObject.layer = layer;
            }
        } else {
            go.layer = layer;
        }
    }
    /// <summary>
    /// 设置对象的层
    /// </summary>
    /// <param name="layerName">层名称</param>
    /// <param name="includeChild">是否改变子物体的层</param>
    /// <param name="includeInactive">是否改变未激活的子物体层</param>
    public static void SetLayer (this GameObject go, string layerName, bool includeChild = false, bool includeInactive = true) {
        int layer = LayerMask.NameToLayer (layerName);
        go.SetLayer (layer, includeChild, includeInactive);
    }

    /// <summary>
    /// 获取对象包括子物体在内的所有Renderer
    /// </summary>
    /// <param name="includeInactive">是否改变未激活的子物体层</param>
    /// <returns></returns>
    public static Renderer[] GetRenderersInChildren (this GameObject go, bool includeInactive = true) {
        return go.GetComponentsInChildren<Renderer> (includeInactive);
    }
}