using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 代理
/// </summary>
public class ProxyManager : Singleton<ProxyManager> {
	private Dictionary<Type, IProxy> dic_proxy;

	private ProxyManager () {
		dic_proxy = new Dictionary<Type, IProxy> ();
	}

	/// <summary>
	/// 获得代理
	/// </summary>
	/// <param name="_type">代理类型</param>
	/// <returns></returns>
	public IProxy Get (Type _type) {
		IProxy _proxy = null;
		if (!dic_proxy.TryGetValue (_type, out _proxy)) {
			_proxy = Activator.CreateInstance (_type, true) as DataProxy;
			if (_proxy != null)
				dic_proxy.Add (_type, _proxy);
			else
				throw new Exception (string.Format ("获得{0}代理失败.", _type.Name));
		}
		return _proxy;
	}

	/// <summary>
	/// 获得代理
	/// </summary>
	/// <returns></returns>
	public T Get<T> () where T : IProxy,
	new () {
		return (T) Get (typeof (T));
	}

	/// <summary>
	/// 移除代理
	/// </summary>
	/// <param name="_type">代理类型</param>
	/// <returns></returns>
	public bool Remove (Type _type) {
		IProxy _proxy = null;
		if (dic_proxy.TryGetValue (_type, out _proxy)) {
			_proxy.OnRemove ();
			return dic_proxy.Remove (_type);
		}
		return false;
	}

	/// <summary>
	/// 移除代理
	/// </summary>
	/// <returns></returns>
	public bool Remove<T> () where T : IProxy {
		return Remove (typeof (T));
	}

	/// <summary>
	/// 清除全部缓存的代理
	/// </summary>
	public void Clear () {
		foreach (var item in dic_proxy)
			item.Value.OnRemove ();
		dic_proxy.Clear ();
	}
}