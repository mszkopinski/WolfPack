using Zenject;

namespace Wolfpack
{
    public class IocContainer : MonoInstaller<IocContainer>
    {
        public override void InstallBindings()
        {
            // state
            Container.Bind<IGameState>().To<GameState>().AsSingle();
            
            // managers & controllers
            Container.Bind<ILevelManager>().To<LevelManager>().AsSingle();
            Container.Bind<IControllerInput>().To<PlayerInput>().AsSingle();
        }
    }
}