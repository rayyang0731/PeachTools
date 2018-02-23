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

    #region 移除 Update
    public void RemoveUpdate (IUpdate update) {
        if (_UpdateStore.Contains (update)) {
            _UpdateStore.Remove (update);
            UpdateStore = _UpdateStore.ToArray ();
        }
    }

    public void RemoveFixedUpdate (IFixedUpdate fixedUpdate) {
        if (_FixedUpdateStore.Contains (fixedUpdate)) {
            _FixedUpdateStore.Remove (fixedUpdate);
            FixedUpdateStore = _FixedUpdateStore.ToArray ();
        }
    }

    public void RemoveLateUpdate (ILateUpdate lateUpdate) {
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
    }
    #endregion

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