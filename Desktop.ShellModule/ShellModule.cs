using Desktop.ShellModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
namespace Desktop.ShellModule
{
    public class ShellModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ShellContentRegion", typeof(ShellView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }

}
