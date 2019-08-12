using Desktop.HeaderMenuModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Desktop.HeaderMenuModule
{
    public class HeaderMenuModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("HeaderMenuRegion", typeof(HeaderMenuView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }

}
