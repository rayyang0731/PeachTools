using System;
using System.Collections;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class SFXAudio : BaseMonoBehaviour {
    /// <summary>
    /// 默认名称
    /// </summary>
    public const string DEFAULT_NAME = "SFXAudio";
    public Action<SFXAudio> OnStop;

    private AudioSource audioSource;

    public string SFXAudioName {
        get { return gameObject.name; }
        set { gameObject.name = value; }
    }

    public AudioSource AudioSource {
        get { return audioSource; }
    }

    public bool IsPlaying {
        get { return audioSource.isPlaying; }
    }

    protected override void _Awake () {
        if (audioSource == null) { Init (); }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init () {
        this.audioSource = gameObject.GetComponent<AudioSource> ();
        this.audioSource.loop = false;
    }

    /// <summary>
    /// 播放
    /// </summary>
    /// <param name="clip">音频</param>
    /// <param name="loop">是否循环</param>
    public void Play (AudioClip clip, bool loop) {
        Play (clip, loop, audioSource.volume);
    }

    /// <summary>
    /// 播放
    /// </summary>
    /// <param name="clip">音频</param>
    /// <param name="loop">是否循环</param>
    /// <param name="volume">播放音量</param>
    public void Play (AudioClip clip, bool loop, float volume) {
        this.gameObject.SetActive (true);

        if (audioSource.isPlaying) {
            audioSource.Stop ();
        }
        audioSource.volume = volume;
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play ();
        this.enabled = !loop;

        if (!loop) {
            Invoke ("Stop", clip.length);
        }
    }

    /// <summary>
    /// 播放
    /// </summary>
    /// <param name="clip">音频</param>
    /// <param name="spatialBlend">空间混合(0-2D|1-3D)</param>
    /// <param name="loop">是否循环</param>
    /// <param name="volume">播放音量</param>
    public void Play (AudioClip clip, float spatialBlend, bool loop, float volume) {
        audioSource.spatialBlend = spatialBlend;
        Play (clip, loop, volume);
    }

    /// <summary>
    /// 停止
    /// </summary>
    public void Stop () {
        if (IsInvoking ("Stop")) {
            CancelInvoke ("Stop");
        }

        if (audioSource.isPlaying) {
            audioSource.Stop ();
        }

        this.audioSource.clip = null;
        this.gameObject.SetActive (false);

        if (this.OnStop != null) {
            this.OnStop.Invoke (this);
        }
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <returns></returns>
    public static SFXAudio Create () {
        GameObject obj = new GameObject (DEFAULT_NAME);
        SFXAudio sfx = obj.AddComponent<SFXAudio> ();
        sfx.Init ();
        return sfx;
    }
}