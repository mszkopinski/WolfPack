using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wolfpack
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        public static event Action<LevelName> LevelChanged;
        public static string CurrentLevelName => SceneManager.GetActiveScene().name;

        void Awake()
        {
            SceneManager.sceneLoaded += (arg0, mode) =>
            {
                var levelName = arg0.name.ToLower().Contains("game")
                    ? LevelName.Game
                    : arg0.name.ToLower().Contains("menu")
                        ? LevelName.Menu
                        : LevelName.Unknown;

                LevelChanged?.Invoke(levelName);
            };
        }

        void OnEnable()
        {
            DontDestroyOnLoad(this);
        }
    
        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        public void Load(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }

        public void LoadWithDelay(string levelName, float delayInS)
        {
            StartCoroutine(LoadWithDelayCoroutine(levelName, delayInS));
        }

        IEnumerator LoadWithDelayCoroutine(string levelName, float delayInS)
        {
            yield return new WaitForSeconds(delayInS);
            SceneManager.LoadScene(levelName);
        }
    }
}
