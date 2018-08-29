using System;
using System.Collections;
using UnityEngine;

namespace Wolfpack
{   
    public class GameState : IGameState
    {
        public GameStatus Status { get; private set; }
        public event Action StatusChanged;

        public GameState()
        {
            Status = GameStatus.Unknown;
        }

        public void SetGameStatus(GameStatus gameStatus)
        {
            if (Status == gameStatus) return;
            Status = gameStatus;
            OnGameStatusChanged();
        }

        public IEnumerator SetGameStatusAsync(GameStatus gameStatus)
        {
            yield return null;
            SetGameStatus(gameStatus);
        }

        protected virtual void OnGameStatusChanged()
        {
            StatusChanged?.Invoke();
        }
    }
}