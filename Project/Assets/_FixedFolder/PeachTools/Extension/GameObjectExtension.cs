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
    /// 获取对象全部渲染器
    /// </summary>
    /// <param name="includeInactive">是否包含未激活的子物体</param>
    /// <param name="includeParticle">是否包含特效</param>
    /// <returns></returns>
    public static Renderer[] GetRenderers (this GameObject go, bool includeInactive = true, bool includeParticle = false) {
        Renderer[] rs = go.GetComponentsInChildren<Renderer> (includeInactive);

        if (!includeParticle) {
            List<Renderer> nonParticleRs = new List<Renderer> ();
            foreach (Renderer r in rs) {
                if (r.GetType () != typeof (ParticleSystemRenderer) &&
                    r.GetType () != typeof (TrailRenderer)) {
                    nonParticleRs.Add (r);
                }
            }
            return nonParticleRs.ToArray ();
        } else
            return rs;
    }

    /// <summary>
    /// 获得粒子时长
    /// </summary>
    public static float GetParticleLength (this GameObject go) {
        return go.transform.GetParticleLength ();
    }

    /// <summary>
    /// 移除组件
    /// </summary>
    public static void RemoveComponent<T> (this GameObject go) where T : Component {
        T comp = go.GetComponent<T> ();
        if (comp != null)
            GameObject.Destroy (comp);
    }

    /// <summary>
    /// 是否激活组件
    /// </summary>
    /// <param name="_enable">是否激活</param>
    /// <param name="includeChild">是否影响子物体</param>
    public static void EnableComponent<T> (this GameObject go, bool _enable, bool includeChild = false) where T : Behaviour {
        go.transform.EnableComponent<T> (_enable, includeChild);
    }
}