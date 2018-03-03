using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UGUI 序列帧动画
/// </summary>
[RequireComponent (typeof (Image))]
public class UGUIAnimation : BaseMonoBehaviour, IUpdate {

    /// <summary>
    /// 开始播放时执行
    /// </summary>
    private Action<UGUIAnimation> OnPlay;
    /// <summary>
    /// 播放结束时执行
    /// </summary>
    private Action<UGUIAnimation> OnFinish;
    /// <summary>
    /// 播放中执行(每更新一张图执行一次)
    /// </summary>
    private Action<UGUIAnimation> OnUpdate;

    /// <summary>
    /// 添加播放开始事件
    /// </summary>
    /// <param name="callback">回调</param>
    public UGUIAnimation AddPlayCallback (Action<UGUIAnimation> callback) {
        if (callback != null)
            OnPlay += callback;
        return this;
    }
    /// <summary>
    /// 添加播放结束事件
    /// </summary>
    /// <param name="callback">回调</param>
    public UGUIAnimation AddFinishCallback (Action<UGUIAnimation> callback) {
        if (callback != null)
            OnFinish += callback;
        return this;
    }
    /// <summary>
    /// 添加播放中事件
    /// </summary>
    /// <param name="callback">回调</param>
    public UGUIAnimation AddUpdateCallback (Action<UGUIAnimation> callback) {
        if (callback != null)
            OnUpdate += callback;
        return this;
    }

    /// <summary>
    /// 类型
    /// </summary>
    public enum STYLE {
        /// <summary>
        /// 仅执行一次
        /// </summary>
        ONCE,
        /// <summary>
        /// 循环
        /// </summary>
        LOOP,
        /// <summary>
        /// 来回重复
        /// </summary>
        PINGPONG,
    }

    /// <summary>
    /// 是否运行直接播放
    /// </summary>
    [SerializeField]
    private bool playOnAwake = true;
    /// <summary>
    /// 帧率
    /// </summary>
    [SerializeField]
    private int frameRate = 30;
    /// <summary>
    /// 类型
    /// </summary>
    [SerializeField]
    private STYLE style;
    /// <summary>
    /// 结束自动删除
    /// </summary>
    [SerializeField]
    private bool autoDestroy;
    /// <summary>
    /// 要调用的图片
    /// </summary>
    [SerializeField]
    private Sprite[] imageStore;
    /// <summary>
    /// 是否正在播放
    /// </summary>
    [SerializeField]
    private bool isPlaying;

    private Image img;
    private int index = 1;
    private float timer;
    private float deltaTime;

    /// <summary>
    /// 是否运行直接播放
    /// </summary>
    /// <returns></returns>
    public bool PlayOnAwake { get { return playOnAwake; } }
    /// <summary>
    /// 帧率
    /// </summary>
    /// <returns></returns>
    public float FrameRate { get { return frameRate; } }
    /// <summary>
    /// 持续时间
    /// </summary>
    /// <returns></returns>
    public float Duration { get { return 1 / frameRate * imageStore.Length; } }
    /// <summary>
    /// 类型
    /// </summary>
    /// <returns></returns>
    public STYLE Style { get { return style; } }
    /// <summary>
    /// 结束自动删除
    /// </summary>
    /// <returns></returns>
    public bool AutoDestroy { get { return autoDestroy; } }
    /// <summary>
    /// 要调用的图片
    /// </summary>
    /// <returns></returns>
    public Sprite[] ImageStore { get { return imageStore; } }
    /// <summary>
    /// 手动创建
    /// </summary>
    /// <param name="_images">序列帧图片</param>
    /// <param name="_frameRate">帧率</param>
    /// <param name="_style">类型</param>
    /// <param name="_autoDestroy">结束自动删除</param>
    /// <param name="_playOnAwake">自动播放</param>
    public UGUIAnimation ManualCreate (
        Sprite[] _images, int _frameRate = 30,
        STYLE _style = STYLE.ONCE,
        bool _autoDestroy = true, bool _playOnAwake = true) {
        imageStore = _images;
        frameRate = _frameRate;
        style = _style;
        autoDestroy = _autoDestroy;
        playOnAwake = _playOnAwake;
        return this;
    }

    #region 排序
    /// <summary>
    /// 对要使用的图片进行排序
    /// </summary>
    [ContextMenu ("Sort Images")]
    public void SortImages () {
        if (imageStore != null) {
            for (int x = 0; x < imageStore.Length; x++) {
                for (int y = x + 1; y < imageStore.Length; y++) {
                    int xvalue = Match (imageStore[x].name);
                    int yvalue = Match (imageStore[y].name);

                    if (xvalue > yvalue) {
                        Sprite temp = imageStore[x];
                        imageStore[x] = imageStore[y];
                        imageStore[y] = temp;
                    }
                }
            }
        }
    }

    private int Match (string str) {
        System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match (str, "\\d+");
        if (m.Success && m.Length > 0) {
            return int.Parse (m.Value);
        }
        return -1;
    }
    #endregion

    private void OnValidate () {
        if (imageStore != null && imageStore.Length > 0) {
            GetComponent<Image> ().sprite = imageStore[0];
        }
    }

    /// <summary>
    /// 播放
    /// </summary>
    public void Play () {
        this.timer = this.deltaTime = 1f / this.frameRate;
        this.index = 0;
        this.isPlaying = true;

        if (OnPlay != null)
            OnPlay.Invoke (this);
    }

    /// <summary>
    /// 停止
    /// </summary>
    public void Stop () {
        if (autoDestroy) {
            transform.SetParent (null);
            GameObject.Destroy (this.gameObject);
        } else {
            isPlaying = false;
        }

        if (OnFinish != null) {
            OnFinish.Invoke (this);
        }
    }

    protected override void _Awake () {
        img = GetComponent<Image> ();
    }

    private void Start () {
        if (playOnAwake)
            Play ();
    }

    public void _Update () {
        if (isPlaying) {
            timer -= Time.deltaTime;

            if (timer <= 0f) {
                int length = imageStore.Length;
                timer = deltaTime;
                int progress = Mathf.Abs (index);
                img.sprite = imageStore[progress];

                if (OnUpdate != null)
                    OnUpdate.Invoke (this);

                index++;

                if (Mathf.Abs (index) >= length) {
                    switch (style) {
                        case STYLE.ONCE:
                            Stop ();
                            break;

                        case STYLE.LOOP:
                            index = 0;
                            break;

                        case STYLE.PINGPONG:
                            if (index > 0)
                                index = -(imageStore.Length - 1);
                            break;
                    }
                }
            }
        }
    }
}