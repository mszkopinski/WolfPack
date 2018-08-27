using UnityEngine;
using Zenject;

namespace Wolfpack
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public IGameState State { get; private set; }
        public ILevelManager Level { get; private set; }

        [Inject]
        public void Initialize(IGameState gameState, ILevelManager levelManager)
        {
            State = gameState;
            Level = levelManager;
        }
        
        void OnEnable()
        {
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            FadeInImage.Instance.Fade(FadeDirection.Out, 10f);
            StartCoroutine(State.SetGameStatusWithDelay(GameStatus.Intro, .1f));
            Level.LevelChanged += LevelManagerOnLevelChanged;
        }

        void LevelManagerOnLevelChanged(LevelName levelName)
        {
            if (levelName != LevelName.Game) return;
            FadeInImage.Instance.Fade(FadeDirection.Out, 5f);
            State.SetGameStatus(GameStatus.Game);
        }

        void OnGUI()
        {
#if UNITY_EDITOR
            GUI.Label(new Rect(0, 0, 250, 25), $"State: {State.Status.ToString()}");     
            GUI.Label(new Rect(0, 25, 250, 25), $"GameTime: {Mathf.RoundToInt(Time.timeSinceLevelLoad)}");
            GUI.Label(new Rect(0, 50, 250, 25), $"Level: {Level.CurrentLevelName}");
#endif
        }
    }
}
