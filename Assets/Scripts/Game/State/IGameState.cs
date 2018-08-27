using System;
using System.Collections;

namespace Wolfpack
{
    public interface IGameState
    {
        GameStatus Status { get; }
        event Action StatusChanged;
        void SetGameStatus(GameStatus gameStatus);
        IEnumerator SetGameStatusWithDelay(GameStatus gameStatus, float delayInS);
    }
}