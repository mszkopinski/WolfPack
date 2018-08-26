using System;
using System.Collections;
using UnityEngine;

namespace Wolfpack
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public static event Action<GameState> GameStateChanged;
        public GameState CurrentState { get; private set; }

        void OnEnable()
        {
            SetGameState(GameState.Unknown);
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            FadeInImage.Instance.Fade(FadeDirection.Out, 10f);
            StartCoroutine(SetGameStateWithDelay(GameState.Intro, .1f));
            LevelManager.LevelChanged += LevelManagerOnLevelChanged;
        }

        void LevelManagerOnLevelChanged(LevelName levelName)
        {
            if (levelName == LevelName.Game)
            {
                FadeInImage.Instance.Fade(FadeDirection.Out, 5f);
                SetGameState(GameState.Game);
            }
        }

        public void SetGameState(GameState gameState)
        {
            if (gameState == CurrentState)
                return;
            CurrentState = gameState;
            OnGameStateChanged(CurrentState);
        }

        IEnumerator SetGameStateWithDelay(GameState gameState, float delay)
        {
            yield return new WaitForSeconds(delay);
            SetGameState(gameState);
        }
    
        protected virtual void OnGameStateChanged(GameState gameState)
        {
            GameStateChanged?.Invoke(CurrentState);   
        }

        void OnGUI()
        {
#if UNITY_EDITOR
            GUI.Label(new Rect(0, 0, 250, 25), $"State: {CurrentState.ToString()}");     
            GUI.Label(new Rect(0, 25, 250, 25), $"GameTime: {Mathf.RoundToInt(Time.timeSinceLevelLoad)}");
            GUI.Label(new Rect(0, 50, 250, 25), $"Level: {LevelManager.CurrentLevelName}");
#endif
        }
    }
}
