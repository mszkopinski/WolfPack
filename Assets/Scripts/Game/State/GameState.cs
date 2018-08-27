using System;
using System.Collections;
using UnityEngine;

namespace Wolfpack
{   
    public class GameState : IGameState
    {
        public GameStateData Value { get; private set; }
        public event Action StateChanged;

        public GameState(GameStateData stateData)
        {
            Value = GameStateData.Default;
        }

        public void SetGameStatus(GameStatus gameStatus)
        {
            var newState = Value;
            newState.Status = gameStatus;
            Value = newState;
            OnGameStateChanged();
        }

        public IEnumerator SetGameStatusWithDelay(GameStatus gameStatus, float delayInS)
        {
            yield return new WaitForSeconds(delayInS);
            SetGameStatus(gameStatus);
        }

        protected virtual void OnGameStateChanged()
        {
            StateChanged?.Invoke();
        }
    }
}