using Desktop.MainBoardModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Desktop.MainBoardModule
{
    public class MainBoardModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("MainBoardRegion", typeof(MainBoardView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }

}
