using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wolfpack
{
    public class LevelManager : ILevelManager
    {
        public string CurrentLevelName => SceneManager.GetActiveScene().name;
        public event Action<LevelName> LevelChanged;

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

        public void LoadLevel(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }

        public IEnumerator LoadLevelWithDelay(string levelName, float delayInS)
        {
            yield return new WaitForSeconds(delayInS);
            SceneManager.LoadScene(levelName);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
