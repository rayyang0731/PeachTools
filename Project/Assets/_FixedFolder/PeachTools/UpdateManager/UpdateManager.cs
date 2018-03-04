using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// Update管理器
/// </summary>
public class UpdateManager : MonoSingleton<UpdateManager> {
    private readonly List<IUpdate> _UpdateStore = new List<IUpdate> ();
    private readonly List<IFixedUpdate> _FixedUpdateStore = new List<IFixedUpdate> ();
    private readonly List<ILateUpdate> _LateUpdateStore = new List<ILateUpdate> ();

    private IUpdate[] UpdateStore = new IUpdate[0];
    private IFixedUpdate[] FixedUpdateStore = new IFixedUpdate[0];
    private ILateUpdate[] LateUpdateStore = new ILateUpdate[0];

#if UNITY_EDITOR
    private GameObject updateVirtual;
    private GameObject fixedUpdateVirtual;
    private GameObject lateUpdateVirtual;
    protected override void _Awake () {
        this.name = "[UpdateRuntime]";
        updateVirtual = new GameObject (string.Format ("[Update Pool:{0}]", UpdateStore.Length));
        updateVirtual.transform.SetParent (this.transform);
        fixedUpdateVirtual = new GameObject (string.Format ("[FixedUpdate Pool:{0}]", FixedUpdateStore.Length));
        fixedUpdateVirtual.transform.SetParent (this.transform);
        lateUpdateVirtual = new GameObject (string.Format ("[LateUpdate Pool:{0}]", LateUpdateStore.Length));
        lateUpdateVirtual.transform.SetParent (this.transform);
    }
#endif

    #region 移除 Update
    private void RemoveUpdate (IUpdate update) {
        if (_UpdateStore.Contains (update)) {
            _UpdateStore.Remove (update);
            UpdateStore = _UpdateStore.ToArray ();
        }
    }

    private void RemoveFixedUpdate (IFixedUpdate fixedUpdate) {
        if (_FixedUpdateStore.Contains (fixedUpdate)) {
            _FixedUpdateStore.Remove (fixedUpdate);
            FixedUpdateStore = _FixedUpdateStore.ToArray ();
        }
    }

    private void RemoveLateUpdate (ILateUpdate lateUpdate) {
        if (_LateUpdateStore.Contains (lateUpdate)) {
            _LateUpdateStore.Remove (lateUpdate);
            LateUpdateStore = _LateUpdateStore.ToArray ();
        }
    }

    public void Remove<T> (T _inst) {
        IUpdate update = _inst as IUpdate;
        if (update != null)
            RemoveUpdate (update);

        IFixedUpdate fixedUpdate = _inst as IFixedUpdate;
        if (fixedUpdate != null)
            RemoveFixedUpdate (fixedUpdate);

        ILateUpdate lateUpdate = _inst as ILateUpdate;
        if (lateUpdate != null)
            RemoveLateUpdate (lateUpdate);

#if UNITY_EDITOR
        UpdateRuntimeTimerCount ();
#endif
    }
    #endregion

    #region 添加 Update
    private void AddUpdate (IUpdate update) {
        _UpdateStore.Add (update);
        UpdateStore = _UpdateStore.ToArray ();
    }

    private void AddFixedUpdate (IFixedUpdate fixedUpdate) {
        _FixedUpdateStore.Add (fixedUpdate);
        FixedUpdateStore = _FixedUpdateStore.ToArray ();
    }

    private void AddLateUpdate (ILateUpdate lateUpdate) {
        _LateUpdateStore.Add (lateUpdate);
        LateUpdateStore = _LateUpdateStore.ToArray ();
    }

    public void Add<T> (T _instance_) {
        IUpdate update = _instance_ as IUpdate;
        if (update != null)
            AddUpdate (update);

        IFixedUpdate fixedUpdate = _instance_ as IFixedUpdate;
        if (fixedUpdate != null)
            AddFixedUpdate (fixedUpdate);

        ILateUpdate lateUpdate = _instance_ as ILateUpdate;
        if (lateUpdate != null)
            AddLateUpdate (lateUpdate);

#if UNITY_EDITOR
        UpdateRuntimeTimerCount ();
#endif
    }
    #endregion

    /// <summary>
    /// 是否存在此实例
    /// </summary>
    /// <param name="_instance_"></param>
    /// <returns></returns>
    public bool Exist<T> (T _instance_) {
        IUpdate update = _instance_ as IUpdate;
        if (update != null)
            return _UpdateStore.Contains (update);

        IFixedUpdate fixedUpdate = _instance_ as IFixedUpdate;
        if (fixedUpdate != null)
            return _FixedUpdateStore.Contains (fixedUpdate);

        ILateUpdate lateUpdate = _instance_ as ILateUpdate;
        if (lateUpdate != null)
            return _LateUpdateStore.Contains (lateUpdate);

        return false;
    }

#if UNITY_EDITOR
    /// <summary>
    /// 显示计时器数量
    /// </summary>
    private void UpdateRuntimeTimerCount () {
        this.updateVirtual.name = string.Format ("[Update Pool:{0}]", UpdateStore.Length);
        this.fixedUpdateVirtual.name = string.Format ("[FixedUpdate Pool:{0}]", FixedUpdateStore.Length);
        this.lateUpdateVirtual.name = string.Format ("[LateUpdate Pool:{0}]", LateUpdateStore.Length);
    }
#endif

    void Update () {
        if (UpdateStore.Length == 0) return;

        for (int i = 0; i < UpdateStore.Length; i++)
            if (UpdateStore[i] != null)
                UpdateStore[i]._Update ();
    }

    void FixedUpdate () {
        if (FixedUpdateStore.Length == 0) return;

        for (int i = 0; i < FixedUpdateStore.Length; i++)
            if (FixedUpdateStore[i] != null)
                FixedUpdateStore[i]._FixedUpdate ();
    }

    void LateUpdate () {
        if (LateUpdateStore.Length == 0) return;

        for (int i = 0; i < LateUpdateStore.Length; i++)
            if (LateUpdateStore[i] != null)
                LateUpdateStore[i]._LateUpdate ();
    }
}