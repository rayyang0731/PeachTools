using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> {
	protected static T _instance = null;
	public static T Instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<T> ();

				if (FindObjectsOfType<T> ().Length > 1) {
					Debug.LogErrorFormat ("{0}实例超过1个!", _instance);
					return _instance;
				}

				if (_instance == null) {
					string instanceName = typeof (T).Name;
					GameObject instanceGO = GameObject.Find (instanceName);

					if (instanceGO == null)
						instanceGO = new GameObject (instanceName);

					_instance = instanceGO.AddComponent<T> ();
					DontDestroyOnLoad (instanceGO); //保证实例不会被释放
					Debug.LogFormat ("添加一个新的单例:{0}", instanceName);
					return _instance;
				} else {
					Debug.LogWarningFormat ("已经存在这个实例:{0}", _instance.name);
					return _instance;
				}
			}
			return _instance;
		}
	}

	private void Awake () {
		if (_instance == null) {
			_instance = gameObject.GetComponent<T> ();

			if (_instance != null)
				DontDestroyOnLoad (gameObject);
		}
		_Awake ();
	}

	private void OnDestroy () {
		_instance = null;
		_OnDestroy ();
	}

	protected virtual void _Awake () { }
	protected virtual void _OnDestroy () { }
}