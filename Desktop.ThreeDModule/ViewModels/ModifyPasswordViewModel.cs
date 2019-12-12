using Desktop.Core;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Models;
using Prism.Commands;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Utility;

namespace Desktop.ThreeDModule.ViewModels
{
    /// <summary>
    /// 修改密码VM
    /// </summary>
    public class ModifyPasswordViewModel : Desktop.Core.ViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ModifyPasswordViewModel()
        {
            CurrentDataModel = new ModifyPasswordModel() { Name = StaticData.CurrentUser.Name };
            CurrentDataModel.PropertyChanged += CurrentDataModel_PropertyChanged;
            initCommand();
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
        private ModifyPasswordModel currentDataModel = new ModifyPasswordModel();
        /// <summary>
        /// 当前的数据模型
        /// </summary>
        public ModifyPasswordModel CurrentDataModel
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
                controller = await metroWindow?.ShowProgressAsync("系统提示", "正在保存...");
                controller.SetIndeterminate();
                var user = StaticData.CurrentUser.Clone() as User;
                user.LastUpdatedTime = dbContext.Db.GetDate();
                user.LastUpdatorUser = StaticData.CurrentUser.Name;
                user.SecurityPassword = Security.AESEncrypt(CurrentDataModel.NewPassword);
                if (dbContext.UserDb.Update(user))
                {
                    StaticData.CurrentUser = user;
                    UiMessage = "修改成功,请牢记新密码";
                    controller.SetMessage(UiMessage);
                    await controller.CloseAsync();
                    EventAggregator.GetEvent<ClosePopupEvent>().Publish(metroWindow);
                    EventAggregator.GetEvent<UpListDataEvent>().Publish(CurrentDataModel);
                }
                else
                {
                    UiMessage = "修改失败，请联系管理员";
                    LogHelper.Logger.Error(UiMessage, null);
                }
            }
            catch (Exception ex)
            {
                await controller?.CloseAsync();
                string msg = $"修改密码时错误{ex.Message}";
                LogHelper.Logger.Error(msg, ex);
                metroWindow?.ShowMessageAsync("系统错误", msg);
            }
            finally
            {
                CurrentDataModel.PropertyChanged += CurrentDataModel_PropertyChanged;
            }
        }

        /// <summary>
        /// 是否可以执行确认命令
        /// </summary>
        /// <returns></returns>
        private bool canExecuteConfirmCommand()
        {
            if (Equals(CurrentDataModel, null))
                return false;
            if (string.IsNullOrWhiteSpace(CurrentDataModel.OldPassword))
            {
                UiMessage = "请输入原密码";
                return false;
            }
            if (!Equals(Security.AESEncrypt(CurrentDataModel.OldPassword), StaticData.CurrentUser.SecurityPassword))
            {
                UiMessage = "原密码错误，请重新输入";
                return false;
            }
            if (string.IsNullOrWhiteSpace(CurrentDataModel.NewPassword))
            {
                UiMessage = "请输入新密码";
                return false;
            }
            if (string.IsNullOrWhiteSpace(CurrentDataModel.ConfirmNewPassword))
            {
                UiMessage = "请输入确认密码";
                return false;
            }
            if (!Equals(CurrentDataModel.NewPassword, CurrentDataModel.ConfirmNewPassword))
            {
                UiMessage = "两次输入新密码不一致，请重新输入";
                return false;
            }
            UiMessage = string.Empty;
            return true;
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
