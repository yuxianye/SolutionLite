using Desktop.Core;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Models;
using OpcUaHelper;
using Prism.Commands;
using SqlSugar;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Utility;

namespace Desktop.OpcModule.ViewModels
{
    /// <summary>
    /// 编辑OPC服务器VM
    /// </summary>
    public class OpcServerEditViewModel : Desktop.Core.ViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OpcServerEditViewModel()
        {
            initCommand();
        }

        private void CurrentDataModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //更新按钮可用状态
            ConfirmCommand.RaiseCanExecuteChanged();
            ConnectTestCommand.RaiseCanExecuteChanged();
        }

        protected override void OnParameterChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (base.Parameter.Any(a => a.Key == DataModelName))
            {
                this.CurrentDataModel = (OpcServer)(base.Parameter.FirstOrDefault(a => a.Key == DataModelName).Value as OpcServer).Clone();
                CurrentDataModel.PropertyChanged += CurrentDataModel_PropertyChanged;
            }
        }

        /// <summary>
        /// 数据访问上下文
        /// </summary>
        private Dal.DbContext dbContext = new Dal.DbContext();

        #region OPC服务类型      

        System.Array opcTypes = Enum.GetValues(typeof(OpcType)).Cast<OpcType>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        /// <summary>
        /// OPC服务类型 
        /// </summary>
        public System.Array OpcTypes
        {
            get { return opcTypes; }
            set { SetProperty(ref opcTypes, value); }
        }

        #endregion

        #region 当前的数据模型
        private OpcServer currentDataModel = new OpcServer();
        /// <summary>
        /// 当前的数据模型
        /// </summary>
        public OpcServer CurrentDataModel
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
        /// 测试连接命令
        /// </summary>
        public DelegateCommand ConnectTestCommand { get; set; }

        /// <summary>
        /// 初始化命令,关闭命令始终可用。
        /// </summary>
        private void initCommand()
        {
            ConfirmCommand = new DelegateCommand(executeConfirmCommand, canExecuteConfirmCommand);
            CancelCommand = new DelegateCommand(executeCancelCommand);
            ConnectTestCommand = new DelegateCommand(executeConnectTestCommand, canExecuteConnectTestCommand);

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
                if (dbContext.OpcServerDb.Update(CurrentDataModel))
                {
                    UiMessage = "编辑成功";
                    LogHelper.Logger.Info($"{StaticData.CurrentUser.Name}/{StaticData.CurrentUser.NickName},{UiMessage}{System.Environment.NewLine}{Utility.TypeExtensions.GetPropertiesValue<OpcServer>(CurrentDataModel)}");
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
                string msg = $"编辑OPC服务器时错误{ex.Message}";
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
            if (dbContext.OpcServerDb.Count(a => a.Name == CurrentDataModel.Name && a.Id != CurrentDataModel.Id) > 0)
            {
                UiMessage = $"OPC服务器名{CurrentDataModel.Name}已被使用，请使用其他服务器名";
                return false;
            }
            if (dbContext.OpcServerDb.Count(a => a.Uri == CurrentDataModel.Uri && a.Id != CurrentDataModel.Id) > 0)
            {
                UiMessage = $"地址{CurrentDataModel.Name}已被使用，请使用其他地址";
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

        /// <summary>
        /// 执行连接测试命令
        /// </summary>
        private async void executeConnectTestCommand()
        {
            UiMessage = "开始执行测试连接命令...";


            if (this.CurrentDataModel.OpcType == OpcType.OpcUa)
            {
                OpcUaHelper.OpcUaClientHelper opcUaClientHelper = new OpcUaHelper.OpcUaClientHelper();
                try
                {
                    opcUaClientHelper.ServerUri = CurrentDataModel.Uri;
                    var result = await opcUaClientHelper.ConnectAsync();

                    UiMessage = $"服务器连接结果：{result},连接状态：{opcUaClientHelper.IsConnected}";
                }
                catch (Exception ex)
                {
                    UiMessage = ex.Message;
                }
                finally
                {
                    await opcUaClientHelper.DisConnectAsync();
                    opcUaClientHelper?.Dispose();
                }
            }
            else if (this.CurrentDataModel.OpcType == OpcType.OpcClassics)
            {

                OpcHelper.OpcClientHelper opcClientHelper = new OpcHelper.OpcClientHelper();
                try
                {
                    var result = opcClientHelper.Connect(CurrentDataModel.Uri);

                    UiMessage = $"服务器连接结果：{result},连接状态：{opcClientHelper.IsConnected}";
                }
                catch (Exception ex)
                {
                    UiMessage = ex.Message;
                }
                finally
                {
                    opcClientHelper.DisConnectAsync();
                    opcClientHelper?.Dispose();
                }
            }

        }

        /// <summary>
        /// 是否可以执行执行连接测试命令
        /// </summary>
        /// <returns></returns>
        private bool canExecuteConnectTestCommand()
        {
            if (dbContext.OpcServerDb.Count(a => a.Name == CurrentDataModel.Name && a.Id != CurrentDataModel.Id) > 0)
            {
                UiMessage = $"OPC服务器名{CurrentDataModel.Name}已被使用，请使用其他服务器名";
                return false;
            }
            if (dbContext.OpcServerDb.Count(a => a.Uri == CurrentDataModel.Uri && a.Id != CurrentDataModel.Id) > 0)
            {
                UiMessage = $"地址{CurrentDataModel.Name}已被使用，请使用其他地址";
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
