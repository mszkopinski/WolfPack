using System;
using System.Collections;

namespace Wolfpack
{
    public interface ILevelManager
    {
        string CurrentLevelName { get; }
        void LoadLevel(string levelName);
        IEnumerator LoadLevelAsync(string levelName);
        void Quit();
        event Action<string> LevelChanged;
    }
}