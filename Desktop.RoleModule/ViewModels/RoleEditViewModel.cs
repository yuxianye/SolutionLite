using Desktop.Core;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
using Utility;

namespace Desktop.RoleModule.ViewModels
{
    /// <summary>
    /// 编辑角色VM
    /// </summary>
    public class RoleEditViewModel : Desktop.Core.ViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RoleEditViewModel()
        {
            initCommand();
            ModuleList.AddRange(dbContext.ModuleDb.GetList().OrderBy(a => a.OrderCode));
        }

        private void CurrentDataModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //更新按钮可用状态
            ConfirmCommand.RaiseCanExecuteChanged();
        }

        protected override void OnParameterChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (base.Parameter.Any(a => a.Key == DataModelName))
            {
                this.CurrentDataModel = (Role)(base.Parameter.FirstOrDefault(a => a.Key == DataModelName).Value as Role).Clone();
                foreach (var v in dbContext.RoleModuleDb.GetList(a => a.RoleId == CurrentDataModel.Id))
                {
                    SelectedModuleList.Add(ModuleList.FirstOrDefault(a => a.Id == v.ModuleId));
                }
                SelectedModuleList.CollectionChanged += SelectedModuleList_CollectionChanged;
                CurrentDataModel.PropertyChanged += CurrentDataModel_PropertyChanged;
            }
        }

        //private bool selectedRoleListChanged = false;
        private void SelectedModuleList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //selectedRoleListChanged = true;
            //更新按钮可用状态
            ConfirmCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// 数据访问上下文
        /// </summary>
        private Dal.DbContext dbContext = new Dal.DbContext();

        #region 当前的数据模型
        private Role currentDataModel = new Role();
        /// <summary>
        /// 当前的数据模型
        /// </summary>
        public Role CurrentDataModel
        {
            get { return currentDataModel; }
            set { SetProperty(ref currentDataModel, value); }
        }
        #endregion

        #region 命令定义和初始化

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
            ConfirmCommand = new DelegateCommand(executeConfirmCommand, canExecuteConfirmCommand);
            CancelCommand = new DelegateCommand(executeCancelCommand);
        }

        #endregion

        #region 命令和消息等执行函数

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
                addAuditedData();
                controller = await metroWindow?.ShowProgressAsync("系统提示", "正在保存...");
                controller.SetIndeterminate();
                //开始事物
                dbContext.Db.Ado.BeginTran();
                if (dbContext.RoleDb.Update(CurrentDataModel))
                {
                    var roleModuleNow = (from a in SelectedModuleList
                                         select new Models.RoleModule
                                         {
                                             Id = Guid.NewGuid(),
                                             RoleId = CurrentDataModel.Id,
                                             ModuleId = a.Id,
                                             CreatedTime = dbContext.Db.GetDate(),
                                             CreatorUser = StaticData.CurrentUser.Name,
                                             LastUpdatedTime = dbContext.Db.GetDate(),
                                             LastUpdatorUser = StaticData.CurrentUser.Name
                                         }).ToList();
                    if (roleModuleNow.Any())
                    {
                        var roleModuleOld = dbContext.RoleModuleDb.GetList(a => a.RoleId == CurrentDataModel.Id);
                        foreach (var v in roleModuleOld)
                        {
                            if (!roleModuleNow.Any(a => a.RoleId == v.RoleId && a.ModuleId == v.ModuleId))
                            {
                                //之前有，现在没有则删除
                                var result = dbContext.RoleModuleDb.Delete(v);
                            }
                        }
                        foreach (var v in roleModuleNow)
                        {
                            if (!dbContext.RoleModuleDb.IsAny(a => a.RoleId == v.RoleId && a.ModuleId == v.ModuleId))
                            {
                                //现在有，之前没有则增加
                                dbContext.RoleModuleDb.Insert(v);
                            }
                        }
                    }
                    else//没有选择模块，那么删除该角色的角色模块RoleModule
                    {
                        dbContext.RoleModuleDb.Delete(a => a.RoleId == CurrentDataModel.Id);
                    }
                    //提交事物
                    dbContext.Db.Ado.CommitTran();
                    UiMessage = "编辑成功";
                    LogHelper.Logger.Info($"{StaticData.CurrentUser.Name }/{StaticData.CurrentUser.NickName},{UiMessage}{System.Environment.NewLine}{Utility.TypeExtensions.GetPropertiesValue<Role>(CurrentDataModel)}");
                    controller.SetMessage(UiMessage);
                    await controller.CloseAsync();
                    EventAggregator.GetEvent<ClosePopupEvent>().Publish(metroWindow);
                    EventAggregator.GetEvent<UpListDataEvent>().Publish(CurrentDataModel);
                }
                else
                {
                    //回滚事物
                    dbContext.Db.Ado.RollbackTran();
                    UiMessage = "编辑失败，请联系管理员";
                    LogHelper.Logger.Error(UiMessage, null);
                }
            }
            catch (Exception ex)
            {
                //回滚事物
                dbContext.Db.Ado.RollbackTran();
                await controller?.CloseAsync();
                string msg = $"编辑角色时错误{ex.Message}";
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
            if (!Equals(CurrentDataModel, null) && CurrentDataModel.IsValidated)
            {
                if (dbContext.RoleDb.Count(a => a.Name == CurrentDataModel.Name && a.Id != CurrentDataModel.Id) > 0)
                {
                    UiMessage = $"角色名{CurrentDataModel.Name}已被使用，请使用其他角色名";
                    return false;
                }
                else
                {
                    UiMessage = string.Empty;
                    return true;
                }
            }
            else
            {
                UiMessage = string.Empty;
                return false;
            }
        }

        #region 模块列表数据
        private ObservableCollection<Module> moduleList = new ObservableCollection<Module>();

        /// <summary>
        /// 模块列表数据
        /// </summary>
        public ObservableCollection<Module> ModuleList
        {
            get { return moduleList; }
            set { SetProperty(ref moduleList, value); }
        }
        #endregion


        #region 选择的模块列表数据
        private ObservableCollection<Module> selectedModuleList = new ObservableCollection<Module>();

        /// <summary>
        /// 选择的模块列表数据
        /// </summary>
        public ObservableCollection<Module> SelectedModuleList
        {
            get { return selectedModuleList; }
            set { SetProperty(ref selectedModuleList, value); }
        }
        #endregion

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
            CurrentDataModel.PropertyChanged += CurrentDataModel_PropertyChanged;
            CurrentDataModel?.Dispose();
            dbContext.Db.Close();
            dbContext.Db.Dispose();
            dbContext = null;
            LogHelper.Logger.Debug($"释放资源：{this.ToString()}");
        }

    }
    //public class RoleEditViewModel : Desktop.Core.ViewModelBase
    //{
    //    /// <summary>
    //    /// 构造函数
    //    /// </summary>
    //    public RoleEditViewModel()
    //    {
    //        initCommand();
    //    }

    //    private void CurrentDataModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //    {
    //        //更新按钮可用状态
    //        ConfirmCommand.RaiseCanExecuteChanged();
    //    }

    //    protected override void OnParameterChanged(object sender, NotifyCollectionChangedEventArgs e)
    //    {
    //        if (base.Parameter.Any(a => a.Key == DataModelName))
    //        {
    //            this.CurrentDataModel = (Role)(base.Parameter.FirstOrDefault(a => a.Key == DataModelName).Value as Role).Clone();
    //            CurrentDataModel.PropertyChanged += CurrentDataModel_PropertyChanged;
    //        }
    //    }

    //    /// <summary>
    //    /// 数据访问上下文
    //    /// </summary>
    //    private Dal.DbContext dbContext = new Dal.DbContext();

    //    #region 当前的数据模型
    //    private Role currentDataModel;
    //    /// <summary>
    //    /// 当前的数据模型
    //    /// </summary>
    //    public Role CurrentDataModel
    //    {
    //        get { return currentDataModel; }
    //        set { SetProperty(ref currentDataModel, value); }
    //    }
    //    #endregion

    //    #region 命令定义和初始化

    //    /// <summary>
    //    /// 确定命令
    //    /// </summary>
    //    public DelegateCommand ConfirmCommand { get; set; }

    //    /// <summary>
    //    /// 关闭命令
    //    /// </summary>
    //    public DelegateCommand CancelCommand { get; set; }

    //    /// <summary>
    //    /// 初始化命令,关闭命令始终可用。
    //    /// </summary>
    //    private void initCommand()
    //    {
    //        ConfirmCommand = new DelegateCommand(executeConfirmCommand, canExecuteConfirmCommand);
    //        CancelCommand = new DelegateCommand(executeCancelCommand);
    //    }

    //    #endregion

    //    #region 命令和消息等执行函数

    //    /// <summary>
    //    /// 执行确认命令
    //    /// </summary>
    //    private async void executeConfirmCommand()
    //    {
    //        CurrentDataModel.PropertyChanged -= CurrentDataModel_PropertyChanged;
    //        ProgressDialogController controller = null;
    //        var metroWindow = Parameter.FirstOrDefault(a => a.Key == ParentName).Value as MetroWindow;

    //        try
    //        {
    //            addAuditedData();
    //            controller = await (metroWindow as MetroWindow)?.ShowProgressAsync("系统提示", "正在保存...");
    //            controller.SetIndeterminate();
    //            if (dbContext.RoleDb.Update(CurrentDataModel))
    //            {
    //                UiMessage = "编辑成功";
    //                LogHelper.Logger.Info($"{StaticData.CurrentUser.Name }/{StaticData.CurrentUser.NickName},{UiMessage}{System.Environment.NewLine}{Utility.TypeExtensions.GetPropertiesValue<Role>(CurrentDataModel)}");
    //                controller.SetMessage(UiMessage);
    //                await controller.CloseAsync();
    //                EventAggregator.GetEvent<ClosePopupEvent>().Publish(metroWindow);
    //                EventAggregator.GetEvent<UpListDataEvent>().Publish(CurrentDataModel);
    //            }
    //            else
    //            {
    //                UiMessage = "编辑失败，请联系管理员";
    //                LogHelper.Logger.Error(UiMessage, null);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            await controller?.CloseAsync();
    //            string msg = $"编辑角色时错误{ex.Message}";
    //            LogHelper.Logger.Error(msg, ex);
    //            (metroWindow as MetroWindow)?.ShowMessageAsync("系统错误", msg);
    //        }
    //        finally
    //        {
    //            CurrentDataModel.PropertyChanged += CurrentDataModel_PropertyChanged;
    //        }
    //    }

    //    /// <summary>
    //    /// 增加审计数据
    //    /// </summary>
    //    private void addAuditedData()
    //    {
    //        CurrentDataModel.LastUpdatedTime = dbContext.Db.GetDate();
    //        CurrentDataModel.LastUpdatorUser = StaticData.CurrentUser.Name;
    //    }

    //    /// <summary>
    //    /// 是否可以执行确认命令
    //    /// </summary>
    //    /// <returns></returns>
    //    private bool canExecuteConfirmCommand()
    //    {
    //        if (!Equals(CurrentDataModel, null) && CurrentDataModel.IsValidated)
    //        {
    //            if (dbContext.RoleDb.Count(a => a.Name == CurrentDataModel.Name && a.Id != CurrentDataModel.Id) > 0)
    //            {
    //                UiMessage = $"角色名{CurrentDataModel.Name}已被使用，请使用其他角色名";
    //                return false;
    //            }
    //            else
    //            {
    //                UiMessage = string.Empty;
    //                return true;
    //            }
    //        }
    //        else
    //        {
    //            UiMessage = string.Empty;
    //            return false;
    //        }
    //    }

    //    /// <summary>
    //    /// 执行关闭命令
    //    /// </summary>
    //    private void executeCancelCommand()
    //    {
    //        EventAggregator.GetEvent<ClosePopupEvent>().Publish(Parameter.FirstOrDefault(a => a.Key == ParentName).Value as MetroWindow);
    //    }

    //    #endregion

    //    /// <summary>
    //    /// 释放资源
    //    /// </summary>
    //    protected override void Disposing()
    //    {
    //        //释放相关的资源
    //        CurrentDataModel.PropertyChanged += CurrentDataModel_PropertyChanged;
    //        CurrentDataModel?.Dispose();
    //        dbContext.Db.Close();
    //        dbContext.Db.Dispose();
    //        dbContext = null;
    //        LogHelper.Logger.Debug($"释放资源：{this.ToString()}");
    //    }

    //}


}
