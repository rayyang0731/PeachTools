using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池
/// </summary>
/// <typeparam name="T">缓存对象类型</typeparam>
public class ObjectPool<T> where T : class, new () {

	//-------------------------
	//        实例部分
	//-------------------------
	/// <summary>
	/// 缓存最大数量
	/// </summary>
	private int _maxSize;
	/// <summary>
	/// 缓存队列
	/// </summary>
	private Queue<T> _cacheQueue;
	/// <summary>
	/// 由池衍生出来的全部对象
	/// </summary>
	private List<T> _allObjStore;
	/// <summary>
	/// 创建对象委托,对象池将会使用此委托创建对象,此委托不能为Null
	/// </summary>
	private Func<T> _createObject;
	/// <summary>
	/// 销毁对象委托,对象池将会使用此委托销毁对象,此委托不能为Null
	/// </summary>
	private Action<T> _destroyObject;

	//-------------------------
	//        公共委托
	//-------------------------
	/// <summary>
	/// 激活对象委托,对象池将会使用此委托激活对象
	/// </summary>
	public Action<T> onActiveObject;
	/// <summary>
	/// 取消激活对象委托,对象池将会使用此委托取消激活对象
	/// </summary>
	public Action<T> onInactiveObject;

	/// <summary>
	/// 创建一个对象池
	/// </summary>
	/// <param name="capacity">对象池容量</param>
	/// <param name="onCreate">创建对象委托方法,这个参数不能为Null</param>
	/// <param name="onDestroy">销毁对象委托方法,这个参数不能为Null</param>
	public ObjectPool (int capacity, Func<T> onCreate, Action<T> onDestroy) {
		if (onCreate == null || onDestroy == null)
			throw new Exception ("缓存池的创建对象委托与销毁对象委托不能为null.");

		this._maxSize = capacity;

		this._cacheQueue = new Queue<T> (_maxSize);

		this._allObjStore = new List<T> ();

		this._createObject = onCreate;

		this._destroyObject = onDestroy;
	}

	/// <summary>
	/// 创建一个对象池
	/// </summary>
	/// <param name="capacity">对象池容量</param>
	/// <param name="onCreate">创建对象委托方法,这个参数不能为Null</param>
	/// <param name="onDestroy">销毁对象委托方法,这个参数不能为Null</param>
	/// <param name="onActive">激活对象委托方法</param>
	/// <param name="onInactive">取消激活对象委托方法</param>
	public ObjectPool (int capacity, Func<T> onCreate, Action<T> onDestroy,
			Action<T> onActive, Action<T> onInactive):
		this (capacity, onCreate, onDestroy) {
			this.onActiveObject = onActive;

			this.onInactiveObject = onInactive;
		}

	/// <summary>
	/// 从对象池中获取一个缓存对象,如果池中没有对象,将使用创建方法创建一个对象
	/// </summary>
	public T Get () {
		T obj = null;

		if (_cacheQueue.Count > 0)
			obj = _cacheQueue.Dequeue ();

		if (obj == null) {
			obj = _createObject.Invoke ();
			this._allObjStore.Add (obj);
		}

		if (onActiveObject != null)
			onActiveObject.Invoke (obj);

		return obj;
	}

	/// <summary>
	/// 缓存对象,对象池满时将会使用销毁方法销毁对象
	/// </summary>
	public bool Recycle (T obj) {
		//保证鲁棒性
		if (obj == null || _cacheQueue.Contains (obj))
			return false;

		if (onInactiveObject != null)
			onInactiveObject.Invoke (obj);

		if (_cacheQueue.Count < _maxSize) {
			_cacheQueue.Enqueue (obj);
			return true;
		}

		this._allObjStore.Remove (obj);
		_destroyObject.Invoke (obj);
		return true;
	}

	/// <summary>
	/// 清空对象池,清空并销毁对象池中的对象
	/// </summary>
	/// <param name="destroyAll">是否销毁从池中衍生出来的所有对象</param>
	public void Clear (bool destroyAll = false) {
		while (_cacheQueue.Count > 0) {
			_destroyObject.Invoke (_cacheQueue.Dequeue ());
		}
		if (destroyAll) {
			this._allObjStore.ForEach ((obj) => {
				if (obj != null && !obj.Equals (null)) {
					_destroyObject.Invoke (obj);
				}
			});
		}
		//_cacheQueue.Clear();
	}

	/// <summary>
	/// 预加载对象池,预先填满对象池
	/// </summary>
	public void Preload () {
		Preload (_maxSize);
	}

	/// <summary>
	/// 预加载对象池,预先填满对象池
	/// </summary>
	/// <param name="number">预加载数量</param>
	public void Preload (int number) {
		if (number > _cacheQueue.Count && number <= _maxSize) {
			int preloadNum = number - _cacheQueue.Count;

			for (int i = 0; i < preloadNum; i++) {
				T obj = _createObject.Invoke ();

				if (onInactiveObject != null) {
					onInactiveObject.Invoke (obj);
				}
				_cacheQueue.Enqueue (obj);

				this._allObjStore.Add (obj);
			}
		}
	}

	/// <summary>
	/// 返回缓存池中是否存在这个对象
	/// </summary>
	public bool Contains (T obj) { return _cacheQueue.Contains (obj); }

	/// <summary>
	/// 返回缓存池中剩余对象的数量
	/// </summary>
	public int Count { get { return _cacheQueue.Count; } }

	/// <summary>
	/// 缓存池最大容量
	/// </summary>
	public int Size { get { return _maxSize; } set { _maxSize = value; } }
}