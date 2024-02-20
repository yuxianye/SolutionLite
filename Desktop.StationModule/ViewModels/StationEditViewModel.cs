using Desktop.Core;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Models;
using Prism.Commands;
using SqlSugar;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Utility;

namespace Desktop.StationModule.ViewModels
{
    /// <summary>
    /// 编辑工位VM
    /// </summary>
    public class StationEditViewModel : Desktop.Core.ViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StationEditViewModel()
        {
            initCommand();
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
                this.CurrentDataModel = (Station)(base.Parameter.FirstOrDefault(a => a.Key == DataModelName).Value as Station).Clone();
                CurrentDataModel.PropertyChanged += CurrentDataModel_PropertyChanged;
            }
        }

        /// <summary>
        /// 数据访问上下文
        /// </summary>
        private Dal.DbContext dbContext = new Dal.DbContext();

        #region 当前的数据模型
        private Station currentDataModel = new Station();
        /// <summary>
        /// 当前的数据模型
        /// </summary>
        public Station CurrentDataModel
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
                controller = await (metroWindow as MetroWindow)?.ShowProgressAsync("系统提示", "正在保存...");
                controller.SetIndeterminate();
                if (dbContext.StationDb.Update(CurrentDataModel))
                {
                    UiMessage = "编辑成功";
                    LogHelper.Logger.Info($"{StaticData.CurrentUser.Name}/{StaticData.CurrentUser.NickName},{UiMessage}{System.Environment.NewLine}{Utility.TypeExtensions.GetPropertiesValue<Station>(CurrentDataModel)}");
                    controller.SetMessage(UiMessage);
                    await controller.CloseAsync();
                    EventAggregator.GetEvent<ClosePopupEvent>().Publish(metroWindow);
                    EventAggregator.GetEvent<UpListDataEvent>().Publish(CurrentDataModel);
                }
                else
                {
                    UiMessage = "编辑失败，请联系管理员";
                    LogHelper.Logger.Error(UiMessage, null);
                }
            }
            catch (Exception ex)
            {
                await controller?.CloseAsync();
                string msg = $"编辑工位时错误{ex.Message}";
                LogHelper.Logger.Error(msg, ex);
                (metroWindow as MetroWindow)?.ShowMessageAsync("系统错误", msg);
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
            CurrentDataModel.LastUpdatedTime = dbContext.Db.GetDate();
            CurrentDataModel.LastUpdatorUser = StaticData.CurrentUser.Name;
        }

        /// <summary>
        /// 是否可以执行确认命令
        /// </summary>
        /// <returns></returns>
        private bool canExecuteConfirmCommand()
        {

            if (dbContext.StationDb.Count(a => a.Code == CurrentDataModel.Code && a.Id != CurrentDataModel.Id) > 0)
            {
                UiMessage = $"工位编号【{CurrentDataModel.Code}】已被使用，请使用其他名称";
                return false;
            }

            if (dbContext.StationDb.Count(a => a.Name == CurrentDataModel.Name && a.Id != CurrentDataModel.Id) > 0)
            {
                UiMessage = $"工位名称【{CurrentDataModel.Name}】已被使用，请使用其他名称";
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
            CurrentDataModel.PropertyChanged += CurrentDataModel_PropertyChanged;
            CurrentDataModel?.Dispose();
            dbContext.Db.Close();
            dbContext.Db.Dispose();
            dbContext = null;
            LogHelper.Logger.Debug($"释放资源：{this.ToString()}");
        }

    }

}
