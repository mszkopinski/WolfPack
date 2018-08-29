using UnityEngine;
using UnityEngine.Audio;
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

        [Header("Audio")]
        public AudioMixer AudioMixer;
        public AudioSource AudioSource;
        public AudioClip MenuTrack;
        public AudioClip GameTrack;
      
        float currentVolume;

        void OnEnable()
        {
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            Level.LevelChanged += OnLevelChanged;
            
            State.SetGameStatusAsync(GameStatus.Intro).RunWithDelay(1f); // there was a slightly problem when running with .1f delay. Be aware
            AudioSource.ChangeClip(MenuTrack).Run();
            ScreenFading.Instance.Fade(FadeDirection.Out, 10f);
        }

        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.K) && Level.CurrentLevelName != "Game")
                Level.LoadLevel("Game");
        }

        public void LoadGame()
        {
            Level.LoadLevelAsync("Game").RunWithDelay(10f);
            
            AudioMixer.ChangeFloatOverTime("musicPitch", 0.4f, 3f).Run();
            AudioMixer.ChangeVolumeOverTime("sfxVolume", 0.0f, 3f).Run();
            AudioMixer.ChangeVolumeOverTime("musicVolume", 0.0f, 1f).Run();

            AudioSource.ChangeClip(GameTrack, () => AudioSource.time = 15f).Run();
            AudioMixer.ChangeVolumeOverTime("musicVolume", 1f, 4f).Run();

            ScreenFading.Instance.Fade(FadeDirection.In, 1f);
        }

        void OnLevelChanged(string levelName)
        {
            if (levelName != "Game")
                return;

            State.SetGameStatus(GameStatus.Game);
            StartCoroutine(AudioMixer.ChangeVolumeOverTime("sfxVolume", 1f, 3f));
            StartCoroutine(AudioMixer.ChangeFloatOverTime("musicPitch", 1f, 3f));
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
