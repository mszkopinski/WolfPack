using UnityEngine;
using Zenject;

namespace Wolfpack
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public IGameState State { get; private set; }
        public ILevelManager Level { get; private set; }
        public IInputController Input { get; private set; }

        [Inject]
        public void Initialize(IGameState gameState, ILevelManager levelManager, IInputController inputController)
        {
            State = gameState;
            Level = levelManager;
            Input = inputController;
        }
        
        void OnEnable()
        {
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            FadeInImage.Instance.Fade(FadeDirection.Out, 10f);
            StartCoroutine(State.SetGameStatusWithDelay(GameStatus.Intro, .1f));
            Level.LevelChanged += OnLevelChanged;
        }

        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.K) && Level.CurrentLevelName != "Game")
                Level.LoadLevel("Game");
        }

        public void LoadGame()
        {
            FadeInImage.Instance.Fade(FadeDirection.In, 1f);
            StartCoroutine(Level.LoadLevelWithDelay("Game", 3f));
        }

        void OnLevelChanged(string levelName)
        {
            if (levelName == "Game")
            {
                State.SetGameStatus(GameStatus.Game);
                FadeInImage.Instance.Fade(FadeDirection.Out, 5f);
            }
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
