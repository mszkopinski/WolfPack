using System;
using System.Collections;

namespace Wolfpack
{
    public interface IGameState
    {
        GameStateData Value { get; }
        event Action StateChanged;
        void SetGameStatus(GameStatus gameStatus);
        IEnumerator SetGameStatusWithDelay(GameStatus gameStatus, float delayInS);
    }
}