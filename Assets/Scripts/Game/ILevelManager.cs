using System;
using System.Collections;

namespace Wolfpack
{
    public interface ILevelManager
    {
        string CurrentLevelName { get; }
        void LoadLevel(string levelName);
        IEnumerator LoadLevelWithDelay(string levelName, float delay);
        void Quit();
        event Action<string> LevelChanged;
    }
}