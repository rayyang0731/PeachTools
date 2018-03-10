using UnityEngine;

/// <summary>
/// UnityObjectPool 对象池标记
/// </summary>
public class UnityObjectPoolSign : BaseMonoBehaviour {
    /// <summary>
    /// 所属对象池
    /// </summary>
    private UnityObjectPool pool;
    /// <summary>
    /// 计时器GUID
    /// </summary>
    private string timerGUID;

    /// <summary>
    /// 签署这个对象
    /// </summary>
    public void Sign (UnityObjectPool _pool) {
        this.pool = _pool;
    }

    /// <summary>
    /// 释放这个对象
    /// </summary>
    public void Release () {
        pool = null;
        Destroy (this);
    }

    /// <summary>
    /// 让对象池回收这个对象
    /// </summary>
    public void Recycle () {
        ReleaseTimer ();
        if (pool == null || !pool.Recycle (this.gameObject))
            DestroySignObject ();
    }

    /// <summary>
    /// 延迟让对象池回收这个对象
    /// </summary>
    /// <param name="delay">延迟时间</param>
    public void RecycleDelay (float delay) {
        if (pool != null)
            Timer.Startup (delay, (t) => Recycle (), out timerGUID);
    }

    /// <summary>
    /// 释放计时器
    /// </summary>
    private void ReleaseTimer () {
        if (TimerManager.Instance.ExistTimer (timerGUID)) {
            Timer timer = TimerManager.Instance.GetTimer (timerGUID);
            timer.Stop ();
        }
    }

    /// <summary>
    /// 销毁掉这个签署的对象
    /// </summary>
    public void DestroySignObject () {
        Release ();
        this.transform.SetParent (null);
        Destroy (this.gameObject);
    }

    /// <summary>
    /// 对象正在销毁
    /// </summary>
    protected override void _OnDestroy () {
        ReleaseTimer ();
        if (pool != null) {
            Debug.LogWarningFormat (
                "有被对象池签署标记的对象没按正常流程销毁，销毁对象名称：{0}，PoolName: {1}",
                gameObject.name, pool.poolName);
            pool = null;
        }
    }
}