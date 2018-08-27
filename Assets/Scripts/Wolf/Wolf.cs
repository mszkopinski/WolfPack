using System;
using UnityEngine;
using Zenject;

namespace Wolfpack
{
    public sealed class Wolf : MonoBehaviour
    {
        public static Action<Transform> WolfAppeared;

        [SerializeField] 
        AudioClip wolfGrowlSound;
        AudioSource audioSource;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        void Start()
        {
            WolfAppeared?.Invoke(transform);
        }

        void OnTriggerEnter(Collider other)
        {
            var gameState = GameManager.Instance.State;
                
            if (!other.IsPlayer() || gameState.Status == GameStatus.Lost) return;
            gameState.SetGameStatus(GameStatus.Lost);
            audioSource.PlayOneShot(wolfGrowlSound);
            FadeInImage.Instance.Fade(FadeDirection.In, .1f);
            StartCoroutine(GameManager.Instance.Level.LoadLevelWithDelay(LevelName.Game.ToString(), 3f));
        }
    }
}