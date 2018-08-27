using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Wolfpack
{
    public interface IGlitchEffect
    {
        void Initialize(Material[] materials, AudioSource audioSource, GlitchEffectSettings settings);
        IEnumerator PlayGlitchEffectOnce([CanBeNull] Action callback);
        void PlayGlitchEffectConstantly([CanBeNull] AudioClip glitchSound);
        IEnumerator PlayGlitchEffectConstantlyWithDelay();
        event Action OnGlitch;
    }
}