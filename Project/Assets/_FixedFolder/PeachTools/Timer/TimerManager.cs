using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 计时器管理器
/// </summary>
public sealed class TimerManager : MonoSingleton<TimerManager> {
	/// <summary>
	/// 正在使用的计时器
	/// </summary>
	private List<Timer> _Timers;
	/// <summary>
	/// 要移除的计时器
	/// </summary>
	private List<Timer> _Removes;

	protected override void _Awake () {
		Init ();
	}

	/// <summary>
	/// 初始化
	/// </summary>
	private void Init () {
		this._Timers = new List<Timer> ();
		this._Removes = new List<Timer> ();
	}

	/// <summary>
	/// 添加计时器
	/// </summary>
	/// <param name="timer">计时器</param>
	public void AddTimer (Timer timer) {
		if (!_Timers.Contains (timer)) {
			_Timers.Add (timer);
			if (!this.enabled) {
				this.enabled = true;
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
	public Timer GetTimer (long guid) {
		return _Timers.Find ((t) => t.GUID == guid);
	}

	/// <summary>
	/// 是否包含这个计时器
	/// </summary>
	/// <param name="guid">计时器唯一标识符</param>
	/// <returns></returns>
	public bool ExistTimer (long guid) {
		return _Timers.Exists ((t) => t.GUID == guid);
	}

	private void Update () {
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
					this.enabled = false;
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
		gameObject.name = string.Format ("[TimerRuntime ({0})]", _Timers.Count.ToString ());
	}
#endif
}