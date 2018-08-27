namespace Wolfpack
{
    public interface IInputController
    {
        void OnUpdate();
        float Horizontal { get; }
        bool OnHorizontalDown { get; }
        float Vertical { get; }
        bool OnVerticalDown { get; }
        float MouseX { get; }
        float MouseY { get; }
    }
}