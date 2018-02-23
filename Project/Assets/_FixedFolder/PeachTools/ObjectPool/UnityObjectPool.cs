using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unity GameObject对象池
/// </summary>
public class UnityObjectPool : BaseMonoBehaviour {
	//--------------------
	//      静 态
	//--------------------
	/// <summary>
	/// 创建一个Unity对象池
	/// </summary>
	/// <param name="_poolName">对象池名称</param>
	/// <param name="_prefab">对象预设</param>
	/// <param name="_capacity">容量</param>
	/// <param name="_preload">是否预加载</param>
	/// <returns></returns>
	public static UnityObjectPool CreatePool (string poolName, GameObject prefab, int capacity, bool preload) {
		return CreatePool (poolName, prefab, capacity, preload, string.Empty);
	}
	/// <summary>
	/// 创建一个Unity对象池
	/// </summary>
	/// <param name="_poolName">对象池名称</param>
	/// <param name="_prefab">对象预设</param>
	/// <param name="_capacity">容量</param>
	/// <param name="_preload">是否预加载</param>
	/// <param name="_group">组名</param>
	/// <returns></returns>
	public static UnityObjectPool CreatePool (string _poolName, GameObject _prefab, int _capacity, bool _preload, string _group) {
		GameObject go = new GameObject (_poolName);
		UnityObjectPool _pool = go.AddComponent<UnityObjectPool> ();
		_pool.poolName = _poolName;
		_pool.prefab = _prefab;
		_pool.capacity = _capacity;
		_pool.preload = _preload;
		_pool.group = _group;
		_pool.Start ();
		return _pool;
	}

	//--------------------               
	//       实 例    
	//--------------------
	/// <summary>
	/// 对象池
	/// </summary>
	private ObjectPool<GameObject> pool;
	/// <summary>
	/// 对象池名称
	/// </summary>
	public string poolName;
	/// <summary>
	/// 对象预制
	/// </summary>
	public GameObject prefab;
	/// <summary>
	/// 容量
	/// </summary>
	public int capacity = 5;
	/// <summary>
	/// 是否预加载
	/// </summary>
	public bool preload;
	/// <summary>
	/// 组名
	/// </summary>
	public string group;

	/// <summary>
	/// 创建对象时调用的方法
	/// </summary>
	public Action<GameObject> onCreateObj;
	/// <summary>
	/// 销毁对象之前调用的方法
	/// </summary>
	public Action<GameObject> onDestroyObj;

	private void Start () {
		if (pool == null) {
			if (prefab == null) {
				Debug.LogErrorFormat ("prefab未添加引用,对象池创建失败.对象池: {0}", poolName);
				return;
			}

			if (capacity < 1) {
				Debug.LogErrorFormat ("对象池容量小于1,不能使用对象池.对象池: {0}", poolName);
				return;
			}

			CreatePool ();
		}
	}

	/// <summary>
	/// 创建对象池
	/// </summary>
	private void CreatePool () {
		//初始化对象池
		pool = new ObjectPool<GameObject> (
			capacity,
			OnCreateGO, OnDestroyGO,
			OnActiveGO, OnInactiveGO);

		if (preload) pool.Preload ();

		PoolManager.Instance.AddPool (this);
	}

	/// <summary>
	/// 销毁释放这个对象池
	/// </summary>
	/// <param name="destroyAll">是否销毁从池中衍生出来的所有对象</param>
	public void DestroyPool (bool destroyAll = false) {
		pool.Clear (destroyAll);
		pool = null;
		prefab = null;

		onCreateObj = null;
		onDestroyObj = null;

		this.transform.SetParent (null);
		GameObject.Destroy (this.gameObject);
	}

	/// <summary>
	/// 获取生成预制体对象
	/// </summary>
	public GameObject Get () {
		GameObject go = pool.Get ();
#if UNITY_EDITOR
		gameObject.name = string.Format ("[{0} ({1})]", poolName, pool.Count.ToString ());
#endif  
		return go;
	}

	/// <summary>
	/// 获取生成预制体对象
	/// </summary>
	/// <param name="sign">对象池标记</param>
	/// <returns></returns>
	public GameObject Get (out UnityObjectPoolSign sign) {
		GameObject go = Get ();
		sign = go.GetComponent<UnityObjectPoolSign> ();
		return go;
	}

	/// <summary>
	/// 回收预制体对象
	/// </summary>
	/// <param name="go">要回收的对象</param>
	/// <returns></returns>
	public bool Recycle (GameObject go) {
		if (go == null || go.Equals (null) || pool == null) return false;

		pool.Recycle (go);
#if UNITY_EDITOR
		gameObject.name = string.Format ("[{0} ({1})]", poolName, pool.Count.ToString ());
#endif
		return true;
	}

	/// <summary>
	/// 清空对象池
	/// </summary>
	/// <param name="destroyAll">是否销毁从池中衍生出来的所有对象</param>
	public void Clear (bool destroyAll = false) { pool.Clear (destroyAll); }

	/// <summary>
	/// 预加载对象池
	/// </summary>
	public void Preload () { pool.Preload (); }

	/// <summary>
	/// 预加载指定数量的对象
	/// </summary>
	public void Preload (int count) { pool.Preload (count); }

	/// <summary>
	/// 创建对象委托
	/// </summary>
	private GameObject OnCreateGO () {
		GameObject go = GameObject.Instantiate (prefab);
		//签署标记这个对象
		go.AddComponent<UnityObjectPoolSign> ().Sign (this);

		if (onCreateObj != null)
			onCreateObj.Invoke (go);
		return go;
	}

	/// <summary>
	/// 销毁对象委托
	/// </summary>
	private void OnDestroyGO (GameObject go) {
		//如果为true表示这是已经销毁的GameObject
		if (go == null || go.Equals (null)) return;

		if (onDestroyObj != null)
			onDestroyObj.Invoke (go);

		UnityObjectPoolSign sign = go.GetComponent<UnityObjectPoolSign> ();
		//如果没有标记表示这不是对象池创建的对象  如果是 调用销毁对象
		if (sign != null) sign.DestroySignObject ();
	}

	/// <summary>
	/// 激活对象委托
	/// </summary>
	private void OnActiveGO (GameObject go) {
		go.transform.SetParent (null);
		go.SetActive (true);
	}

	/// <summary>
	/// 取消激活对象委托
	/// </summary>
	private void OnInactiveGO (GameObject go) {
		if (go == null || go.Equals (null)) return;

		go.transform.SetParent (this.transform);
		go.SetActive (false);
	}
}