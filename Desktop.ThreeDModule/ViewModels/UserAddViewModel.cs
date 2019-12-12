using Desktop.Core;
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
using Utility;

namespace Desktop.ThreeDModule.ViewModels
{
    /// <summary>
    /// 新建用户VM
    /// </summary>
    public class UserAddViewModel : Desktop.Core.ViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UserAddViewModel()
        {
            CurrentDataModel.PropertyChanged += CurrentDataModel_PropertyChanged;
            initCommand();
            RoleList = new ObservableCollection<Role>(dbContext.RoleDb.GetList());
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
        private User currentDataModel = new User();
        /// <summary>
        /// 当前的数据模型
        /// </summary>
        public User CurrentDataModel
        {
            get { return currentDataModel; }
            set { SetProperty(ref currentDataModel, value); }
        }
        #endregion

        private ObservableCollection<Role> roleList;

        /// <summary>
        /// 角色列表数据
        /// </summary>
        public ObservableCollection<Role> RoleList
        {
            get { return roleList; }
            set { SetProperty(ref roleList, value); }
        }

        private ObservableCollection<Role> selectedRoleList = new ObservableCollection<Role>();

        /// <summary>
        /// 选择的角色列表数据
        /// </summary>
        public ObservableCollection<Role> SelectedRoleList
        {
            get { return selectedRoleList; }
            set { SetProperty(ref selectedRoleList, value); }
        }

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
                CurrentDataModel.SecurityPassword = Security.AESEncrypt(CurrentDataModel.SecurityPassword);
                controller = await metroWindow?.ShowProgressAsync("系统提示", "正在保存...");
                controller.SetIndeterminate();
                //开始事物
                dbContext.Db.Ado.BeginTran();
                if (dbContext.UserDb.Insert(CurrentDataModel))
                {

                    var userRole = from a in SelectedRoleList
                                   select new UserRole
                                   {
                                       Id = Guid.NewGuid(),
                                       UserId = CurrentDataModel.Id,
                                       RoleId = a.Id,
                                       CreatedTime = dbContext.Db.GetDate(),
                                       CreatorUser = StaticData.CurrentUser.Name,
                                       LastUpdatedTime = dbContext.Db.GetDate(),
                                       LastUpdatorUser = StaticData.CurrentUser.Name
                                   };
                    if (userRole.Any())
                    {
                        dbContext.UserRoleDb.InsertRange(userRole?.ToList());
                    }
                    //提交事物
                    dbContext.Db.Ado.CommitTran();
                    UiMessage = "新建成功";
                    LogHelper.Logger.Info($"{StaticData.CurrentUser.Name }/{StaticData.CurrentUser.NickName},{UiMessage}{System.Environment.NewLine}{Utility.TypeExtensions.GetPropertiesValue<User>(CurrentDataModel)}");
                    controller.SetMessage(UiMessage);
                    await controller.CloseAsync();
                    EventAggregator.GetEvent<ClosePopupEvent>().Publish(metroWindow);
                    EventAggregator.GetEvent<UpListDataEvent>().Publish(CurrentDataModel);
                }
                else
                {
                    //回滚事物
                    dbContext.Db.Ado.RollbackTran();
                    UiMessage = "新建失败，请联系管理员";
                    LogHelper.Logger.Error(UiMessage, null);
                }
            }
            catch (Exception ex)
            {
                //回滚事物
                dbContext.Db.Ado.RollbackTran();
                await controller?.CloseAsync();
                string msg = $"新建用户时错误{ex.Message}";
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
            if (Equals(CurrentDataModel, null))
                return false;
            if (dbContext.UserDb.Count(a => a.Name == CurrentDataModel.Name) > 0)
            {
                UiMessage = $"用户名{CurrentDataModel.Name}已被使用，请使用其他用户名";
                return false;
            }

            if (!Equals(CurrentDataModel, null) && CurrentDataModel.IsValidated)
            {
                if (!Equals(CurrentDataModel.SecurityPassword, CurrentDataModel.ConfirmSecurityPassword))
                {
                    UiMessage = "两次输入新密码不一致，请重新输入";
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

}
