using UnityEngine;

namespace Wolfpack
{
    public static class AnimatorExtensions
    {
        public static float GetClipLength(this Animator animator, string clipName)
        {
            if (animator.runtimeAnimatorController != null)
            {
                AnimationClip first = null;
                foreach (var clip in animator.runtimeAnimatorController.animationClips)
                {
                    if (clip.name == clipName)
                    {
                        first = clip;
                        break;
                    }
                }

                if (first != null)
                    return first.length;
            };

            return 0f;
        }

        public static void SetPlaybackSpeed(this Animator animator, float value)
        {
            animator.speed = value;
        }
    }
}