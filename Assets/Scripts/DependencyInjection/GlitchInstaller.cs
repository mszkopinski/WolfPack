using Zenject;

namespace Wolfpack
{
    public class GlitchInstaller : MonoInstaller<GlitchInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IGlitchEffect>().To<GlitchEffect>().AsTransient();
        }
    }
}