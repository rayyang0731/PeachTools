using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消息系统
/// </summary>
public static class NotifySystem {
	private static NotifyCenter<int, INoticeObj> notifyCenter = new NotifyCenter<int, INoticeObj> ();

	#region 发送消息
	/// <summary>
	/// 发送消息
	/// </summary>
	/// <param name="noticeObj">消息对象</param>
	public static void Send (INoticeObj noticeObj) {
		notifyCenter.Send (noticeObj.Number, noticeObj);
	}
	/// <summary>
	/// 发送消息
	/// </summary>
	/// <param name="number">消息号</param>
	/// <param name="obj">要传递的参数</param>
	public static void Send (int number, object obj) {
		INoticeObj _noticeObj = new NoticeObj (number, obj);
		Send (_noticeObj);
	}
	/// <summary>
	/// 发送消息
	/// </summary>
	/// <param name="number">消息号</param>
	public static void Send (int number) {
		Send (number, null);
	}
	#endregion 

	#region 监听
	/// <summary>
	/// 监听消息
	/// </summary>
	/// <param name="number">消息号</param>
	/// <param name="action">回调方法</param>
	public static void Listen (int number, Action<INoticeObj> action) {
		notifyCenter.Listen (number, action);
	}
	#endregion

	#region 移除监听
	/// <summary>
	/// 移除监听
	/// </summary>
	/// <param name="number">消息号</param>
	/// <param name="action">回调方法</param>
	public static void RemoveListen (int number, Action<INoticeObj> action) {
		notifyCenter.RemoveListen (number, action);
	}
	/// <summary>
	/// 移除此消息号整体监听
	/// </summary>
	/// <param name="number">消息号</param>
	public static void RemoveListen (int number) {
		notifyCenter.RemoveListen (number);
	}

	/// <summary>
	/// 清除所有监听
	/// </summary>
	public static void Clear () {
		notifyCenter.Clear ();
	}
	#endregion 
}