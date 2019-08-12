using Desktop.Core;
using Desktop.ModuleModule.Models;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Models;
using Prism.Commands;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Utility;

namespace Desktop.ModuleModule.ViewModels
{
    /// <summary>
    /// 新建模块VM
    /// </summary>
    public class ModuleAddViewModel : Desktop.Core.ViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ModuleAddViewModel()
        {
            CurrentDataModel.PropertyChanged += CurrentDataModel_PropertyChanged;
            initCommand();
            var treeViewItems = new ObservableCollection<TreeViewItem>(GetTreeViewItems(Guid.Empty, dbContext.ModuleDb.GetList()));
            var st = new StackPanel() { Orientation = Orientation.Horizontal };
            var icon = Utility.Windows.BitmapImageHelper.GetImage(Utility.ConstValue.DefaultIconPath, 16, 16);
            st.Children.Add(icon ?? Utility.Windows.BitmapImageHelper.GetImage(Utility.ConstValue.DefaultIconPath));
            st.Children.Add(new TextBlock() { Text = "顶级菜单", Margin = new Thickness(2, 0, 0, 0) });
            treeViewItems.Insert(0, new TreeViewItem() { Header = st, Tag = new Module() { Id = Guid.Empty, Name = "顶级菜单", ParentId = Guid.Empty } });
            ModuleList = treeViewItems;
        }

        protected override void OnParameterChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.OnParameterChanged(sender, e);
            if (base.Parameter.Any(a => a.Key == DataModelName))
            {
                var parentModule = (Module)((base.Parameter.FirstOrDefault(a => a.Key == DataModelName).Value as TreeGridItem)?.Value as Module)?.Clone();
                SelectedModule = parentModule;
            }
        }

        /// <summary>
        /// 取得模块的树形数据源
        /// </summary>
        /// <param name="parentId">第一层默认null，数据库里面配置null</param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private List<TreeViewItem> GetTreeViewItems(Guid parentId, List<Module> nodes)
        {
            var mainNodes = nodes.Where(x => x.ParentId == parentId).ToList().OrderBy(a => a.OrderCode);
            List<TreeViewItem> mainTreeViewItems = new List<TreeViewItem>();
            foreach (var module in mainNodes)
            {
                TreeViewItem mainTreeViewItem = new TreeViewItem();
                if (parentId == Guid.Empty)
                {
                    mainTreeViewItem.IsExpanded = true;
                }
                mainTreeViewItem.Header = module.Name;
                if (string.IsNullOrWhiteSpace(module.Icon))
                {
                    module.Icon = Utility.ConstValue.DefaultIconPath;
                }
                mainTreeViewItem.Tag = module;
                var st = new StackPanel() { Orientation = Orientation.Horizontal };
                var icon = Utility.Windows.BitmapImageHelper.GetImage(module.Icon, 16, 16);
                st.Children.Add(icon ?? Utility.Windows.BitmapImageHelper.GetImage(Utility.ConstValue.DefaultIconPath));
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
            }
            return mainTreeViewItems;
        }

        private void CurrentDataModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //更新按钮可用状态
            ConfirmCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// 数据访问上下文
        /// </summary>
        private Dal.DbContext dbContext = new Dal.DbContext();

        #region 当前的数据模型
        private Module currentDataModel = new Module();
        /// <summary>
        /// 当前的数据模型
        /// </summary>
        public Module CurrentDataModel
        {
            get { return currentDataModel; }
            set { SetProperty(ref currentDataModel, value); }
        }
        #endregion

        #region 模块列表数据
        private ObservableCollection<TreeViewItem> moduleList;
        /// <summary>
        /// 模块列表数据
        /// </summary>
        public ObservableCollection<TreeViewItem> ModuleList
        {
            get { return moduleList; }
            set { SetProperty(ref moduleList, value); }
        }
        #endregion

        #region 选择的模块位置
        private int comboBoxSelectedIndex = 0;
        /// <summary>
        /// 选择的模块位置，始终保持0
        /// </summary>
        public int ComboBoxSelectedIndex
        {
            get { return comboBoxSelectedIndex; }
            set { SetProperty(ref comboBoxSelectedIndex, value); }
        }
        #endregion

        #region 选择的模块
        private Module selectedModule;
        /// <summary>
        /// 选择的模块
        /// </summary>
        public Module SelectedModule
        {
            get { return selectedModule; }
            set { SetProperty(ref selectedModule, value); }
        }
        #endregion

        #region 命令定义和初始化

        /// <summary>
        /// 下拉列表弹出框关闭命令
        /// </summary>
        public DelegateCommand ComboBoxDropDownClosedCommand { get; set; }

        /// <summary>
        /// 模块选择改变命令
        /// </summary>
        public DelegateCommand<TreeViewItem> ModuleSelectedCommand { get; set; }

        /// <summary>
        /// 程序集名称命令
        /// </summary>
        public DelegateCommand AssemblyNameCommand { get; set; }

        /// <summary>
        /// 页面名称命令
        /// </summary>
        public DelegateCommand ViewNameCommand { get; set; }

        /// <summary>
        /// 图标名称命令
        /// </summary>
        public DelegateCommand IconCommand { get; set; }

        /// <summary>
        /// 确定命令
        /// </summary>
        public DelegateCommand ConfirmCommand { get; set; }

        /// <summary>
        /// 关闭命令
        /// </summary>
        public DelegateCommand CancelCommand { get; set; }



        /// <summary>
        /// 初始化命令,关闭命令始终可用。
        /// </summary>
        private void initCommand()
        {
            ModuleSelectedCommand = new DelegateCommand<TreeViewItem>(executeModuleSelectedCommand);
            ComboBoxDropDownClosedCommand = new DelegateCommand(executeComboBoxDropDownClosedCommand);

            AssemblyNameCommand = new DelegateCommand(executeAssemblyNameCommand);
            ViewNameCommand = new DelegateCommand(executeViewNameCommand);
            IconCommand = new DelegateCommand(executeIconCommand);

            ConfirmCommand = new DelegateCommand(executeConfirmCommand, canExecuteConfirmCommand);
            CancelCommand = new DelegateCommand(executeCancelCommand);
        }

        #endregion

        #region 命令和消息等执行函数

        /// <summary>
        /// 上级模块选择变化事件函数
        /// </summary>
        /// <param name="treeViewItem"></param>
        private void executeModuleSelectedCommand(TreeViewItem treeViewItem)
        {
            SelectedModule = (treeViewItem?.Tag as Module);
        }

        /// <summary>
        /// 下拉列表弹出框关闭命令事件函数
        /// </summary>
        private void executeComboBoxDropDownClosedCommand()
        {
            ComboBoxSelectedIndex = 0;
        }

        private void executeAssemblyNameCommand()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "dll文件(*.dll)|*.dll|所有文件(*.*)|*.*";
            openFileDialog.InitialDirectory = ConstValue.AppPath;
            if (openFileDialog.ShowDialog() == true)
            {
                var tmp = openFileDialog.SafeFileName.TrimEnd(new char[] { '.', 'd', 'l', 'l' }).TrimEnd(new char[] { '.', 'D', 'L', 'L' });
                CurrentDataModel.AssemblyName = tmp;
                tmp = null;
            }
        }

        /// <summary>
        /// 执行图标选择命令
        /// </summary>
        private void executeViewNameCommand()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "dll文件(*.dll)|*.dll|所有文件(*.*)|*.*";
            openFileDialog.InitialDirectory = ConstValue.AppPath;
            if (openFileDialog.ShowDialog() == true)
            {
                var tmp = openFileDialog.SafeFileName.TrimEnd(new char[] { '.', 'd', 'l', 'l' }).TrimEnd(new char[] { '.', 'D', 'L', 'L' });
                CurrentDataModel.ViewName = tmp + ".Views.Add/Edit???View";
            }
        }

        /// <summary>
        /// 执行图标选择命令
        /// </summary>
        private void executeIconCommand()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "图片|*.jpg;*.png;*.gif;*.jpeg;*.bmp|所有文件(*.*)|*.*";
            openFileDialog.InitialDirectory = ConstValue.AppPath;
            if (openFileDialog.ShowDialog() == true)
            {
                string targetPath = System.IO.Path.Combine(ConstValue.AppPath + "Images", openFileDialog.SafeFileName);
                if (!System.IO.File.Exists(targetPath))
                {
                    if (!Equals(targetPath, openFileDialog.FileName))
                    {
                        System.IO.File.Copy(openFileDialog.FileName, targetPath);
                        UiMessage = $"软件图片目录没有此同名文件{openFileDialog.SafeFileName}，将自动复制到软件图片目录中";
                    }
                    else
                    {
                        UiMessage = string.Empty;
                    }
                }
                else
                {
                    UiMessage = string.Empty;
                }
                CurrentDataModel.Icon = targetPath.Replace(ConstValue.AppPath, string.Empty);
            }
        }

        /// <summary>
        /// 执行确认命令
        /// </summary>
        private async void executeConfirmCommand()
        {
            CurrentDataModel.PropertyChanged -= CurrentDataModel_PropertyChanged;
            ProgressDialogController controller = null;
            var metroWindow = Parameter.FirstOrDefault(a => a.Key == ParentName).Value as MetroWindow;
            try
            {
                CurrentDataModel.ParentId = SelectedModule is null ? Guid.Empty : SelectedModule.Id;
                addAuditedData();
                controller = await metroWindow?.ShowProgressAsync("系统提示", "正在保存...");
                controller.SetIndeterminate();
                if (dbContext.ModuleDb.Insert(CurrentDataModel))
                {
                    UiMessage = "新建成功";
                    LogHelper.Logger.Info($"{StaticData.CurrentUser.Name }/{StaticData.CurrentUser.NickName},{UiMessage}{System.Environment.NewLine}{Utility.TypeExtensions.GetPropertiesValue<Module>(CurrentDataModel)}");
                    controller.SetMessage(UiMessage);
                    await controller.CloseAsync();
                    EventAggregator.GetEvent<ClosePopupEvent>().Publish(metroWindow);
                    EventAggregator.GetEvent<UpListDataEvent>().Publish(CurrentDataModel);
                }
                else
                {
                    UiMessage = "新建失败，请联系管理员";
                    LogHelper.Logger.Error(UiMessage, null);
                }
            }
            catch (Exception ex)
            {
                await controller?.CloseAsync();
                string msg = $"新建模块时错误{ex.Message}";
                LogHelper.Logger.Error(msg, ex);
                metroWindow?.ShowMessageAsync("系统错误", msg);
            }
            finally
            {
                CurrentDataModel.PropertyChanged += CurrentDataModel_PropertyChanged;
            }
        }

        /// <summary>
        /// 增加审计数据
        /// </summary>
        private void addAuditedData()
        {
            CurrentDataModel.CreatedTime = dbContext.Db.GetDate();
            CurrentDataModel.CreatorUser = StaticData.CurrentUser.Name;
            CurrentDataModel.LastUpdatedTime = dbContext.Db.GetDate();
            CurrentDataModel.LastUpdatorUser = StaticData.CurrentUser.Name;
        }

        /// <summary>
        /// 是否可以执行确认命令
        /// </summary>
        /// <returns></returns>
        private bool canExecuteConfirmCommand()
        {
            if (dbContext.ModuleDb.Count(a => a.Name == CurrentDataModel.Name) > 0)
            {
                UiMessage = $"模块名{CurrentDataModel.Name}已被使用，请使用其他模块名";
                return false;
            }
            if (CurrentDataModel.IsValidated)
            {

                UiMessage = string.Empty;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 执行关闭命令
        /// </summary>
        private void executeCancelCommand()
        {
            EventAggregator.GetEvent<ClosePopupEvent>().Publish(Parameter.FirstOrDefault(a => a.Key == ParentName).Value as MetroWindow);
        }

        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Disposing()
        {
            //释放相关的资源
            CurrentDataModel.PropertyChanged -= CurrentDataModel_PropertyChanged;
            CurrentDataModel?.Dispose();
            dbContext.Db.Close();
            dbContext.Db.Dispose();
            dbContext = null;
            LogHelper.Logger.Debug($"释放资源：{this.ToString()}");
        }

    }

}
