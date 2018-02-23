using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池管理
/// </summary>
public class PoolManager : Singleton<PoolManager> {

    /// <summary>
    /// 对象池信息
    /// </summary>
    private class PoolInfo {
        /// <summary>
        /// 对象预制体
        /// </summary>
        public GameObject prefab;
        /// <summary>
        /// 容量
        /// </summary>
        public int capacity;
        /// <summary>
        /// 是否预加载kd
        /// </summary>
        public bool preload;
        /// <summary>
        /// 组名
        /// </summary>
        public string group;
    }

    /// <summary>
    /// 对象池管理器父物体
    /// </summary>
    private Transform __parent__;
    /// <summary>
    /// 对象池字典
    /// </summary>
    private IDictionary<string, UnityObjectPool> dic_pool;
    /// <summary>
    /// 对象池信息列表
    /// </summary>
    private List<PoolInfo> list_poolInfo;

    private PoolManager () {
        dic_pool = new Dictionary<string, UnityObjectPool> ();

        if (__parent__ == null || __parent__.Equals (null)) {
            GameObject go = new GameObject ("[UnityObject Pools]");
            __parent__ = go.transform;
            GameObject.DontDestroyOnLoad (go);
        }
    }

    /// <summary>
    /// 添加加载信息
    /// </summary>
    /// <param name="_prefab">对象路径</param>
    /// <param name="_capacity">对象池容量</param>
    /// <param name="_preload">是否预加载</param>
    public void AddLoadInfo (GameObject _prefab, int _capacity, bool _preload) {
        AddLoadInfo (_prefab, _capacity, _preload, string.Empty);
    }

    /// <summary>
    /// 添加加载信息,带有组信息
    /// </summary>
    /// <param name="_prefab">对象预制体</param>
    /// <param name="_capacity">对象池容量</param>
    /// <param name="_preload">是否预加载</param>
    /// <param name="_group">组名</param>
    public void AddLoadInfo (GameObject _prefab, int _capacity, bool _preload, string _group) {
        if (_prefab == null || _prefab.Equals (null)) return;

        if (list_poolInfo == null)
            list_poolInfo = new List<PoolInfo> ();

        PoolInfo info = list_poolInfo.Find (item => item.prefab.Equals (_prefab));

        if (info == null) {
            info = new PoolInfo ();
            list_poolInfo.Add (info);
            info.prefab = _prefab;
        }

        info.capacity = _capacity;
        info.preload = _preload;
        info.group = _group;
    }

    /// <summary>
    /// 改变对象池的组
    /// </summary>
    /// <param name="poolName">对象池名称</param>
    /// <param name="group">组名</param>
    public void ChangePoolGroup (string poolName, string group) {
        UnityObjectPool pool;
        if (TryGetPool (poolName, out pool))
            pool.group = group;
    }

    /// <summary>
    /// 开始加载所有加载信息
    /// </summary>
    public void StartLoadAllInfos () {
        if (list_poolInfo == null) return;
        for (int i = 0; i < list_poolInfo.Count; i++) {
            PoolInfo info = list_poolInfo[i];
            LoadPool (info.prefab, info.capacity, info.preload, info.group);
        }
        list_poolInfo.Clear ();
        list_poolInfo = null;
    }

    /// <summary>
    /// 释放所有对象池
    /// </summary>
    /// <param name="destroyAll">是否销毁从池中衍生出来的所有对象</param>
    public void ReleaseAllPools (bool destroyAll = false) {
        foreach (var item in dic_pool)
            item.Value.DestroyPool (destroyAll);
        dic_pool.Clear ();
    }

    /// <summary>
    /// 释放默认组的对象池
    /// </summary>
    /// <param name="destroyAll">是否销毁从池中衍生出来的所有对象</param>
    public void ReleasePoolsByDefaultGroup (bool destroyAll = false) {
        ReleasePoolsByGroup (string.Empty, destroyAll);
    }

    /// <summary>
    /// 释放标记为某个组的对象池
    /// </summary>
    /// <param name="group">组名</param>
    /// <param name="destroyAll">是否销毁从池中衍生出来的所有对象</param>
    public void ReleasePoolsByGroup (string group, bool destroyAll = false) {
        if (group == null) return;
        foreach (var item in dic_pool)
            if (item.Value.group.Equals (group))
                item.Value.DestroyPool (destroyAll);
    }

    /// <summary>
    /// 加载对象池
    /// </summary>
    /// <param name="prefab">对象路径</param>
    /// <param name="capacity">容量</param>
    /// <param name="preload">是否预加载</param>
    /// <param name="group">组名</param>
    public void LoadPool (GameObject prefab, int capacity, bool preload, string group) {
        string poolName = prefab.name;

        if (HasPool (poolName)) return;

        if (prefab == null) {
            Debug.LogErrorFormat ("创建对象池，加载对象失败！请检查路径：{0}", prefab.name);
            return;
        }

        UnityObjectPool.CreatePool (poolName, prefab, capacity, preload, group);
    }

    /// <summary>
    /// 创建一个Unity对象池
    /// </summary>
    /// <param name="poolName">对象池名称</param>
    /// <param name="prefab">预制体</param>
    /// <param name="capacity">容量</param>
    /// <param name="preload">是否预加载</param>
    /// <returns></returns>
    public UnityObjectPool CreatePool (string poolName, GameObject prefab, int capacity, bool preload) {
        return UnityObjectPool.CreatePool (poolName, prefab, capacity, preload);
    }

    /// <summary>
    /// 创建一个Unity对象池
    /// </summary>
    /// <param name="poolName">对象池名称</param>
    /// <param name="prefab">预制体</param>
    /// <param name="capacity">容量</param>
    /// <param name="preload">是否预加载</param>
    /// <param name="group">组名</param>
    /// <returns></returns>
    public UnityObjectPool CreatePool (string poolName, GameObject prefab, int capacity, bool preload, string group) {
        return UnityObjectPool.CreatePool (poolName, prefab, capacity, preload, group);
    }

    /// <summary>
    /// 添加一个对象池到管理器, 可以自定义组标记
    /// </summary>
    /// <param name="pool">对象池</param>
    public void AddPool (UnityObjectPool pool) {
        AddPool (pool, string.Empty);
    }
    /// <summary>
    /// 添加一个对象池到管理器, 可以自定义组标记
    /// </summary>
    /// <param name="pool">对象池</param>
    /// <param name="group">组名</param>
    public void AddPool (UnityObjectPool pool, string group) {
        if (pool == null) return;
        if (!HasPool (pool.poolName)) {
            dic_pool.Add (pool.poolName, pool);
            pool.transform.SetParent (__parent__);
        } else {
            Debug.LogError (string.Format ("\"{0}\"这个对象池名字已经存在了,请输入不重复的对象池名称", pool.poolName), pool);
            return;
        }
    }

    /// <summary>
    /// 获取一个对象池
    /// </summary>
    /// <param name="poolName">对象池名称</param>    
    public UnityObjectPool GetPool (string poolName) {
        UnityObjectPool pool;
        if (!TryGetPool (poolName, out pool))
            Debug.LogWarningFormat ("不存在这个对象池，请检查是否已经创建：{0}", poolName);
        return pool;
    }

    /// <summary>
    /// 尝试获取一个对象池
    /// </summary>
    /// <param name="poolName">对象池名称</param>
    /// <param name="pool">对象池</param>
    /// <returns></returns>
    public bool TryGetPool (string poolName, out UnityObjectPool pool) {
        return dic_pool.TryGetValue (poolName, out pool);
    }

    /// <summary>
    /// 判断对象池是否存在
    /// </summary>
    /// <param name="poolName">对象池名称</param>
    /// <returns></returns>
    public bool HasPool (string poolName) {
        return dic_pool.ContainsKey (poolName);
    }

    /// <summary>
    /// 移除管理的对象池
    /// </summary>
    /// <param name="poolName">对象池名称</param>
    public bool RemovePool (string poolName) {
        return dic_pool.Remove (poolName);
    }

    /// <summary>
    /// 销毁一个对象池
    /// </summary>
    /// <param name="poolName">对象池名称</param>
    public bool DestroyPool (string poolName) {
        UnityObjectPool pool;
        if (TryGetPool (poolName, out pool)) {
            pool.DestroyPool ();
            return true;
        }
        return false;
    }

}