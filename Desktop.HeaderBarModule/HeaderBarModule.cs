using Desktop.HeaderBarModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Desktop.HeaderBarModule
{
    public class HeaderBarModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("HeaderBarRegion", typeof(HeaderBarView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }

}
