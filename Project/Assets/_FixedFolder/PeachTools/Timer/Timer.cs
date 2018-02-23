using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 计时器对象
/// </summary>
public sealed class Timer : IDisposable {
	public Timer () { }
	/// <summary>
	/// 计时器唯一标识符
	/// </summary>
	private long guid;
	/// <summary>
	/// 计时器持续时间
	/// </summary>
	private float duration;
	/// <summary>
	/// 是否循环
	/// </summary>
	private bool loop;
	/// <summary>
	/// 回调频率
	/// </summary>
	private float callbackFrequency;
	/// <summary>
	/// 计时开始时回调
	/// </summary>
	private Action<Timer> OnStart;
	/// <summary>
	/// 计时到设定时间回调
	/// </summary>
	private Action<Timer> OnCallback;
	/// <summary>
	/// 计时中回调
	/// </summary>
	private Action<Timer> OnUpdate;
	/// <summary>
	/// 计时器暂停回调
	/// </summary>
	private Action<Timer> OnPause;
	/// <summary>
	/// 计时器恢复回调
	/// </summary>
	private Action<Timer> OnResume;
	/// <summary>
	/// 计时结束回调
	/// </summary>
	private Action<Timer> OnStop;
	/// <summary>
	/// 计时取消回调
	/// </summary>
	private Action<Timer> OnCancel;
	/// <summary>
	/// 是否忽略 TimeScale
	/// </summary>
	private bool ignoreTimeScale;

	/// <summary>
	/// 是否暂停
	/// </summary>
	private bool isPause;

	/// <summary>
	/// 剩余计时时间
	/// </summary>
	private float remainTime;
	/// <summary>
	/// 下次回调时间
	/// </summary>
	private float nextCallbackTime;

	/// <summary>
	/// 开始次数
	/// </summary>
	private int startCount;
	/// <summary>
	/// 完成次数
	/// </summary>
	private int finishCount;

	/// <summary>
	/// 传入的参数
	/// </summary>
	private object[] args;
	/// <summary>
	/// 是否已经释放过资源
	/// </summary>
	private bool disposed = false;

	#region 公开属性
	/// <summary>
	/// 计时器唯一标识符
	/// </summary>
	public long GUID { get { return this.guid; } }

	/// <summary>
	/// 总持续时间
	/// </summary>
	public float Duration { get { return this.duration; } }

	/// <summary>
	/// 计时器是否循环
	/// </summary>
	public bool Loop { get { return this.loop; } }

	/// <summary>
	/// 计时器是否暂停
	/// </summary>
	public bool IsPause { get { return this.isPause; } }

	/// <summary>
	/// 计时器是否停止
	/// </summary>
	public bool IsStop { get { return !this.isPause && this.remainTime <= 0; } }

	/// <summary>
	/// 计时器是否忽略时间缩放
	/// </summary>
	public bool IgnoreTimeScale { get { return this.ignoreTimeScale; } }

	/// <summary>
	/// 回调频率
	/// </summary>
	public float CallbackFrequency { get { return this.callbackFrequency; } }

	/// <summary>
	/// 已用时间
	/// </summary>
	public float ElapsedTime { get { return this.duration - this.remainTime; } }

	/// <summary>
	/// 剩余时间
	/// </summary>
	public float RemainTime { get { return this.remainTime; } }

	/// <summary>
	/// 计时器已经完成的百分比
	/// </summary>
	public float Ratio { get { return 1 - this.remainTime / this.duration; } }

	/// <summary>
	/// 下次回调时间
	/// </summary>
	public float NextCallbackTime { get { return this.nextCallbackTime; } }

	/// <summary>
	/// 开始次数
	/// </summary>
	/// <returns></returns>
	public int StartCount { get { return this.startCount; } }
	/// <summary>
	/// 结束次数
	/// </summary>
	/// <returns></returns>
	public int FinishCount { get { return this.finishCount; } }

	/// <summary>
	/// 添加计时开始回调
	/// </summary>
	public Timer AddStartCallback (Action<Timer> callback) {
		if (callback != null)
			this.OnStart += callback;

		return this;
	}
	public Timer RemoveStartCallback (Action<Timer> callback) {
		if (callback != null)
			this.OnStart -= callback;

		return this;
	}

	/// <summary>
	/// 添加计时到设定时间回调
	/// </summary>
	public Timer AddCallback (Action<Timer> callback) {
		if (callback != null)
			this.OnCallback += callback;

		return this;
	}
	/// <summary>
	/// 移除计时到设定时间回调
	/// </summary>
	public Timer RemoveCallback (Action<Timer> callback) {
		if (callback != null)
			this.OnCallback -= callback;

		return this;
	}

	/// <summary>
	/// 添加计时中回调
	/// </summary>
	public Timer AddUpdateCallback (Action<Timer> callback) {
		if (callback != null)
			this.OnUpdate += callback;

		return this;
	}
	/// <summary>
	/// 移除计时中回调
	/// </summary>
	public Timer RemoveUpdateCallback (Action<Timer> callback) {
		if (callback != null)
			this.OnUpdate -= callback;

		return this;
	}
	/// <summary>
	/// 添加暂停回调
	/// </summary>
	public Timer AddPauseCallback (Action<Timer> callback) {
		if (callback != null)
			this.OnPause += callback;

		return this;
	}
	/// <summary>
	/// 移除暂停回调
	/// </summary>
	public Timer RemovePauseCallback (Action<Timer> callback) {
		if (callback != null)
			this.OnPause -= callback;

		return this;
	}

	/// <summary>
	/// 添加恢复计时回调
	/// </summary>
	public Timer AddResumeCallback (Action<Timer> callback) {
		if (callback != null)
			this.OnResume += callback;

		return this;
	}
	/// <summary>
	/// 移除恢复计时回调
	/// </summary>
	public Timer RemoveResumeCallback (Action<Timer> callback) {
		if (callback != null)
			this.OnResume -= callback;

		return this;
	}

	/// <summary>
	/// 添加结束回调
	/// </summary>
	public Timer AddStopCallback (Action<Timer> callback) {
		if (callback != null)
			this.OnStop += callback;

		return this;
	}
	/// <summary>
	/// 移除计时中回调
	/// </summary>
	public Timer RemoveStopCallback (Action<Timer> callback) {
		if (callback != null)
			this.OnStop -= callback;

		return this;
	}

	/// <summary>
	/// 添加取消回调
	/// </summary>
	public Timer AddCancelCallback (Action<Timer> callback) {
		if (callback != null)
			this.OnCancel += callback;

		return this;
	}
	/// <summary>
	/// 移除取消回调
	/// </summary>
	public Timer RemoveCancelCallback (Action<Timer> callback) {
		if (callback != null)
			this.OnCancel -= callback;

		return this;
	}

	/// <summary>
	/// 设置持续时间
	/// </summary>
	public Timer SetDuration (float duration) {
		if (duration > 0) {
			this.duration = duration;
		}
		return this;
	}

	/// <summary>
	/// 设置是否循环
	/// </summary>
	public Timer SetLoop (bool loop) {
		this.loop = loop;
		return this;
	}

	/// <summary>
	/// 设置回调频率
	/// </summary>
	public Timer SetCallbackFrequency (float callbackFrequency) {
		this.callbackFrequency = callbackFrequency > 0 ? callbackFrequency : 0;
		return this;
	}

	/// <summary>
	/// 设置参数对象，会覆盖原有对象
	/// </summary>
	public Timer SetParams (params object[] objs) {
		this.args = objs;
		return this;
	}

	/// <summary>
	/// 获取参数对象
	/// </summary>
	/// <typeparam name="T">参数类型</typeparam>
	/// <param name="index">参数索引</param>
	public T GetParam<T> (int index) {
		if (this.args != null && index < this.args.Length)
			return (T) this.args[index];
		return default (T);
	}

	/// <summary>
	/// 获取参数对象,默认获取第0个参数
	/// </summary>
	/// <typeparam name="T">参数类型</typeparam>
	public T GetParam<T> () {
		return GetParam<T> (0);
	}
	#endregion

	/// <summary>
	/// 初始运行变量
	/// </summary>
	private void ResetRunVariable () {
		this.isPause = true;
		this.remainTime = 0f;
		this.nextCallbackTime = 0f;
	}

	/// <summary>
	/// (私有方法)启动计时器
	/// </summary>
	/// <param name="duration">持续时间</param>
	/// <param name="onCallback">计时到设定时间回调</param>
	/// <param name="callFrequency">回调频率</param>
	/// <param name="loop">是否循环</param>
	/// <param name="ignoreTimeScale">是否忽略 TimeScale</param>
	private static Timer _startup (float duration, Action<Timer> onCallback, float callFrequency = 0f, bool loop = false, bool ignoreTimeScale = false) {
		if (onCallback != null) {
			Timer timer = Timer.Create (duration, onCallback, callFrequency, loop, ignoreTimeScale);
			timer.Startup ();
			return timer;
		} else
			throw new Exception ("启动计时器失败,回调方法不能为Null");
	}
	/// <summary>
	/// 启动计时器
	/// </summary>
	/// <param name="duration">持续时间</param>
	/// <param name="onCallback">计时到设定时间回调</param>
	/// <param name="callFrequency">回调频率</param>
	/// <param name="loop">是否循环</param>
	/// <param name="ignoreTimeScale">是否忽略 TimeScale</param>
	public static void Startup (float duration, Action<Timer> onCallback, float callFrequency = 0f, bool loop = false, bool ignoreTimeScale = false) {
		_startup (duration, onCallback, callFrequency, loop, ignoreTimeScale);
	}
	/// <summary>
	/// 启动计时器
	/// </summary>
	/// <param name="duration">持续时间</param>
	/// <param name="onCallback">计时到设定时间回调</param>
	/// <param name="guid">计时器唯一标识符</param>
	/// <param name="callFrequency">回调频率</param>
	/// <param name="loop">是否循环</param>
	/// <param name="ignoreTimeScale">是否忽略 TimeScale</param>
	public static void Startup (float duration, Action<Timer> onCallback, out long guid, float callFrequency = 0f, bool loop = false, bool ignoreTimeScale = false) {
		Timer timer = _startup (duration, onCallback, callFrequency, loop, ignoreTimeScale);
		if (timer != null)
			guid = timer.guid;
		else
			throw new Exception ("启动计时器失败,要启动的计时器为 Null");
	}
	/// <summary>
	/// 启动计时器
	/// </summary>
	public void Startup () {
		//表示未在计时
		if (this.remainTime <= 0) {
			Restart ();
		}
	}

	/// <summary>
	/// 创建一个Timer
	/// </summary>
	public static Timer Create (float duration, Action<Timer> callback, float callFrequency = 0f, bool loop = false, bool ignoreTimeScale = false) {
		if (callback != null) {
			Timer timer = TimerManager.Instance.Pool.Get ();

			timer.guid = System.DateTime.Now.Ticks;
			timer.duration = duration;
			timer.OnCallback = callback;
			timer.callbackFrequency = callFrequency;
			timer.loop = loop;
			timer.ignoreTimeScale = ignoreTimeScale;

			timer.ResetRunVariable ();

			return timer;
		} else {
			Debug.LogError ("创建计时器失败,回调方法不能为Null");
			return null;
		}
	}

	/// <summary>
	/// 无论是否正在计时，马上重新开始
	/// </summary>
	public void Restart () {
		this.InitTick ();

		TimerManager.Instance.AddTimer (this);

		if (OnStart != null)
			OnStart.Invoke (this);
	}

	/// <summary>
	/// 初始运行变量
	/// </summary>
	private void InitTick () {
		this.isPause = false;

		this.remainTime = this.duration;

		this.startCount++;

		if (this.callbackFrequency > 0) {
			this.nextCallbackTime = this.remainTime - this.callbackFrequency;
		}
	}

	/// <summary>
	/// 暂停
	/// </summary>
	public void Pause () {
		if (this.isPause) return;

		this.isPause = true;

		if (OnPause != null)
			OnPause.Invoke (this);
	}

	/// <summary>
	/// 继续
	/// </summary>
	public void Renumes () {
		if (!this.isPause) return;

		this.isPause = false;

		if (OnResume != null)
		OnResume.Invoke (this);
	}

	/// <summary>
	/// 停止
	/// </summary>
	public void Stop () {
		if (OnStop != null)
			OnStop.Invoke (this);

		ResetAndRecycle ();
	}

	/// <summary>
	/// 取消
	/// </summary>
	public void Cancel () {
		if (OnCancel != null)
			OnCancel.Invoke (this);

		ResetAndRecycle ();
	}

	/// <summary>
	/// 重置并回收计时器
	/// </summary>
	private void ResetAndRecycle () {
		this.ResetRunVariable ();
		ClearAllEvent ();
		TimerManager.Instance.RemoveTimer (this);
		TimerManager.Instance.Pool.Recycle (this);
	}

	/// <summary>
	/// 清理全部事件
	/// </summary>
	private void ClearAllEvent () {
		OnStart = null;
		OnCallback = null;
		OnUpdate = null;
		OnPause = null;
		OnResume = null;
		OnStop = null;
		OnCancel = null;
	}

	public void Tick (float delteTime) {
		if (isPause) return;

		this.remainTime -= delteTime;
		if (this.remainTime <= this.nextCallbackTime) {
			OnCallback.Invoke (this);
			if (remainTime <= 0f) {
				if (loop) {
					this.InitTick ();
				} else {
					this.Stop ();
				}
				this.finishCount++;
				return;
			}

			if (this.callbackFrequency > 0) {
				this.nextCallbackTime -= this.callbackFrequency;
				if (this.nextCallbackTime < 0) {
					this.nextCallbackTime = 0f;
				}
			} else {
				this.nextCallbackTime = 0f;
			}
		} else if (OnUpdate != null) {
			OnUpdate.Invoke (this);
		}
	}

	/// <summary>
	/// 清理所有正在使用的资源
	/// </summary>
	public void Dispose () {
		Close ();
		GC.SuppressFinalize (this);
	}

	private void Close () {
		if (!this.disposed) {
			guid = 0;
			duration = 0;
			loop = false;
			callbackFrequency = 0;
			OnStart = null;
			OnCallback = null;
			OnUpdate = null;
			OnPause = null;
			OnResume = null;
			OnStop = null;
			OnCancel = null;
			ignoreTimeScale = false;
			isPause = false;
			remainTime = 0;
			nextCallbackTime = 0;
			startCount = 0;
			finishCount = 0;
			args = null;

			this.disposed = true;
		}
	}
}