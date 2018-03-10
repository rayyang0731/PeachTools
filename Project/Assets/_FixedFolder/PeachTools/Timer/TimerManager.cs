using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 计时器管理器
/// </summary>
public class TimerManager : Singleton<TimerManager>, IUpdate {
	/// <summary>
	/// 计时器池
	/// </summary>
	private ObjectPool<Timer> timerPool;
	/// <summary>
	/// 计时器池
	/// </summary>
	public ObjectPool<Timer> Pool {
		get {
			return timerPool;
		}
	}
	/// <summary>
	/// 对象池容量
	/// </summary>
	private const int poolCapacity = 10;
	/// <summary>
	/// 正在使用的计时器
	/// </summary>
	private List<Timer> _Timers;
	/// <summary>
	/// 要移除的计时器
	/// </summary>
	private List<Timer> _Removes;

	/// <summary>
	/// 计时器管理器虚拟体
	/// </summary>
	private GameObject TimeMgrVirtual;

	protected TimerManager () {
		Init ();
	}

	/// <summary>
	/// 初始化
	/// </summary>
	private void Init () {
		this.timerPool = new ObjectPool<Timer> (poolCapacity, createTimer, DestroyTimer);

		this._Timers = new List<Timer> ();
		this._Removes = new List<Timer> ();

#if UNITY_EDITOR
		TimeMgrVirtual = new GameObject ("[TimerRuntime (0)|Pool (0/0)]");
		MonoBehaviour.DontDestroyOnLoad (TimeMgrVirtual);
#endif
	}

	/// <summary>
	/// 删除计时器
	/// </summary>
	/// <param name="obj">要删除的计时器</param>
	private void DestroyTimer (Timer obj) {
		obj.Dispose ();
	}

	/// <summary>
	/// 创建计时器
	/// </summary>
	/// <returns></returns>
	private Timer createTimer () {
		return new Timer ();
	}

	/// <summary>
	/// 添加计时器
	/// </summary>
	/// <param name="timer">计时器</param>
	public void AddTimer (Timer timer) {
		if (!_Timers.Contains (timer)) {
			_Timers.Add (timer);
			if (!UpdateManager.Instance.Exist (this)) {
				UpdateManager.Instance.Add (this);
			}
#if UNITY_EDITOR
			UpdateRuntimeTimerCount ();
#endif
		}
	}

	/// <summary>
	/// 移除计时器
	/// </summary>
	/// <param name="timer">计时器</param>
	public void RemoveTimer (Timer timer) {
		if (_Timers.Contains (timer)) {
			_Removes.Add (timer);
		}
	}

	/// <summary>
	/// 获得计时器
	/// </summary>
	/// <param name="guid">计时器唯一标识符</param>
	/// <returns></returns>
	public Timer GetTimer (string guid) {
		return _Timers.Find ((t) => t.GUID == guid);
	}

	/// <summary>
	/// 是否包含这个计时器
	/// </summary>
	/// <param name="guid">计时器唯一标识符</param>
	/// <returns></returns>
	public bool ExistTimer (string guid) {
		return _Timers.Exists ((t) => t.GUID == guid);
	}

	public void _Update () {
		if (_Timers.Count > 0) {
			for (int i = 0, y = _Timers.Count; i < y; ++i) {
				Timer timer = _Timers[i];

				if (!timer.IsPause) {
					timer.Tick (timer.IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime);
				}
			}

			//移除已经停止的Timer
			if (_Removes.Count > 0) {
				for (int i = 0, y = _Removes.Count; i < y; i++) {
					_Timers.Remove (_Removes[i]);
				}

				_Removes.Clear ();

				if (_Timers.Count < 1) {
					UpdateManager.Instance.Remove (this);
				}
#if UNITY_EDITOR
				UpdateRuntimeTimerCount ();
#endif
			}
		}
	}

#if UNITY_EDITOR
	/// <summary>
	/// 显示计时器数量
	/// </summary>
	private void UpdateRuntimeTimerCount () {
		TimeMgrVirtual.name = string.Format ("[TimerRuntime ({0})|Pool ({1}/{2})]", _Timers.Count.ToString (), timerPool.Count, poolCapacity);
	}
#endif

	private void OnDestroy () {
		this.timerPool.Clear (true);
	}
}