using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> where T : Singleton<T> {
	protected static T _instance = null;
	protected Singleton () { }
	public static T Instance {
		get {
			if (_instance == null) {
				_instance = System.Activator.CreateInstance (typeof (T), true) as T;
				if (_instance == null)
					throw new System.Exception ("创建单例失败");
				Debug.LogFormat ("添加一个新的单例:{0}", typeof (T).Name);
			}
			return _instance;
		}
	}
}