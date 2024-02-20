using Desktop.Core;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Utility;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;

namespace Desktop.MainBoardModule.ViewModels
{
    /// <summary>
    /// 主面板VM
    /// </summary>
    public class MainBoardViewModel : Desktop.Core.ViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MainBoardViewModel()
        {
            initMianContentControl();
            EventAggregator.GetEvent<NavigateEvent>().Subscribe(navigate);
            EventAggregator.GetEvent<ClosePopupEvent>().Subscribe(closePopup);
        }

        /// <summary>
        /// 导航、页面跳转
        /// </summary>
        /// <param name="message"></param>
        private void navigate(object message)
        {
            //导航
            ViewInfo viewInfo = message as ViewInfo;
            if (Equals(viewInfo, null))
            {
                string msg = "打开页面时错误！请联系管理员！";
                LogHelper.Logger.Error(msg, new NullReferenceException("ViewInfo"));
                (Application.Current.MainWindow as MetroWindow)?.ShowMessageAsync("系统错误", msg);
                return;
            }
            else
            {
                navigateView(viewInfo);
            }
        }

        /// <summary>
        /// 导航到页面
        /// </summary>
        /// <param name="viewInfo">ViewInfo实例</param>
        private void navigateView(ViewInfo viewInfo)
        {
            //树形导航的内容
            var module = viewInfo?.Module as Module;
            object view = null;
            try
            {
                view = System.Reflection.Assembly.Load(module?.AssemblyName)
                           .CreateInstance(module?.ViewName);
                if (Equals(view, null))
                {
                    throw new NullReferenceException("加载模块时错误，页面未找到！");
                }
            }
            catch (Exception ex)
            {
                string msg = $"加载{module?.Name}{module?.AssemblyName}时错误，请确认模块名称和页面名称配置正确！{System.Environment.NewLine}{TypeExtensions.GetPropertiesValue<Module>(module)}{System.Environment.NewLine}{ex.Message}{System.Environment.NewLine}{ex.StackTrace}";
                LogHelper.Logger.Error(msg, ex);
                (Application.Current.MainWindow as MetroWindow).ShowMessageAsync("系统错误", msg);
                return;
            }

            switch (viewInfo.ViewType)
            {
                case ViewType.DockableView://可停靠的页面
                default:
                    #region DockableView
                    try
                    {
                        if (layoutDocumentPane.Children.Any(a => a.ContentId == module?.Id.ToString()))
                        {
                            var tmp = layoutDocumentPane.Children.FirstOrDefault(a => a.ContentId == module?.Id.ToString());
                            tmp.IsActive = true;
                            return;
                        }
                        LayoutDocument layoutDocument = new LayoutDocument();
                        layoutDocument.Title = module?.Name;
                        layoutDocument.ContentId = module?.Id.ToString();
                        layoutDocument.ToolTip = layoutDocument.Title;
                        var icon = Utility.Windows.BitmapImageHelper.GetBitmapImage(module?.Icon, 16, 16);
                        layoutDocument.IconSource = icon;
                        (view as UserControlBase).Margin = (Thickness)Utility.Windows.ResourceHelper.FindResource("UserControlMargin");
                        var viewModelBase = (view as UserControlBase)?.DataContext as ViewModelBase;
                        if (!Equals(viewModelBase, null))
                        {
                            viewModelBase.Parameter.Add(new KeyValuePair<string, object>(ParentName, layoutDocument));
                            if (!Equals(viewInfo.Parameter))
                            {
                                viewModelBase.Parameter.Add(new KeyValuePair<string, object>(DataModelName, viewInfo.Parameter));
                            }
                        }
                        else
                        { 
                            LogHelper.Logger.Error($"{StaticData.CurrentUser?.Name }/{StaticData.CurrentUser?.NickName},打开{module?.Name}模块时，没有正确加载VM.");
                        }

                        layoutDocument.Content = view;
                        layoutDocumentPane.Children.Add(layoutDocument);
                        layoutDocument.IsActive = true;
                        Application.Current.MainWindow.Cursor = Cursors.Arrow;
                        LogHelper.Logger.Info($"{StaticData.CurrentUser?.Name }/{StaticData.CurrentUser?.NickName},打开{module?.Name}模块");
                    }
                    catch (Exception ex)
                    {
                        string msg = $"加载{module?.AssemblyName}时错误，请确认模块名称和页面名称配置正确！{System.Environment.NewLine}{TypeExtensions.GetPropertiesValue<Module>(module)}{System.Environment.NewLine}{ex.Message }{System.Environment.NewLine}{ex.StackTrace}";
                        LogHelper.Logger.Error(msg, ex);
                        (Application.Current.MainWindow as MetroWindow).ShowMessageAsync("系统错误", msg);
                    }
                    #endregion
                    break;
                case ViewType.MetroPopup://Metro蒙版模式对话框
                    #region MetroPopup
                    try
                    {
                        var customDialog = new CustomDialog() { Title = module.Name };
                        var userControlBase = view as UserControlBase;
                        var viewModelBase = userControlBase.DataContext as ViewModelBase;
                        if (!Equals(viewModelBase, null))
                        {
                            viewModelBase.Parameter.Add(new KeyValuePair<string, object>(ParentName, customDialog));
                            if (!Equals(viewInfo.Parameter))
                            {
                                viewModelBase.Parameter.Add(new KeyValuePair<string, object>(DataModelName, viewInfo.Parameter));
                            }
                        }
                        customDialog.Content = view;
                        LogHelper.Logger.Info($"{StaticData.CurrentUser?.Name }/{StaticData.CurrentUser?.NickName},打开{module?.Name}Metro对话框模块");
                        var windows = Application.Current.MainWindow as MahApps.Metro.Controls.MetroWindow;
                        windows?.ShowMetroDialogAsync(customDialog);
                    }
                    catch (Exception ex)
                    {
                        string msg = $"加载{module?.AssemblyName}时错误，请确认模块名称和页面名称配置正确！{System.Environment.NewLine}{TypeExtensions.GetPropertiesValue<Module>(module)}{System.Environment.NewLine}{ex.Message }{System.Environment.NewLine}{ex.StackTrace}";
                        LogHelper.Logger.Error(msg, ex);
                        (Application.Current.MainWindow as MetroWindow).ShowMessageAsync("系统错误", msg);
                    }
                    #endregion
                    break;
                case ViewType.Popup://模式对话框弹出窗体
                    #region Popup
                    try
                    {
                        MahApps.Metro.Controls.MetroWindow popupWindows = new MahApps.Metro.Controls.MetroWindow();
                        //popupWindows.TitlebarHeight = 25;
                        popupWindows.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        popupWindows.Style = Utility.Windows.ResourceHelper.FindResource(@"CleanWindowStyleKey") as Style;
                        popupWindows.WindowTitleBrush = Utility.Windows.ResourceHelper.FindResource(@"PopupWindowsTitleColorBrush") as System.Windows.Media.Brush;
                        popupWindows.NonActiveWindowTitleBrush = Utility.Windows.ResourceHelper.FindResource(@"PopupWindowsTitleColorBrush") as System.Windows.Media.Brush;
                        popupWindows.Background = Utility.Windows.ResourceHelper.FindResource(@"PopupWindowsBackgroundColorBrush") as System.Windows.Media.Brush;
                        popupWindows.GlowBrush = Utility.Windows.ResourceHelper.FindResource(@"PopupWindowsGlowBrush") as System.Windows.Media.Brush;
                        popupWindows.Owner = Application.Current.MainWindow.IsInitialized == false ? null : Application.Current.MainWindow;
                        popupWindows.ResizeMode = ResizeMode.NoResize;
                        popupWindows.IsCloseButtonEnabled = true;
                        popupWindows.ShowCloseButton = true;
                        popupWindows.Width = (view as UserControlBase).Width + popupWindows.BorderThickness.Left + popupWindows.BorderThickness.Right;
                        popupWindows.Height = (view as UserControlBase).Height + popupWindows.TitlebarHeight;
                        popupWindows.Title = module?.Name;
                        //有则使用配置的图标，没有则使用默认的图标，再没有则使用主窗体的图标
                        popupWindows.Icon =
                            Utility.Windows.BitmapImageHelper.GetBitmapImage(module?.Icon)
                            ?? Utility.Windows.BitmapImageHelper.GetBitmapImage(@"pack://application:,,,/Desktop.Resource;component/Images/Logo_128.ico")
                            ?? Application.Current.MainWindow.Icon;
                        popupWindows.ShowInTaskbar = false;
                        var viewModelBase = (view as UserControlBase)?.DataContext as ViewModelBase;
                        if (!Equals(viewModelBase, null))
                        {
                            viewModelBase.Parameter.Add(new KeyValuePair<string, object>(ParentName, popupWindows));
                            if (!Equals(viewInfo.Parameter))
                            {
                                viewModelBase.Parameter.Add(new KeyValuePair<string, object>(DataModelName, viewInfo.Parameter));
                            }
                        }
                        popupWindows.Content = view;
                        popupWindows.Closed += windows_Closed;
                        popupWindows.MouseDown += popupWindows_MouseDown;
                        LogHelper.Logger.Info($"{StaticData.CurrentUser?.Name }/{StaticData.CurrentUser?.NickName},打开{module?.Name}对话框模块");
                        popupWindows.Focus();
                        popupWindows.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        string msg = $"加载{module?.AssemblyName}时错误，请确认模块名称和页面名称配置正确！{System.Environment.NewLine}{TypeExtensions.GetPropertiesValue<Module>(module)}{System.Environment.NewLine}{ex.Message }{System.Environment.NewLine}{ex.StackTrace}";
                        LogHelper.Logger.Error(msg, ex);
                        (Application.Current.MainWindow as MetroWindow).ShowMessageAsync("系统错误", msg);
                    }
                    #endregion
                    break;
                case ViewType.SingleWindow://单独视图
                    #region SingleWindow
                    try
                    {
                        MahApps.Metro.Controls.MetroWindow singleWindows = new MahApps.Metro.Controls.MetroWindow();
                        singleWindows.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        singleWindows.Style = Utility.Windows.ResourceHelper.FindResource(@"CleanWindowStyleKey") as Style;
                        singleWindows.WindowTitleBrush = Utility.Windows.ResourceHelper.FindResource(@"PopupWindowsTitleColorBrush") as System.Windows.Media.Brush;
                        singleWindows.NonActiveWindowTitleBrush = Utility.Windows.ResourceHelper.FindResource(@"PopupWindowsTitleColorBrush") as System.Windows.Media.Brush;
                        singleWindows.Background = Utility.Windows.ResourceHelper.FindResource(@"PopupWindowsBackgroundColorBrush") as System.Windows.Media.Brush;
                        singleWindows.GlowBrush = Utility.Windows.ResourceHelper.FindResource(@"AccentColorBrush") as System.Windows.Media.Brush;
                        //singleWindows.Owner = Application.Current.MainWindow.IsInitialized == false ? null : Application.Current.MainWindow;
                        singleWindows.IsCloseButtonEnabled = true;
                        singleWindows.ShowCloseButton = true;
                        singleWindows.Width = (view as UserControlBase).Width + singleWindows.BorderThickness.Left + singleWindows.BorderThickness.Right;
                        singleWindows.Height = (view as UserControlBase).Height + singleWindows.TitlebarHeight;
                        singleWindows.Title = module?.Name;
                        //有则使用配置的图标，没有则使用默认的图标，再没有则使用主窗体的图标
                        singleWindows.Icon =
                            Utility.Windows.BitmapImageHelper.GetBitmapImage(module?.Icon)
                            ?? Utility.Windows.BitmapImageHelper.GetBitmapImage(@"pack://application:,,,/Desktop.Resource;component/Images/Logo_128.ico")
                            ?? Application.Current.MainWindow.Icon;
                        var viewModelBase = (view as UserControlBase)?.DataContext as ViewModelBase;
                        if (!Equals(viewModelBase, null))
                        {
                            viewModelBase.Parameter.Add(new KeyValuePair<string, object>(ParentName, singleWindows));
                            if (!Equals(viewInfo.Parameter))
                            {
                                viewModelBase.Parameter.Add(new KeyValuePair<string, object>(DataModelName, viewInfo.Parameter));
                            }
                        }
                        singleWindows.ShowInTaskbar = true;
                        singleWindows.Content = view;
                        singleWindows.Closed += windows_Closed;
                        singleWindows.MouseDown += popupWindows_MouseDown;
                        singleWindows.Focus();
                        singleWindows.Show();
                    }
                    catch (Exception ex)
                    {
                        string msg = $"加载{module?.AssemblyName}时错误，请确认模块名称和页面名称配置正确！{System.Environment.NewLine}{TypeExtensions.GetPropertiesValue<Module>(module)}{System.Environment.NewLine}{ex.Message }{System.Environment.NewLine}{ex.StackTrace}";
                        LogHelper.Logger.Error(msg, ex);
                        (Application.Current.MainWindow as MetroWindow).ShowMessageAsync("系统错误", msg);
                    }
                    #endregion
                    break;
            }
        }

        /// <summary>
        /// 子窗体关闭事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windows_Closed(object sender, EventArgs e)
        {
            var windows = (sender as MetroWindow);
            (windows?.Content as IDisposable)?.Dispose();
            windows.MouseDown -= popupWindows_MouseDown;
            LogHelper.Logger.Info($"{StaticData.CurrentUser?.Name }/{StaticData.CurrentUser?.NickName},通过窗体按钮关闭{windows?.Title}模块,");
            windows = null;
            GC.Collect();
        }

        /// <summary>
        /// 关闭对话框
        /// </summary>
        /// <param name="obj"></param>
        private void closePopup(object obj)
        {
            if (obj is BaseMetroDialog)
            {
                var baseMetroDialog = obj as BaseMetroDialog;
                var window = Application.Current.MainWindow as MahApps.Metro.Controls.MetroWindow;
                LogHelper.Logger.Info($"{StaticData.CurrentUser?.Name }/{StaticData.CurrentUser?.NickName},关闭{baseMetroDialog?.Title}Metro对话框模块");
                window?.HideMetroDialogAsync(baseMetroDialog);
            }
            if (obj is MetroWindow)
            {
                var popupWindow = obj as MetroWindow;
                if (!Equals(popupWindow, null))
                {
                    LogHelper.Logger.Info($"{StaticData.CurrentUser?.Name }/{StaticData.CurrentUser?.NickName},关闭{popupWindow?.Title}对话框模块");
                    popupWindow.MouseDown -= popupWindows_MouseDown;
                    popupWindow.Close();
                    popupWindow.Closed -= windows_Closed;
                    popupWindow = null;
                    GC.Collect();
                }
            }
        }

        /// <summary>
        /// 对话框 鼠标按下拖动窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popupWindows_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var v = sender as MetroWindow;
            v?.DragMove();
        }

        #region 主显示区域 包含树形菜单控件,定义和初始化菜单，菜单点击处理等

        #region  主显示区域控件
        private DockingManager mianContentControl;

        /// <summary>
        /// 主显示区域控件
        /// </summary>
        public DockingManager MianContentControl
        {
            get { return mianContentControl; }
            set { SetProperty(ref mianContentControl, value); }
        }

        #endregion

        /// <summary>
        /// 初始化主要内容的容器控件
        /// </summary>
        private void initMianContentControl()
        {
            mianContentControl = new DockingManager();
            mianContentControl.BorderBrush = Utility.Windows.ResourceHelper.FindResource(@"AccentColorBrush") as System.Windows.Media.Brush;
            mianContentControl.Theme = new Xceed.Wpf.AvalonDock.Themes.MetroTheme();
            mianContentControl.DocumentClosed += MianContentControl_DocumentClosed;
            var v = Utility.Windows.ResourceHelper.FindResource(@"DockDocumentHeaderTemplate");
            DataTemplate dataTemplate = v as DataTemplate;

            mianContentControl.DocumentHeaderTemplate = dataTemplate;
            LayoutRoot layoutRoot = new LayoutRoot();
            mianContentControl.Layout = layoutRoot;

            LayoutAnchorSide layoutAnchorSide = new LayoutAnchorSide();

            layoutRoot.LeftSide = layoutAnchorSide;
            LayoutAnchorGroup layoutAnchorGroup = new LayoutAnchorGroup();

            layoutAnchorSide.Children.Add(layoutAnchorGroup);
            LayoutAnchorable layoutAnchorable = new LayoutAnchorable();
            layoutAnchorable.Title = Utility.Windows.ResourceHelper.FindResource(@"NavigationMenu").ToString();
            layoutAnchorable.IconSource = Utility.Windows.BitmapImageHelper.GetBitmapImage("Images/Folder_32.png", 16, 16);

            layoutAnchorable.CanAutoHide = true;
            layoutAnchorable.CanClose = false;
            layoutAnchorable.CanFloat = false;
            layoutAnchorable.CanHide = false;
            layoutAnchorable.IsMaximized = false;
            layoutAnchorable.AutoHideMinWidth = (double)Utility.Windows.ResourceHelper.FindResource(@"LeftTreeAutoHideMinWidth");

            layoutAnchorGroup.Children.Add(layoutAnchorable);
            layoutAnchorable.ToggleAutoHide();
            //TreeView treeView = new TreeView();
            //layoutAnchorable.Content = new ContentControl ()(Prism.Regions.RegionManager.SetRegionName ("");
            //Prism.Regions.RegionManager.GetRegionName(layoutAnchorable, "TreeMenuModule");
            //treeView.ItemsSource = this.MenuItems;
            //layoutAnchorable.Content =
            //Style style = new Style(); ;
            //style.TargetType = typeof(TreeViewItem);
            //style.BasedOn = Utility.Windows.ResourceHelper.FindResource(@"MetroTreeViewItem") as Style;
            //treeView.ItemContainerStyle = style;
            var treeMenuContent = new ContentControl();
            layoutAnchorable.Content = treeMenuContent;

            Prism.Regions.RegionManager.SetRegionName(treeMenuContent, "TreeMenuRegion");
            //Prism.Regions.RegionManager.SetRegionName(layoutAnchorable, "TreeMenuModule");
            //var regionManager = containerProvider.Resolve<IRegionManager>();
            //regionManager.RegisterViewWithRegion("ShellContentRegion", typeof(ShellView));

            LayoutDocumentPaneGroup layoutDocumentPaneGroup = new LayoutDocumentPaneGroup();
            layoutRoot.RootPanel.Orientation = Orientation.Horizontal;
            layoutRoot.RootPanel.Children[1] = layoutDocumentPaneGroup;
            layoutDocumentPaneGroup.InsertChildAt(0, layoutDocumentPane);

        }

        /// <summary>
        /// tab页面关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MianContentControl_DocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            LogHelper.Logger.Info($"用户{StaticData.CurrentUser?.Name }/{StaticData.CurrentUser?.NickName},关闭{e?.Document?.Title}模块");
            (e.Document?.Content as UserControlBase)?.Dispose();
        }

        /// <summary>
        /// 主文档tab页面容器,所有tab页面都由此对象管理
        /// </summary>
        private LayoutDocumentPane layoutDocumentPane = new LayoutDocumentPane();

        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Disposing()
        {
            //释放相关的资源
            UiMessage = null;
            layoutDocumentPane?.Children?.Clear();
            this.MianContentControl = null;
            LogHelper.Logger.Debug($"释放资源：{this.ToString()}");
        }
    }

}
