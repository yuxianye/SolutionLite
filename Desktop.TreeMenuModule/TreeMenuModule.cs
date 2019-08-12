using Desktop.TreeMenuModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Desktop.TreeMenuModule
{
    public class TreeMenuModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("TreeMenuRegion", typeof(TreeMenuView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }

}
