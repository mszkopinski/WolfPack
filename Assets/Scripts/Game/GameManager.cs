using System;
using System.Collections;
using UnityEngine;

namespace Wolfpack
{
    public class GameManager : MonoSingleton<GameManager>
    {
        #region Dependency Injection
        readonly ILevelManager levelManager;

        public GameManager(ILevelManager levelManager)
        {
            this.levelManager = levelManager;
        }
        #endregion
        
        public static event Action<GameState> StateChanged;
        public GameState State { get; private set; }
   
        void OnEnable()
        {
            SetGameState(GameState.Unknown);
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            FadeInImage.Instance.Fade(FadeDirection.Out, 10f);
            StartCoroutine(SetGameStateWithDelay(GameState.Intro, .1f));
            levelManager.LevelChanged += LevelManagerOnLevelChanged;
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
            if (gameState == State)
                return;
            State = gameState;
            OnGameStateChanged(State);
        }

        IEnumerator SetGameStateWithDelay(GameState gameState, float delay)
        {
            yield return new WaitForSeconds(delay);
            SetGameState(gameState);
        }
    
        protected virtual void OnGameStateChanged(GameState gameState)
        {
            StateChanged?.Invoke(State);   
        }

        void OnGUI()
        {
#if UNITY_EDITOR
            GUI.Label(new Rect(0, 0, 250, 25), $"State: {State.ToString()}");     
            GUI.Label(new Rect(0, 25, 250, 25), $"GameTime: {Mathf.RoundToInt(Time.timeSinceLevelLoad)}");
            GUI.Label(new Rect(0, 50, 250, 25), $"Level: {levelManager.CurrentLevelName}");
#endif
        }
    }
}
