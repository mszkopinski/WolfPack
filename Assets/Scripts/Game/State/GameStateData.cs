namespace Wolfpack
{
    public struct GameStateData
    {
        public GameStatus Status;

        public static GameStateData Default => new GameStateData {Status = GameStatus.Unknown};
    }
}