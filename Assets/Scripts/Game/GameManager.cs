using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Wolfpack
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [Header("Audio")]
        public AudioMixer AudioMixer;
        public AudioSource AudioSource;
        public AudioClip MenuTrack;
        public AudioClip GameTrack;
        
        public IGameState State { get; private set; }
        public ILevelManager Level { get; private set; }
        public IInputController Input { get; private set; }

        float currentVolume;
        
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
            AudioSource.ChangeClip(MenuTrack);
            ScreenFading.Instance.Fade(FadeDirection.Out, 10f);
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
            StartCoroutine(Level.LoadLevelWithDelay("Game", 10f));
            SuppressVolumeAndChangeMusicPitch();
            ChangeTrackAndIncreaseVolume();
            ScreenFading.Instance.Fade(FadeDirection.In, 1f);
        }

        void SuppressVolumeAndChangeMusicPitch()
        {
            StartCoroutine(AudioMixer.ChangeFloatOverTime("musicPitch", 0.4f, 3f));
            StartCoroutine(AudioMixer.ChangeVolumeOverTime("sfxVolume", 0.0f, 3f));
            StartCoroutine(AudioMixer.ChangeVolumeOverTimeWithDelay("musicVolume", 0.0f, 1f, 5f));
        }

        void ChangeTrackAndIncreaseVolume()
        {
            StartCoroutine(AudioSource.ChangeClipWithDelay(GameTrack, 6f, () => AudioSource.time = 15f));
            StartCoroutine(AudioMixer.ChangeVolumeOverTimeWithDelay("musicVolume", 1f, 4f, 6f));
        }

        void OnLevelChanged(string levelName)
        {
            if (levelName == "Game")
            {
                State.SetGameStatus(GameStatus.Game);

                StartCoroutine(AudioMixer.ChangeVolumeOverTime("sfxVolume", 1f, 3f));
                StartCoroutine(AudioMixer.ChangeFloatOverTime("musicPitch", 1f, 3f));
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
