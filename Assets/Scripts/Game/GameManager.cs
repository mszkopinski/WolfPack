using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Audio;
using WolfPack;
using Zenject;

namespace Wolfpack
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [Header("Player"), SerializeField] 
        GameObject playerPrefab;

        [Header("Audio")]
        public AudioMixer AudioMixer;
        public AudioSource AudioSource;
        public AudioClip MenuTrack;
        public AudioClip GameTrack;
        
        public IGameState State { get; private set; }
        public ILevelManager Level { get; private set; }
        public IInputController Input { get; private set; }
        
        public readonly ObservableCollection<Wolf> Wolves = new ObservableCollection<Wolf>();
        public Action WolfNumberChanged;

        [Inject]
        public void Initialize(IGameState gameState, ILevelManager levelManager, IInputController inputController)
        {
            State = gameState;
            Level = levelManager;
            Input = inputController;
        }

        float currentVolume;

        void OnEnable()
        {
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            Level.LevelChanged += OnLevelChanged;
            State.SetGameStatusAsync(GameStatus.Intro).RunWithDelay(.1f); // there was a slightly problem when running with .1f delay. Be aware
            AudioSource.ChangeClip(MenuTrack).Run();
            ScreenFading.Instance.FadeInOut(FadeDirection.Out, 3f).Run();
            Wolves.CollectionChanged += (sender, args) => { WolfNumberChanged?.Invoke(); };
            Wolf.Died += wolf =>
            {
                if (wolf.CompareTag("Player"))
                {
                    RestartGame();
                }
            };
        }

        void RestartGame()
        {
            Log.Console("Should restart game.");
            Level.LoadLevelAsync("Game").RunWithDelay(2f, () => SetGameScene());
        }

        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.P) && Level.CurrentLevelName != "Game")
                LoadGame();
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.K) && Level.CurrentLevelName != "Game")
                Level.LoadLevel("Game");
        }

        public void LoadGame()
        {
            Level.LoadLevelAsync("Game").RunWithDelay(10f);
            
            AudioMixer.ChangeFloatOverTime("musicPitch", 0.4f, 3f).Run();
            AudioMixer.ChangeVolumeOverTime("sfxVolume", 0.0f, 3f).Run();
            AudioMixer.ChangeVolumeOverTime("musicVolume", 0.0f, 1f).RunWithDelay(3f);

            AudioSource.ChangeClip(GameTrack, () => AudioSource.time = 15f).RunWithDelay(4f);
            AudioMixer.ChangeVolumeOverTime("musicVolume", 1f, 4f).RunWithDelay(6f);

            ScreenFading.Instance.FadeInOut(FadeDirection.In, 1f).Run();
        }

        void OnLevelChanged(string levelName)
        {
            Wolves.Clear();
            if (levelName != "Game" || State.Status == GameStatus.Game)
                return;

            State.SetGameStatus(GameStatus.Game);
            SetGameScene();
            AudioMixer.ChangeVolumeOverTime("sfxVolume", 1f, 3f).Run();
            AudioMixer.ChangeFloatOverTime("musicPitch", 1f, 3f).Run();
        }

        void SetGameScene()
        {
            // additional delay for Zenject dependency injection process to do everything work.
            SpawnPlayer().RunWithDelay(.001f, ObstacleFormationsSpawner.Instance.Spawn); 
            WolfSpawner.Instance.StartSpawning().RunWithDelay(.3f);
        }

        IEnumerator SpawnPlayer()
        {
            FindObjectOfType<Camera>().enabled = false; 
            Instantiate(playerPrefab);
            yield return null;
        }

        void OnGUI()
        {
#if UNITY_EDITOR
            GUI.Label(new Rect(0, 0, 250, 25), $"State: {State.Status.ToString()}");     
            GUI.Label(new Rect(0, 25, 250, 25), $"GameTime: {Mathf.RoundToInt(Time.timeSinceLevelLoad)}");
            GUI.Label(new Rect(0, 50, 250, 25), $"Level: {Level.CurrentLevelName}");
            GUI.Label(new Rect(250, 0, 250, 25), $"Wolves: {Wolves.Count}");
#endif
        }
    }
}
