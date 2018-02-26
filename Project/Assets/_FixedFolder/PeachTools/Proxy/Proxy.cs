using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 代理
/// </summary>
public abstract class Proxy : IProxy {
	public Proxy () {
		OnRegister ();
	}
	/// <summary>
	/// 代理被注册
	/// </summary>
	public abstract void OnRegister ();

	/// <summary>
	/// 代理被移除
	/// </summary>
	public abstract void OnRemove ();
}