using Desktop.Core;
using Models;
using Prism.Commands;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Utility.Windows;

namespace Desktop.TreeMenuModule.ViewModels
{
    /// <summary>
    /// 左边导航树VM
    /// </summary>
    public class TreeMenuViewModel : Desktop.Core.ViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TreeMenuViewModel()
        {
            NavigateCommand = new DelegateCommand<TreeViewItem>(Navigate);
            RefreshCommand = new DelegateCommand(() => initMenuItems());
            initMenuItems();

            //右键快捷菜单
            ContextMenus = new List<MenuItem>()
            {
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Desktop.Resource;component/Images/Refresh_32.png") ,
                    Header =ResourceHelper.FindResource ("Refresh"),Command =RefreshCommand},
            };
        }

        /// <summary>
        /// 导航命令
        /// </summary>
        public DelegateCommand<TreeViewItem> NavigateCommand { get; private set; }

        /// <summary>
        /// 刷新命令
        /// </summary>
        public ICommand RefreshCommand { get; private set; }

        /// <summary>
        /// 数据访问上下文
        /// </summary>
        private Dal.DbContext dbContext = new Dal.DbContext();

        /// <summary>
        /// 初始化菜单数据，debug模式使用自动生成的测试数据
        /// </summary>
        private void initMenuItems()
        {
            var userid = StaticData.CurrentUser.Id;
            StaticData.Module.Clear();
            foreach (var userRole in dbContext.UserRoleDb.GetList(a => a.UserId == userid))
            {
                var roleId = userRole.RoleId;
                var tmp = dbContext.Db.Queryable<Module, RoleModule>((module, roleModule) => new object[]
                { JoinType.Left, module.Id == roleModule.ModuleId })
                .Where((module, roleModule) => roleModule.RoleId == roleId);
                StaticData.Module.AddRange(tmp.ToList());
            }
            var tmpSolutionTreeViewItems = new List<TreeViewItem>();
            tmpSolutionTreeViewItems = GetTreeViewItems(Guid.Empty, StaticData.Module);
            TreeViewMenu.ItemsSource = tmpSolutionTreeViewItems;
        }

        /// <summary>
        /// 取得模块的树形数据源
        /// </summary>
        /// <param name="parentId">第一层默认null，数据库里面配置null</param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private List<TreeViewItem> GetTreeViewItems(Guid parentId, IEnumerable<Module> nodes)
        {
            //var mainNodes = nodes.Where(x => x.ParentId == parentId).ToList().OrderBy(a => a.OrderCode);
            var mainNodes = nodes.Where(x => x.ParentId == parentId && x.ShowInNavigateMenu == true).ToList().OrderBy(a => a.OrderCode);
            List<TreeViewItem> mainTreeViewItems = new List<TreeViewItem>();
            foreach (var module in mainNodes)
            {
                TreeViewItem mainTreeViewItem = new TreeViewItem();
                mainTreeViewItem.IsExpanded = true;
                mainTreeViewItem.Header = module.Name;
                mainTreeViewItem.ToolTip = module.Name;
                if (string.IsNullOrWhiteSpace(module.Icon))
                {
                    module.Icon = Utility.ConstValue.DefaultIconPath;
                }
                mainTreeViewItem.MouseUp += treeViewItem_MouseUp;
                mainTreeViewItem.MouseEnter += treeViewItem_MouseEnter;
                mainTreeViewItem.MouseLeave += treeViewItem_MouseLeave;

                mainTreeViewItem.Tag = module;
                if (mainTreeViewItem.Items.Count > 0)
                {
                    mainTreeViewItem.Focusable = false;
                }
                var st = new StackPanel() { Orientation = Orientation.Horizontal };
                var icon = Utility.Windows.BitmapImageHelper.GetImage(module.Icon, 16, 16);
                st.Children.Add(icon ?? Utility.Windows.BitmapImageHelper.GetImage("pack://application:,,,/Desktop.Resource;component/Images/Logo_16.ico", 16, 16));
                st.Children.Add(new TextBlock() { Text = module.Name, Margin = new Thickness(2, 0, 0, 0) });
                mainTreeViewItem.Header = st;
                mainTreeViewItems.Add(mainTreeViewItem);
            }
            List<Module> otherNodes = nodes.Where(x => x.ParentId != parentId).OrderBy(a => a.OrderCode).ToList();
            foreach (var treeViewItem in mainTreeViewItems)
            {
                var items = GetTreeViewItems((treeViewItem.Tag as Module).Id, otherNodes);
                foreach (var item in items)
                {
                    treeViewItem.Items.Add(item);
                }
                if (treeViewItem.HasItems)
                {
                    treeViewItem.Focusable = false;
                }
            }
            return mainTreeViewItems;
        }

        /// <summary>
        /// 鼠标选中树形菜单项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem =
              Utility.Windows.DependencyObjectHelper.VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (treeViewItem != null && treeViewItem.Focusable)
            {
                treeViewItem.Focus();
                e.Handled = true;
                Navigate(treeViewItem);
            }
        }

        /// <summary>
        /// 鼠标进入树形菜单项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            var treeViewItem =
               Utility.Windows.DependencyObjectHelper.VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (treeViewItem != null && treeViewItem.Focusable)
            {
                treeViewItem.Opacity = 0.7;
            }
        }

        /// <summary>
        /// 鼠标移开树形菜单项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewItem_MouseLeave(object sender, MouseEventArgs e)
        {
            var treeViewItem =
               Utility.Windows.DependencyObjectHelper.VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (treeViewItem != null && treeViewItem.Focusable)
            {
                treeViewItem.Opacity = 1;

            }
        }

        /// <summary>
        /// 执行导航命令
        /// </summary>
        /// <param name="treeViewItem"></param>
        private void Navigate(TreeViewItem treeViewItem)
        {
            if (treeViewItem != null)
            {
                EventAggregator.GetEvent<NavigateEvent>().Publish(new ViewInfo(ViewType.DockableView, treeViewItem?.Tag));
            }
        }

        #region 树形菜单控件
        private TreeView treeViewMenu = new TreeView();
        /// <summary>
        /// 树形菜单控件
        /// </summary>
        public TreeView TreeViewMenu
        {
            get { return treeViewMenu; }
            set { SetProperty(ref treeViewMenu, value); }
        }
        #endregion

        #region 数据列表的右键快捷菜单

        private List<MenuItem> contextMenus;

        /// <summary>
        /// 右键菜单
        /// </summary>
        public List<MenuItem> ContextMenus
        {
            get { return contextMenus; }
            set { SetProperty(ref contextMenus, value); }
        }
        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Disposing()
        {
            //释放相关的资源
            TreeViewMenu = null;
            dbContext.Db.Close();
            dbContext.Db.Dispose();
            dbContext = null;
            LogHelper.Logger.Debug($"释放资源：{this.ToString()}");
        }
    }

}
