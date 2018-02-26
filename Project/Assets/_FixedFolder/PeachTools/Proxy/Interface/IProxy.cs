using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 代理接口
/// </summary>
public interface IProxy {
	void OnRegister ();
	void OnRemove ();
}