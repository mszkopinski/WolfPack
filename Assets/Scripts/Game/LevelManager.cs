using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wolfpack
{
    public class LevelManager : ILevelManager
    {
        public event Action<string> LevelChanged;
        public string CurrentLevelName => SceneManager.GetActiveScene().name;

        public LevelManager()
        {
            SceneManager.sceneLoaded += (scene, mode) 
                => LevelChanged?.Invoke(scene.name);
        }

        public void LoadLevel(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }

        public IEnumerator LoadLevelAsync(string levelName)
        {
            yield return null;
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
