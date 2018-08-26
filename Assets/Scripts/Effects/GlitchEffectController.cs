using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Wolfpack
{
    [RequireComponent(typeof(AudioSource))]
    public class GlitchEffectController : MonoBehaviour
    {
        public event Action GlitchEffectPlayed;
    
        [Header("Glitch Settings")]
        [SerializeField] AudioClip[] glitchSounds;
        [SerializeField] float minTimeBetweenGlitches = 5f;
        [SerializeField] float maxTimeBetweenGlitches = 12f;

        [Header("Glitch Effect Settings")]
        [SerializeField] float minGlitchAmount = 1f;
        [SerializeField] float maxGlitchAmount = 20f;
        [SerializeField] float minHologramVelocity = .1f;
        [SerializeField] float maxHologramVelocity = 2f;
        [SerializeField] float minGlowIntensity = 1f;
        [SerializeField] float maxGlowIntensity = 3f;

        public float DefaultGlowIntensity { get; set; }   
    
        AudioSource audioSource;
        Material[] materials;
    
        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            GetMaterials();
        }

        void Start()
        {
            StartCoroutine(PlayGlitchEffectWithDelay());
            materials.SetFloatProperty("_Glow_Intensity", DefaultGlowIntensity);
        }

        IEnumerator PlayGlitchEffectWithDelay()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(minTimeBetweenGlitches, maxTimeBetweenGlitches));

                var audioClip = GetRandomGlitchClip();
                audioSource.PlayOneShot(audioClip);
                var glitchAmount = Random.Range(minGlitchAmount, maxGlitchAmount);
                var hologramVelocity = Random.Range(minHologramVelocity, maxHologramVelocity);
                var glowIntensity = Random.Range(minGlowIntensity, maxGlowIntensity);
                SetMaterialProperties(glitchAmount, hologramVelocity, glowIntensity);
                OnGlitchEffectPlayed();

                yield return new WaitForSeconds(audioClip.length);
                SetMaterialProperties(null, null, null);
            }
        }

        protected virtual void OnGlitchEffectPlayed()
        {
            GlitchEffectPlayed?.Invoke();
        }

        void SetMaterialProperties(float? glitchAmount, float? hologramVelocity, float? glowIntensity)
        {
            materials.SetVector4Property("_Hologram_Texture_Tiling", new Vector4(0f, glitchAmount ?? 70f, 0f, 0f));
            materials.SetFloatProperty("_Glow_Intensity", glowIntensity ?? DefaultGlowIntensity);
            materials.SetFloatProperty("_Hologram_Velocity", hologramVelocity ?? 0.1f);
        }

        void GetMaterials()
        {
            materials = GetComponentsInChildren<Renderer>()?.SelectMany(renderer => renderer.materials).ToArray();
        }

        AudioClip GetRandomGlitchClip()
        {
            var randomClip = Random.Range(0, glitchSounds.Length);
            return glitchSounds[randomClip];
        }
    }
}