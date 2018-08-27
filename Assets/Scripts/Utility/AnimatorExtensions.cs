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
    }
}