using System;
using System.Collections;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Wolfpack
{
    public class GlitchEffect : IGlitchEffect
    {
        public event Action OnGlitch;

        Material[] materials;
        AudioSource audioSource;
        GlitchEffectSettings settings;
        
        public void Initialize(Material[] materials, AudioSource audioSource, GlitchEffectSettings settings)
        {
            this.materials = materials;
            this.audioSource = audioSource;
            this.settings = settings;
        }
        
        public void PlayGlitchEffectConstantly(AudioClip glitchSound)
        {
            var audioClip = glitchSound != null ? glitchSound : settings.GlitchEffectSounds.GetRandomClip();
            audioSource.PlayOneShot(audioClip);
            var glitchAmount = Random.Range(settings.MinGlitchAmount, settings.MaxGlitchAmount);
            var hologramVelocity = Random.Range(settings.MinHologramVelocity, settings.MaxHologramVelocity);
            var glowIntensity = Random.Range(settings.MinGlowIntensity, settings.MaxGlowIntensity);
            SetMaterialProperties(glitchAmount, hologramVelocity, glowIntensity);
            OnGlitch?.Invoke();
        }

        public IEnumerator PlayGlitchEffectOnce(Action callback)
        {
            var audioClip = settings.GlitchEffectSounds.GetRandomClip();
            PlayGlitchEffectConstantly(audioClip);
            yield return new WaitForSeconds(audioClip.length);
            SetMaterialProperties(null, null, null);
            callback?.Invoke();
        }

        public IEnumerator PlayGlitchEffectConstantlyWithDelay()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(settings.MinTimeBetweenGlitches, settings.MaxTimeBetweenGlitches));
                var audioClip = settings.GlitchEffectSounds.GetRandomClip();
                PlayGlitchEffectConstantly(audioClip);
                yield return new WaitForSeconds(audioClip.length);
                SetMaterialProperties(null, null, null);
            }
        }
        
        void SetMaterialProperties(float? glitchAmount, float? hologramVelocity, float? glowIntensity)
        {
            materials.SetVector4Property("_Hologram_Texture_Tiling", new Vector4(0f, glitchAmount ?? settings.DefaultGlitchAmount, 0f, 0f));
            materials.SetFloatProperty("_Glow_Intensity", glowIntensity ?? settings.DefaultGlowIntensity);
            materials.SetFloatProperty("_Hologram_Velocity", hologramVelocity ?? settings.DefaultHologramVelocity);
        }
    }
}