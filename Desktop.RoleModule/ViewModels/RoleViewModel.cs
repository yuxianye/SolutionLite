﻿using Desktop.Core;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Models;
using Prism.Commands;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Utility.Windows;

namespace Desktop.RoleModule.ViewModels
{
    /// <summary>
    /// 角色管理VM
    /// </summary>
    public class RoleViewModel : Desktop.Core.PageableViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RoleViewModel()
        {
            initCommand();
            initMenus();
            EventAggregator.GetEvent<UpListDataEvent>().Subscribe(upListData, Prism.Events.ThreadOption.BackgroundThread, false, x =>
            {
                return (x is Role);

            });
            getPageData(1, PageSize);
        }

        /// <summary>
        /// 数据访问上下文
        /// </summary>
        private Dal.DbContext dbContext = new Dal.DbContext();

        #region 程序集名称和页面名称
        /// <summary>
        /// 程序集名称
        /// </summary>
        private static readonly string assemblyName = "Desktop.RoleModule";
        /// <summary>
        /// 新建页面的名称
        /// </summary>
        private static readonly string addViewName = "Desktop.RoleModule.Views.RoleAddView";
        /// <summary>
        /// 编辑页面的名称
        /// </summary>
        private static readonly string editViewName = "Desktop.RoleModule.Views.RoleEditView";
        /// <summary>
        /// 删除页面的名称
        /// </summary>
        private static readonly string deleteViewName = "Desktop.RoleModule.Views.RoleDeleteView";
        /// <summary>
        /// 导入页面的名称
        /// </summary>
        private static readonly string importViewName = "Desktop.RoleModule.Views.RoleImportView";
        /// <summary>
        /// 导出页面的名称
        /// </summary>
        private static readonly string exportViewName = "Desktop.RoleModule.Views.RoleExportView";
        /// <summary>
        /// 打印页面的名称
        /// </summary>
        private static readonly string printViewName = "Desktop.RoleModule.Views.RolePrintView";

        #endregion

        #region 列表数据
        private ObservableCollection<Role> dataList = new ObservableCollection<Role>();

        /// <summary>
        /// 列表数据
        /// </summary>
        public ObservableCollection<Role> DataList
        {
            get { return dataList; }
            set { SetProperty(ref dataList, value); }
        }
        #endregion

        #region 选中的数据
        private Role selectedData;
        /// <summary>
        /// 选中的数据
        /// </summary>
        public Role SelectedData
        {
            get { return selectedData; }
            set
            {
                SetProperty(ref selectedData, value);
                //数据选择改变后更新编辑和删除按钮可用状态
                EditCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region 命令定义和初始化

        /// <summary>
        /// 新建命令
        /// </summary>
        public DelegateCommand AddCommand { get; set; }

        /// <summary>
        /// 编辑命令
        /// </summary>
        public DelegateCommand EditCommand { get; set; }

        /// <summary>
        /// 删除命令
        /// </summary>
        public DelegateCommand DeleteCommand { get; set; }

        /// <summary>
        /// 刷新命令
        /// </summary>
        public DelegateCommand RefreshCommand { get; set; }

        /// <summary>
        /// 导入命令
        /// </summary>
        public DelegateCommand ImportCommand { get; set; }

        /// <summary>
        /// 导出命令
        /// </summary>
        public DelegateCommand ExportCommand { get; set; }

        /// <summary>
        /// 打印命令
        /// </summary>
        public DelegateCommand PrintCommand { get; set; }

        /// <summary>
        /// 查询命令
        /// </summary>
        public DelegateCommand<string> SearchCommand { get; set; }

        /// <summary>
        /// 初始化命令,刷新和查询命令始终可用。
        /// </summary>
        private void initCommand()
        {
            AddCommand = new DelegateCommand(executeAddCommand, canExecuteAddCommand);
            EditCommand = new DelegateCommand(executeEditCommand, canExecuteEditCommand);
            DeleteCommand = new DelegateCommand(executeDeleteCommand, canExecuteDeleteCommand);
            RefreshCommand = new DelegateCommand(executeRefreshCommand);
            ImportCommand = new DelegateCommand(executeImportCommand, canExecuteImportCommand);
            ExportCommand = new DelegateCommand(executeExportCommand, canExecuteExportCommand);
            PrintCommand = new DelegateCommand(executePrintCommand, canExecutePrintCommand);
            SearchCommand = new DelegateCommand<string>(executeSearchCommand);
        }

        #endregion

        #region 右键菜单定义和初始化
        private void initMenus()
        {
            double width = 16;
            double height = 16;

            var add = BitmapImageHelper.GetImage(@"pack://application:,,,/Desktop.Resource;component/Images/Add_32.png");
            add.Width = width;
            add.Height = height;
            MenuItems.Add(new MenuItem() { Icon = add, Header = ResourceHelper.FindResource("New"), Command = AddCommand });

            var edit = BitmapImageHelper.GetImage(@"pack://application:,,,/Desktop.Resource;component/Images/Edit_32.png");
            edit.Width = width;
            edit.Height = height;
            MenuItems.Add(new MenuItem() { Icon = edit, Header = ResourceHelper.FindResource("Edit"), Command = EditCommand });

            var delete = BitmapImageHelper.GetImage(@"pack://application:,,,/Desktop.Resource;component/Images/Delete_32.png");
            delete.Width = width;
            delete.Height = height;
            MenuItems.Add(new MenuItem() { Icon = delete, Header = ResourceHelper.FindResource("Delete"), Command = DeleteCommand });

            var refresh = BitmapImageHelper.GetImage(@"pack://application:,,,/Desktop.Resource;component/Images/Refresh_32.png");
            refresh.Width = width;
            refresh.Height = height;
            MenuItems.Add(new MenuItem() { Icon = refresh, Header = ResourceHelper.FindResource("Refresh"), Command = RefreshCommand });

            var import = BitmapImageHelper.GetImage(@"pack://application:,,,/Desktop.Resource;component/Images/Import_32.png");
            import.Width = width;
            import.Height = height;
            MenuItems.Add(new MenuItem() { Icon = import, Header = ResourceHelper.FindResource("Import"), Command = ImportCommand });

            var export = BitmapImageHelper.GetImage(@"pack://application:,,,/Desktop.Resource;component/Images/Export_32.png");
            export.Width = width;
            export.Height = height;
            MenuItems.Add(new MenuItem() { Icon = export, Header = ResourceHelper.FindResource("Export"), Command = ExportCommand });

            var print = BitmapImageHelper.GetImage(@"pack://application:,,,/Desktop.Resource;component/Images/Print_32.png");
            print.Width = width;
            print.Height = height;
            MenuItems.Add(new MenuItem() { Icon = print, Header = ResourceHelper.FindResource("Print"), Command = PrintCommand });
        }
        #endregion

        #region 命令和消息等执行函数

        /// <summary>
        /// 执行新建命令
        /// </summary>
        private void executeAddCommand()
        {
            EventAggregator.GetEvent<NavigateEvent>().Publish(new ViewInfo(ViewType.Popup,
                StaticData.Module.FirstOrDefault(a => a.AssemblyName == assemblyName && a.ViewName == addViewName)));
        }

        /// <summary>
        /// 是否可以执行新建命令
        /// </summary>
        /// <returns></returns>
        private bool canExecuteAddCommand()
        {
            if (!Equals(StaticData.Module, null) && StaticData.Module.Any(a => a.AssemblyName == assemblyName && a.ViewName == addViewName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 执行编辑命令
        /// </summary>
        private void executeEditCommand()
        {
            EventAggregator.GetEvent<NavigateEvent>().Publish(new ViewInfo(ViewType.Popup,
                StaticData.Module.FirstOrDefault(a => a.AssemblyName == assemblyName && a.ViewName == editViewName), SelectedData));
        }

        /// <summary>
        /// 是否可以执行编辑命令
        /// </summary>
        /// <returns></returns>
        private bool canExecuteEditCommand()
        {
            if (!Equals(StaticData.Module, null) && StaticData.Module.Any(a => a.AssemblyName == assemblyName && a.ViewName == editViewName)
                && !Equals(SelectedData, null))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 执行删除命令
        /// </summary>
        private async void executeDeleteCommand()
        {
            var metroWindow = Application.Current.MainWindow as MetroWindow;
            try
            {
                //关联角色的表有用户角色（UserRole）,
                if (dbContext.UserRoleDb.IsAny(a => a.RoleId == SelectedData.Id))
                {
                    var controller = await metroWindow?.ShowProgressAsync("系统提示", $"该数据已被使用，不能删除{System.Environment.NewLine}{System.Environment.NewLine}若要删除请先移除该数据的用户数据关联");
                    controller.SetIndeterminate();
                    await Task.Delay(StaticData.Delay);
                    await controller.CloseAsync();
                    return;
                }
                var dialogResult = await metroWindow?.ShowMessageAsync("删除数据"
                                   , string.Format("确定要删除【{0}】这条数据么？" + System.Environment.NewLine + "确认删除点击【是】，取消删除点击【否】。", SelectedData.Name)
                                   , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "是", NegativeButtonText = "否" });
                if (dialogResult == MessageDialogResult.Affirmative)
                {
                    if (dbContext.RoleDb.DeleteById(SelectedData.Id))
                    {
                        getPageData(1, PageSize);
                        LogHelper.Logger.Info($"{StaticData.CurrentUser.Name }/{StaticData.CurrentUser.NickName},{UiMessage}{System.Environment.NewLine}{Utility.TypeExtensions.GetPropertiesValue<Role>(SelectedData)}");
                        UiMessage = $"删除{SelectedData.Name}成功";
                        SelectedData = null;
                    }
                    else
                    {
                        UiMessage = "删除失败，请联系管理员";
                        LogHelper.Logger.Error(UiMessage, null);
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = $"删除角色时错误{ex.Message}";
                LogHelper.Logger.Error(msg, ex);
                metroWindow?.ShowMessageAsync("系统错误", msg);
            }
        }

        /// <summary>
        /// 是否可以执行物理删除命令
        /// </summary>
        /// <returns></returns>
        private bool canExecuteDeleteCommand()
        {
            if (!Equals(StaticData.Module, null) && StaticData.Module.Any(a => a.AssemblyName == assemblyName && a.ViewName == deleteViewName)
               && !Equals(SelectedData, null))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 执行刷新命令
        /// </summary>
        private void executeRefreshCommand()
        {
            getPageData(1, PageSize);
        }

        /// <summary>
        /// 执行导入命令
        /// </summary>
        private void executeImportCommand()
        {
            EventAggregator.GetEvent<NavigateEvent>().Publish(new ViewInfo(ViewType.Popup,
              StaticData.Module.FirstOrDefault(a => a.AssemblyName == assemblyName && a.ViewName == importViewName)));
        }

        /// <summary>
        /// 是否可以执行导入命令
        /// </summary>
        /// <returns></returns>
        private bool canExecuteImportCommand()
        {
            if (!Equals(StaticData.Module, null) && StaticData.Module.Any(a => a.AssemblyName == assemblyName && a.ViewName == importViewName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 执行导出命令
        /// </summary>
        private void executeExportCommand()
        {
            EventAggregator.GetEvent<NavigateEvent>().Publish(new ViewInfo(ViewType.Popup,
              StaticData.Module.FirstOrDefault(a => a.AssemblyName == assemblyName && a.ViewName == exportViewName)));
        }

        /// <summary>
        /// 是否可以执行导出命令
        /// </summary>
        /// <returns></returns>
        private bool canExecuteExportCommand()
        {
            if (!Equals(StaticData.Module, null) && StaticData.Module.Any(a => a.AssemblyName == assemblyName && a.ViewName == exportViewName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 执行打印命令
        /// </summary>
        private void executePrintCommand()
        {
            EventAggregator.GetEvent<NavigateEvent>().Publish(new ViewInfo(ViewType.Popup,
                         StaticData.Module.FirstOrDefault(a => a.AssemblyName == assemblyName && a.ViewName == printViewName)));
        }

        /// <summary>
        /// 是否可以执行打印命令
        /// </summary>
        /// <returns></returns>
        private bool canExecutePrintCommand()
        {
            if (!Equals(StaticData.Module, null) && StaticData.Module.Any(a => a.AssemblyName == assemblyName && a.ViewName == printViewName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 执行查询命令
        /// </summary>
        private void executeSearchCommand(string searchText)
        {
            getPageData(1, PageSize, searchText);
        }

        #endregion

        /// <summary>
        /// 执行分页改变事件函数
        /// </summary>
        /// <param name="e"></param>
        public override void OnExecutePageChangedCommand(PageChangedEventArgs e)
        {
            getPageData(e.PageIndex, e.PageSize);
        }

        /// <summary>
        /// 更新列表数据
        /// </summary>
        /// <param name="obj"></param>
        private void upListData(object obj)
        {
            getPageData(1, PageSize);
        }

        /// <summary>
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getPageData(int pageIndex, int pageSize, string searchText = null)
        {
            UiMessage = "加载数据中...";

            Task.Factory.StartNew(() =>
             {
                 try
                 {
                     Expression<Func<Role, bool>> expression = null;
                     if (string.IsNullOrWhiteSpace(searchText))//所有
                     {
                         expression = (a) => a.Id != null;
                     }
                     else//特定条件
                     {
                         expression = (a) => a.Name.Contains(searchText) || a.CreatorUser.Contains(searchText) || a.LastUpdatorUser.Contains(searchText);
                     }
                     //根据条件查询分页数据
                     DataList = new ObservableCollection<Role>(dbContext.RoleDb.GetPageList(expression,
                          new PageModel() { PageIndex = pageIndex, PageSize = pageSize }, a => a.LastUpdatedTime, OrderByType.Desc));
                     TotalCounts = dbContext.RoleDb.Count(expression);
                     UiMessage = "数据加载完成";
                 }
                 catch (Exception ex)
                 {
                     UiMessage = $"数据加载时错误，{ex.Message }";
                     LogHelper.Logger.Error(UiMessage, ex);
                 }
             });
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Disposing()
        {
            //释放相关的资源
            EventAggregator.GetEvent<UpListDataEvent>().Unsubscribe(upListData);
            dbContext.Db.Close();
            dbContext.Db.Dispose();
            dbContext = null;
            LogHelper.Logger.Debug($"释放资源：{this.ToString()}");
        }

    }

}
