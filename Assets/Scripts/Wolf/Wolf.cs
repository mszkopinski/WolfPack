using System;
using UnityEngine;

namespace Wolfpack
{
    public sealed class Wolf : MonoBehaviour
    {
        #region Dependency Injection
        readonly ILevelManager levelManager;

        public Wolf(ILevelManager levelManager)
        {
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
            if (!other.IsPlayer() || GameManager.Instance.State == GameState.Lost) return;
            GameManager.Instance.SetGameState(GameState.Lost);
            audioSource.PlayOneShot(wolfGrowlSound);
            FadeInImage.Instance.Fade(FadeDirection.In, .1f);
            StartCoroutine(levelManager.LoadLevelWithDelay(LevelName.Game.ToString(), 3f));
        }
    }
}