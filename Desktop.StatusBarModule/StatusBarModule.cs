using Desktop.StatusBarModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Desktop.StatusBarModule
{
    public class StatusBarModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("StatusBarRegion", typeof(StatusBarView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }

}
