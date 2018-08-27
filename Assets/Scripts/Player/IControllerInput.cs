namespace Wolfpack
{
    public interface IControllerInput
    {
        float HorizontalAxis { get; }
        float VerticalAxis { get; }
        float MouseHorizontalAxis { get; }
        float MouseVerticalAxis { get; }
        void OnUpdate();
    }
}