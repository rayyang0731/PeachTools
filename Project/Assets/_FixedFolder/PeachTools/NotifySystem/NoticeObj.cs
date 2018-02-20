using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeObj : INoticeObj {
	/// <summary>
	/// 消息号
	/// </summary>
	/// <returns></returns>
	public int Number { get; private set; }
	/// <summary>
	/// 消息体
	/// </summary>
	/// <returns></returns>
	public object Body { get; private set; }

	public NoticeObj (int num, object body) {
		Number = num;
		Body = body;
	}

	public T GetBody<T> () {
		return (T) Body;
	}

	public bool TryGetBody<T> (out T body) where T : class {
		body = Body as T;
		return body != null;
	}
}