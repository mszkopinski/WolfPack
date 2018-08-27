using Zenject;

namespace Wolfpack
{
    public class IocContainer : MonoInstaller<IocContainer>
    {
        public override void InstallBindings()
        {
            Container.Bind<ILevelManager>().To<LevelManager>().AsSingle();
        }
    }
}