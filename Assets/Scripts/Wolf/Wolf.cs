using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Wolfpack
{
    public sealed class Wolf : MonoBehaviour
    {
        public static Action<Transform> WolfAppeared;
        public IGlitchEffect GlitchEffect { get; private set; }
        public GlitchEffectSettings GlitchEffectSettings;

        AudioSource audioSource;

        [Inject]
        public void Init(IGlitchEffect glitchEffect)
        {
            GlitchEffect = glitchEffect;
        }

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        void Start()
        {
            WolfAppeared?.Invoke(transform);
            GlitchEffect.Initialize(
                GetComponentsInChildren<Renderer>()
                    .SelectMany(renderer => renderer.materials)
                    .Where(material => material.name.ToLower().Contains("holo"))
                    .ToArray(),
                audioSource,
                GlitchEffectSettings
            );

            if (GlitchEffectSettings.IsGlitchAutomatic)
                StartCoroutine(GlitchEffect.PlayGlitchEffectConstantlyWithDelay());
        }

//        void OnTriggerEnter(Collider other)
//        {
//            var gameState = GameManager.Instance.State;
//                
//            if (!other.IsPlayer() || gameState.Status == GameStatus.Lost) return;
//            gameState.SetGameStatus(GameStatus.Lost);
//            FadeInImage.Instance.Fade(FadeDirection.In, .1f);
//            StartCoroutine(GameManager.Instance.Level.LoadLevelWithDelay(LevelName.Game.ToString(), 3f));
//        }
    }
}