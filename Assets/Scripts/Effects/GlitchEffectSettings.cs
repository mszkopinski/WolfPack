using UnityEngine;

namespace Wolfpack
{
    [CreateAssetMenu(menuName = "WolfPack/Glitch Effect Settings", fileName = "GlitchSettings")]
    public class GlitchEffectSettings : ScriptableObject
    {
        [Header("Main Settings")] 
        public bool IsGlitchAutomatic;
        public float MinTimeBetweenGlitches = 5f;
        public float MaxTimeBetweenGlitches = 12f;
        
        [Header("Material Settings")]
        public float MinGlitchAmount = 1f;
        public float DefaultGlitchAmount = 70f;
        public float MaxGlitchAmount = 20f;
        public float MinHologramVelocity = .1f;
        public float DefaultHologramVelocity = 0.1f;
        public float MaxHologramVelocity = 2f;
        public float MinGlowIntensity = 1f;
        public float DefaultGlowIntensity = 3f;
        public float MaxGlowIntensity = 3f;
        
        [Header("Audio")]
        public AudioClip[] GlitchEffectSounds;
    }
}