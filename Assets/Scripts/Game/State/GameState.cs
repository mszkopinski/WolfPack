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

        public IEnumerator SetGameStatusWithDelay(GameStatus gameStatus, float delayInS)
        {
            yield return new WaitForSeconds(delayInS);
            SetGameStatus(gameStatus);
        }

        protected virtual void OnGameStatusChanged()
        {
            StatusChanged?.Invoke();
        }
    }
}