using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通知中心
/// </summary>
public class NotifyCenter<K, T> {
	/// <summary>
	/// 监听列表
	/// </summary>
	private IDictionary<K, Action<T>> mylistens;

	public NotifyCenter () { mylistens = new Dictionary<K, Action<T>> (); }

	/// <summary>
	/// 消息监听
	/// </summary>
	/// <param name="number">消息号</param>
	/// <param name="action">消息事件</param>
	public void Listen (K number, Action<T> action) {
		if (action.Equals (null)) {
			Debug.LogError ("监听事件不能为 Null");
			return;
		}

		if (!mylistens.ContainsKey (number))
			mylistens.Add (number, action);
		else
			mylistens[number] += action;
	}

	/// <summary>
	/// 移除监听
	/// </summary>
	/// <param name="number">消息号</param>
	/// <param name="action">要移除的事件</param>
	public void RemoveListen (K number, Action<T> action) {
		if (mylistens.ContainsKey (number) && !action.Equals (null)) {
			mylistens[number] -= action;

			if (mylistens[number].Equals (null))
				mylistens.Remove (number);
		}
	}

	/// <summary>
	/// 移除此消息号整体监听
	/// </summary>
	/// <param name="number">消息号</param>
	/// <returns>成功移除返回 True,如果未找到相对应的消息号返回 False</returns>
	public bool RemoveListen (K number) {
		return mylistens.Remove (number);
	}

	/// <summary>
	/// 发送消息
	/// </summary>
	/// <param name="number">消息号</param>
	/// <param name="param">参数</param>
	public void Send (K number, T param) {
#if UNITY_EDITOR
		if (mylistens.Count <= 0) {
			Debug.LogWarning ("监听列表为空");
			return;
		}
		if (!mylistens.ContainsKey (number)) {
			Debug.LogWarningFormat ("不存在此消息号({0})的监听", number);
			return;
		}
#endif
		Action<T> action;
		if (mylistens.TryGetValue (number, out action))
			action.Invoke (param);
	}

	/// <summary>
	/// 清除所有监听
	/// </summary>
	public void Clear () { mylistens.Clear (); }
}