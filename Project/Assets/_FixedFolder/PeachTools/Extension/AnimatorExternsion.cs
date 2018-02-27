using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 动作控制器扩展
/// </summary>
public static class AnimatorExternsion {
    /// <summary>
    /// 获取动作时长
    /// </summary>
    /// <param name="animationName">动作名称</param>
    public static float GetLength (this Animator animator, string animationName) {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (var ac in clips) {
            if (ac.name.Equals (animationName))
                return ac.length;
        }
        Debug.Log (animator.name + "不包含名为(" + animationName + ")的动作");
        return 0;
    }
}