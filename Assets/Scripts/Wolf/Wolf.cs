using System;
using UnityEngine;

namespace Wolfpack
{
    public sealed class Wolf : MonoBehaviour
    {
        [SerializeField] AudioClip wolfGrowlSound;
    
        public static Action<Transform> WolfAppeared;

        AudioSource audioSource;
    
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

        void OnTriggerEnter(Collider collider)
        {
            if (collider.IsPlayer() && GameManager.Instance.CurrentState != GameState.Lost)
            {
                GameManager.Instance.SetGameState(GameState.Lost);
                audioSource.PlayOneShot(wolfGrowlSound);
                FadeInImage.Instance.Fade(FadeDirection.In, .1f);
                LevelManager.Instance.LoadWithDelay(LevelName.Game.ToString(), 3f);
            }
        }
    }
}