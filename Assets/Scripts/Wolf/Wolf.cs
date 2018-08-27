using System;
using UnityEngine;

namespace Wolfpack
{
    public sealed class Wolf : MonoBehaviour
    {
        #region Dependency Injection

        readonly IGameState gameState;
        readonly ILevelManager levelManager;

        public Wolf(IGameState gameState, ILevelManager levelManager)
        {
            this.gameState = gameState;
            this.levelManager = levelManager;
        }
        #endregion
        
        [SerializeField] AudioClip wolfGrowlSound;
        
        AudioSource audioSource;

        public static Action<Transform> WolfAppeared;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        void Start()
        {
            OnWolfAppeared();
        }

        void OnWolfAppeared()
        {
            WolfAppeared?.Invoke(transform);
        }

        void OnTriggerEnter(Collider other)
        {
            if (!other.IsPlayer() || gameState.Value.Status == GameStatus.Lost) return;
            gameState.SetGameStatus(GameStatus.Lost);
            audioSource.PlayOneShot(wolfGrowlSound);
            FadeInImage.Instance.Fade(FadeDirection.In, .1f);
            StartCoroutine(levelManager.LoadLevelWithDelay(LevelName.Game.ToString(), 3f));
        }
    }
}