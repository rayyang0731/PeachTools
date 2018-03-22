using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class AudioManager : MonoSingleton<AudioManager> {
    private AudioSource audioSource;
    public AudioSource AudioSource { get { return audioSource; } }

    /// <summary>
    /// BGM音量
    /// </summary>
    [SerializeField]
    [Range (0, 1)]
    private float BGMVolume;
    /// <summary>
    /// 音效音量
    /// </summary>
    [SerializeField]
    [Range (0, 1)]
    private float SFXVolume;

    /// <summary>
    /// 排斥音效计数
    /// </summary>
    private int RepelSFXCount;

    /// <summary>
    /// 音效池
    /// </summary>
    private ObjectPool<SFXAudio> SFXPool;

    protected override void _Awake () { OnInit (); }

    private void OnInit () {
        audioSource = GetComponent<AudioSource> ();
        audioSource.enabled = false;

        this.BGMVolume = 1f;
        this.SFXVolume = 1f;
        this.RepelSFXCount = 0;

        this.SFXPool = new ObjectPool<SFXAudio> (20, OnCreateSFX, OnDestroySFX);

        UpdateBGMVolume ();
    }

    /// <summary>
    /// 创建音效源对象
    /// </summary>
    private SFXAudio OnCreateSFX () {
        SFXAudio sfx = SFXAudio.Create ();
        sfx.OnStop = RecycleSFX;

        Transform trans = sfx.gameObject.transform;
        trans.SetParent (this.transform);
        return sfx;
    }

    /// <summary>
    /// 删除音效源对象
    /// </summary>
    private void OnDestroySFX (SFXAudio sfx) {
        sfx.transform.SetParent (null);
        GameObject.Destroy (sfx.gameObject);
    }

    /// <summary>
    /// 回收音效
    /// </summary>
    private void RecycleSFX (SFXAudio sfx) {
        SFXPool.Recycle (sfx);
        if (RepelSFXCount > 0) {
            if (--RepelSFXCount <= 0)
                UpdateBGMVolume ();
        }
    }

    /// <summary>
    /// 设置BGM音量 0 - 1
    /// </summary>
    public void SetBGMVolume (float volume) {
        this.BGMVolume = Mathf.Clamp01 (volume);
        UpdateBGMVolume ();
    }

    /// <summary>
    /// 设置音效音量 0 - 1
    /// </summary>
    public void SetSFXVolume (float volume) {
        this.SFXVolume = Mathf.Clamp01 (volume);
    }

    /// <summary>
    /// 设定音频监听器音量
    /// </summary>
    public void SetListenerVolume (float volume) {
        AudioListener.volume = volume;
    }

    /// <summary>
    /// 暂停全部音频
    /// </summary>
    /// <returns></returns>
    public bool PauseAllAudio {
        get { return AudioListener.pause; }
        set { AudioListener.pause = value; }
    }

    /// <summary>
    /// 更新检测排斥音效
    /// </summary>
    private void UpdateBGMVolume () {
        if (this.RepelSFXCount <= 0) {
            audioSource.volume = BGMVolume;
        }
    }

    /// <summary>
    /// 检测BGM剪辑的有效性
    /// </summary>
    private bool DetectionBGMClip () {
        bool isEnabled = audioSource.clip != null;
        audioSource.enabled = isEnabled;
        return isEnabled;
    }

    /// <summary>
    /// 停止播放BGM
    /// </summary>
    public void StopBGM () {
        if (audioSource.isPlaying) {
            audioSource.Stop ();
        }
    }

    /// <summary>
    /// 暂停BGM
    /// </summary>
    public void PauseBGM () {
        if (audioSource.isPlaying) {
            audioSource.Pause ();
        }
    }

    /// <summary>
    /// 取消暂停BGM
    /// </summary>
    public void UnPauseBGM () {
        if (audioSource.clip != null) {
            audioSource.UnPause ();
        }
    }

    /// <summary>
    /// 播放BGM
    /// </summary>
    /// <param name="audioClip">音频剪辑</param>
    /// <param name="loop">是否循环</param>
    public void PlayBGM (AudioClip audioClip, bool loop = true) {
        StopBGM ();
        audioSource.clip = audioClip;
        if (DetectionBGMClip ()) {
            audioSource.loop = loop;
            audioSource.Play ();
        }
    }

    /// <summary>
    /// 判断BGM是否正在播放
    /// </summary>
    public bool BGMIsPlaying () {
        return audioSource.isPlaying;
    }

    /// <summary>
    /// 判断正在播放的BGM是否audioName
    /// </summary>
    /// <param name="audioName">音频名称</param>
    /// <returns></returns>
    public bool BGMIsPlaying (string audioName) {
        return audioSource.clip != null && audioName.Equals (audioSource.clip.name);
    }

    /// <summary>
    /// 播放一个在零点的音效
    /// </summary>
    /// <param name="clip">音频</param>
    /// <param name="loop">是否循环</param>
    /// <param name="spatialBlend">空间混合(0-2D|1-3D)</param>
    /// <returns></returns>
    public SFXAudio PlaySFX (AudioClip clip, bool loop = false, float spatialBlend = 0f) {
        SFXAudio sfx = SFXPool.Get ();
        sfx.Play (clip, spatialBlend, loop, SFXVolume);
        return sfx;
    }

    /// <summary>
    /// 播放音效,可以设置音效播放的目标
    /// </summary>
    /// <param name="target">要发声的对象</param>
    /// <param name="clip">音频</param>
    /// <param name="loop">是否循环</param>
    /// <param name="spatialBlend">空间混合(0-2D|1-3D)</param>
    /// <returns></returns>
    public SFXAudio PlaySFX (Transform target, AudioClip clip, bool loop = false, float spatialBlend = 0f) {
        SFXAudio sfx = PlaySFX (clip, loop, spatialBlend);
        sfx.transform.SetParent (target);
        return sfx;
    }

    /// <summary>
    /// 播放音效,可以设置音效播放的位置
    /// </summary>
    /// <param name="position">要发声的位置</param>
    /// <param name="clip">音频</param>
    /// <param name="loop">是否循环</param>
    /// <param name="spatialBlend">空间混合(0-2D|1-3D)</param>
    /// <returns></returns>
    public SFXAudio PlaySFX (Vector3 position, AudioClip clip, bool loop = false, float spatialBlend = 0f) {
        SFXAudio sfx = PlaySFX (clip, loop, spatialBlend);
        sfx.transform.position = position;
        return sfx;
    }

    /// <summary>
    /// 播放音效,可以压低BGM的音量
    /// </summary>
    /// <param name="clip">音频</param>
    /// <param name="RepelBGM">排斥 BGM 的音量</param>
    /// <param name="loop">是否循环</param>
    /// <param name="spatialBlend">空间混合(0-2D|1-3D)</param>
    /// <returns></returns>
    public SFXAudio PlaySFX (AudioClip clip, float RepelBGM, bool loop = false, float spatialBlend = 0f) {
        SFXAudio sfx = PlaySFX (clip, loop, spatialBlend);
        //如果排斥BGM，降低BGM音量
        if (RepelBGM > 0f) {
            RepelSFXCount++;
            audioSource.volume = RepelBGM;
        }
        return sfx;
    }

    /// <summary>
    /// 播放音效,可以压低BGM的音量,设置音效播放的目标
    /// </summary>
    /// <param name="target">要发声的对象</param>
    /// <param name="clip">音频</param>
    /// <param name="RepelBGM">排斥 BGM 的音量</param>
    /// <param name="loop">是否循环</param>
    /// <param name="spatialBlend">空间混合(0-2D|1-3D)</param>
    /// <returns></returns>
    public SFXAudio PlaySFX (Transform target, AudioClip clip, float RepelBGM, bool loop = false, float spatialBlend = 0f) {
        SFXAudio sfx = PlaySFX (clip, RepelBGM, loop, spatialBlend);
        sfx.transform.SetParent (target);
        return sfx;
    }

    /// <summary>
    /// 播放音效,可以压低BGM的音量,设置音效播放的位置
    /// </summary>
    /// <param name="position">要发声的位置</param>
    /// <param name="clip">音频</param>
    /// <param name="RepelBGM">排斥 BGM 的音量</param>
    /// <param name="loop">是否循环</param>
    /// <param name="spatialBlend">空间混合(0-2D|1-3D)</param>
    /// <returns></returns>
    public SFXAudio PlaySFX (Vector3 position, AudioClip clip, float RepelBGM, bool loop = false, float spatialBlend = 0f) {
        SFXAudio sfx = PlaySFX (clip, RepelBGM, loop, spatialBlend);
        sfx.transform.position = position;
        return sfx;
    }
}