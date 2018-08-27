using UnityEngine;

namespace Wolfpack
{
    public class GameManager : MonoSingleton<GameManager>
    {
        #region Dependency Injection
        readonly IGameState gameState;
        readonly ILevelManager levelManager;

        public GameManager(IGameState gameState, ILevelManager levelManager)
        {
            this.gameState = gameState;
            this.levelManager = levelManager;
        }
        #endregion
        
        void OnEnable()
        {
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            FadeInImage.Instance.Fade(FadeDirection.Out, 10f);
            StartCoroutine(gameState.SetGameStatusWithDelay(GameStatus.Intro, .1f));
            levelManager.LevelChanged += LevelManagerOnLevelChanged;
        }

        void LevelManagerOnLevelChanged(LevelName levelName)
        {
            if (levelName != LevelName.Game) return;
            FadeInImage.Instance.Fade(FadeDirection.Out, 5f);
            gameState.SetGameStatus(GameStatus.Game);
        }

        void OnGUI()
        {
#if UNITY_EDITOR
            GUI.Label(new Rect(0, 0, 250, 25), $"State: {gameState.Value.Status.ToString()}");     
            GUI.Label(new Rect(0, 25, 250, 25), $"GameTime: {Mathf.RoundToInt(Time.timeSinceLevelLoad)}");
            GUI.Label(new Rect(0, 50, 250, 25), $"Level: {levelManager.CurrentLevelName}");
#endif
        }
    }
}
