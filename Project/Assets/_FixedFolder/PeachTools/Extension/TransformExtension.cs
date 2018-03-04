using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Transform 扩展
/// </summary>
public static class TransformExtension {
    #region 设置对象世界坐标相关
    /// <summary>
    /// 设置对象世界坐标
    /// </summary>
    public static void SetPosition (this Transform t, Vector3 newPos) {
        t.position = newPos;
    }

    /// <summary>
    /// 设置对象世界坐标的X
    /// </summary>
    public static void SetPositionX (this Transform t, float newX) {
        t.position = new Vector3 (newX, t.position.y, t.position.z);
    }
    /// <summary>
    /// 设置对象世界坐标的Y
    /// </summary>
    public static void SetPositionY (this Transform t, float newY) {
        t.position = new Vector3 (t.position.x, newY, t.position.z);
    }
    /// <summary>
    /// 设置对象世界坐标的Z
    /// </summary>
    public static void SetPositionZ (this Transform t, float newZ) {
        t.position = new Vector3 (t.position.x, t.position.y, newZ);
    }
    #endregion

    #region 设置对象相对坐标相关
    /// <summary>
    /// 设置对象相对坐标
    /// </summary>
    public static void SetLocalPosition (this Transform t, Vector3 newPos) {
        t.localPosition = newPos;
    }
    /// <summary>
    /// 设置对象相对坐标的X
    /// </summary>
    public static void SetLocalPositionX (this Transform t, float newX) {
        t.localPosition = new Vector3 (newX, t.localPosition.y, t.localPosition.z);
    }
    /// <summary>
    /// 设置对象相对坐标的Y
    /// </summary>
    public static void SetLocalPositionY (this Transform t, float newY) {
        t.localPosition = new Vector3 (t.localPosition.x, newY, t.localPosition.z);
    }
    /// <summary>
    /// 设置对象相对坐标的Z
    /// </summary>
    public static void SetLocalPositionZ (this Transform t, float newZ) {
        t.localPosition = new Vector3 (t.localPosition.x, t.localPosition.y, newZ);
    }
    #endregion

    #region 设置对象世界欧拉角相关
    /// <summary>
    /// 设置对象世界欧拉角
    /// </summary>
    public static void SetEuler (this Transform t, Vector3 newEuler) {
        t.rotation = Quaternion.Euler (newEuler);
    }
    /// <summary>
    /// 设置对象世界欧拉角X
    /// </summary>
    public static void SetEulerX (this Transform t, float newX) {
        t.rotation = Quaternion.Euler (new Vector3 (newX, t.eulerAngles.y, t.eulerAngles.z));
    }
    /// <summary>
    /// 设置对象世界欧拉角Y
    /// </summary>
    public static void SetEulerY (this Transform t, float newY) {
        t.rotation = Quaternion.Euler (new Vector3 (t.eulerAngles.x, newY, t.eulerAngles.z));
    }
    /// <summary>
    /// 设置对象世界欧拉角Z
    /// </summary>
    public static void SetEuleZ (this Transform t, float newZ) {
        t.rotation = Quaternion.Euler (new Vector3 (t.eulerAngles.x, t.eulerAngles.y, newZ));
    }
    #endregion

    #region 设置对象相对欧拉角相关
    /// <summary>
    /// 设置对象相对欧拉角
    /// </summary>
    public static void SetLocalEuler (this Transform t, Vector3 newEuler) {
        t.localRotation = Quaternion.Euler (newEuler);
    }
    /// <summary>
    /// 设置对象相对欧拉角X
    /// </summary>
    public static void SetLocalEulerX (this Transform t, float newX) {
        t.localRotation = Quaternion.Euler (new Vector3 (newX, t.localEulerAngles.y, t.localEulerAngles.z));
    }
    /// <summary>
    /// 设置对象相对欧拉角Y
    /// </summary>
    public static void SetLocalEulerY (this Transform t, float newY) {
        t.localRotation = Quaternion.Euler (new Vector3 (t.localEulerAngles.x, newY, t.localEulerAngles.z));
    }
    /// <summary>
    /// 设置对象相对欧拉角Z
    /// </summary>
    public static void SetLocalEuleZ (this Transform t, float newZ) {
        t.localRotation = Quaternion.Euler (new Vector3 (t.localEulerAngles.x, t.localEulerAngles.y, newZ));
    }
    #endregion

    #region 设置对象相对缩放相关
    /// <summary>
    /// 设置对象相对缩放
    /// </summary>
    public static void SetLocalScale (this Transform t, Vector3 newScale) {
        t.localScale = newScale;
    }
    /// <summary>
    /// 设置对象相对欧拉角X
    /// </summary>
    public static void SetLocalScaleX (this Transform t, float newX) {
        t.localScale = new Vector3 (newX, t.localScale.y, t.localScale.z);
    }
    /// <summary>
    /// 设置对象相对欧拉角Y
    /// </summary>
    public static void SetLocalScaleY (this Transform t, float newY) {
        t.localScale = new Vector3 (t.localScale.x, newY, t.localScale.z);
    }
    /// <summary>
    /// 设置对象相对欧拉角Z
    /// </summary>
    public static void SetLocalScaleZ (this Transform t, float newZ) {
        t.localScale = new Vector3 (t.localScale.x, t.localScale.y, newZ);
    }
    #endregion

    #region 获得世界坐标相关
    /// <summary>
    /// 获得对象世界坐标X
    /// </summary>
    public static float GetPositionX (this Transform t) {
        return t.position.x;
    }

    /// <summary>
    /// 获得对象世界坐标Y
    /// </summary>
    public static float GetPositionY (this Transform t) {
        return t.position.y;
    }

    /// <summary>
    /// 获得对象世界坐标Z
    /// </summary>
    public static float GetPositionZ (this Transform t) {
        return t.position.z;
    }
    #endregion

    #region 获得相对坐标相关
    /// <summary>
    /// 获得对象相对坐标X
    /// </summary>
    public static float GetLocalPositionX (this Transform t) {
        return t.localPosition.x;
    }

    /// <summary>
    /// 获得对象相对坐标Y
    /// </summary>
    public static float GetLocalPositionY (this Transform t) {
        return t.localPosition.y;
    }

    /// <summary>
    /// 获得对象相对坐标Z
    /// </summary>
    public static float GetLocalPositionZ (this Transform t) {
        return t.localPosition.z;
    }
    #endregion

    #region 获得世界欧拉角相关
    /// <summary>
    /// 获得对象世界欧拉角X
    /// </summary>
    public static float GetEulerX (this Transform t) {
        return t.eulerAngles.x;
    }

    /// <summary>
    /// 获得对象世界欧拉角Y
    /// </summary>
    public static float GetEulerY (this Transform t) {
        return t.eulerAngles.y;
    }

    /// <summary>
    /// 获得对象世界欧拉角Z
    /// </summary>
    public static float GetEulerZ (this Transform t) {
        return t.eulerAngles.z;
    }
    #endregion

    #region 获得相对欧拉角相关
    /// <summary>
    /// 获得对象相对欧拉角X
    /// </summary>
    public static float GetLocalEulerX (this Transform t) {
        return t.localEulerAngles.x;
    }

    /// <summary>
    /// 获得对象相对欧拉角Y
    /// </summary>
    public static float GetLocalEulerY (this Transform t) {
        return t.localEulerAngles.y;
    }

    /// <summary>
    /// 获得对象相对欧拉角Z
    /// </summary>
    public static float GetLocalEulerZ (this Transform t) {
        return t.localEulerAngles.z;
    }
    #endregion

    #region 获得相对缩放相关
    /// <summary>
    /// 获得对象相对缩放X
    /// </summary>
    public static float GetLocalScaleX (this Transform t) {
        return t.localScale.x;
    }

    /// <summary>
    /// 获得对象相对缩放Y
    /// </summary>
    public static float GetLocalScaleY (this Transform t) {
        return t.localScale.y;
    }

    /// <summary>
    /// 获得对象相对缩放Z
    /// </summary>
    public static float GetLocalScaleZ (this Transform t) {
        return t.localScale.z;
    }
    #endregion

    /// <summary>
    /// 重置对象Transform的坐标、轴向、比例
    /// </summary>
    public static void Reset (this Transform t, bool pos = true, bool euler = true, bool scale = true) {
        if (pos)
            t.localPosition = Vector3.zero;
        if (euler)
            t.localEulerAngles = Vector3.zero;
        if (scale)
            t.localScale = Vector3.one;
    }

    /// <summary>
    /// 获取对象全部渲染器
    /// </summary>
    /// <param name="includeInactive">是否包含未激活的子物体</param>
    /// <param name="includeParticle">是否包含特效</param>
    /// <returns></returns>
    public static Renderer[] GetRenderers (this Transform t, bool includeInactive = true, bool includeParticle = false) {
        return t.gameObject.GetRenderers (includeInactive, includeParticle);
    }

    /// <summary>
    /// 获得粒子时长
    /// </summary>
    public static float GetParticleLength (this Transform t) {
        ParticleSystem[] particleSystems = t.GetComponentsInChildren<ParticleSystem> ();
        if (particleSystems.Length != 0) {
            float maxDuration = 0;
            foreach (ParticleSystem ps in particleSystems) {
                if (ps.emission.enabled) {
                    float duration = ps.main.startDelayMultiplier + ps.main.startLifetimeMultiplier;

                    if (ps.emission.rateOverTimeMultiplier != 0)
                        duration += ps.main.duration;

                    if (duration > maxDuration)
                        maxDuration = duration;
                }
            }
            return maxDuration;
        } else
            return 0;
    }
}