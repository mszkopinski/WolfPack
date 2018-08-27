using Zenject;

namespace Wolfpack
{
    public class DefaultInstaller : MonoInstaller<DefaultInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IGameState>().To<GameState>().AsSingle();
            Container.Bind<ILevelManager>().To<LevelManager>().AsSingle();
            Container.Bind<IInputController>().To<InputController>().AsSingle();
        }
    }
}