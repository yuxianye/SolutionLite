using Desktop.ThreeDModule.ViewModels;
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Model.Scene;
using System.Windows;

namespace Desktop.ThreeDModule.Views
{
    /// <summary>
    /// 用户管理页面
    /// </summary>
    public partial class ThreeDx2View
    {
        public ThreeDx2View()
        {
            InitializeComponent();
            view.AddHandler(Element3D.MouseDown3DEvent, new RoutedEventHandler((s, e) =>
            {
                var arg = e as MouseDown3DEventArgs;

                if (arg.HitTestResult == null)
                {
                    return;
                }
                if (arg.HitTestResult.ModelHit is SceneNode node && node.Tag is AttachedNodeViewModel vm)
                {
                    vm.Selected = !vm.Selected;
                }
            }));
        }

        //private void BatchedMeshGeometryModel3D_Mouse3DDown(object sender, HelixToolkit.Wpf.SharpDX.MouseDown3DEventArgs e)
        //{

        //}
    }
}
