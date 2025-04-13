using Desktop.Core;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
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

namespace Desktop.VisionModule.ViewModels
{
    /// <summary>
    /// 编辑节点VM
    /// </summary>
    public class PredictRecoredEditViewModel : Desktop.Core.ViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PredictRecoredEditViewModel()
        {
            Stations = new ObservableCollection<Station>(dbContext.StationDb.GetList());
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
                this.CurrentDataModel = (PredictRecored)(base.Parameter.FirstOrDefault(a => a.Key == DataModelName).Value as PredictRecored).Clone();
                CurrentDataModel.PropertyChanged += CurrentDataModel_PropertyChanged;
            }
        }

        /// <summary>
        /// 数据访问上下文
        /// </summary>
        private Dal.DbContext dbContext = new Dal.DbContext();

        #region 集合      

        private ObservableCollection<Station> stations;

        /// <summary>
        /// Stations集合 
        /// </summary>
        public ObservableCollection<Station> Stations
        {
            get { return stations; }
            set { SetProperty(ref stations, value); }
        }
        #endregion
        #region VehicleType类型      

        private System.Array vehicleTypes = Enum.GetValues(typeof(VehicleType)).Cast<VehicleType>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        /// <summary>
        /// 类型类表
        /// </summary>
        public System.Array VehicleTypes
        {
            get { return vehicleTypes; }
            set { SetProperty(ref vehicleTypes, value); }
        }

        #endregion

        #region ExteriorColor类型      

        private System.Array exteriorColors = Enum.GetValues(typeof(ExteriorColor)).Cast<ExteriorColor>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        /// <summary>
        /// 类型类表
        /// </summary>
        public System.Array ExteriorColors
        {
            get { return exteriorColors; }
            set { SetProperty(ref exteriorColors, value); }
        }

        #endregion


        #region 当前的数据模型
        private PredictRecored currentDataModel;
        /// <summary>
        /// 当前的数据模型
        /// </summary>
        public PredictRecored CurrentDataModel
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

        public DelegateCommand ImageUriCommand { get; set; }


        /// <summary>
        /// 初始化命令,关闭命令始终可用。
        /// </summary>
        private void initCommand()
        {
            ConfirmCommand = new DelegateCommand(executeConfirmCommand, canExecuteConfirmCommand);
            CancelCommand = new DelegateCommand(executeCancelCommand);
            ImageUriCommand = new DelegateCommand(executeImageUriCommand);

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
                if (dbContext.PredictRecoredDb.Update(CurrentDataModel))
                {
                    UiMessage = "编辑成功";
                    LogHelper.Logger.Info($"{StaticData.CurrentUser.Name}/{StaticData.CurrentUser.NickName},{UiMessage}{System.Environment.NewLine}{Utility.TypeExtensions.GetPropertiesValue<PredictRecored>(CurrentDataModel)}");
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
                string msg = $"编辑时错误{ex.Message}";
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
            if (Equals(CurrentDataModel, null))
            {
                return false;
            }

            if (dbContext.PredictRecoredDb.IsAny(a => a.Name == CurrentDataModel.Name && a.Id != CurrentDataModel.Id && a.StationId == CurrentDataModel.StationId))
            {
                UiMessage = $"选择的工位已经存在检测记录名称{CurrentDataModel.Name}，不能重复添加";
                return false;
            }
            if (CurrentDataModel.StationId == Guid.Empty)
            {
                UiMessage = $"请选择工位";
                return false;
            }
            if (CurrentDataModel.IsValidated)
            {

                UiMessage = string.Empty;
                return true;
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




        /// <summary>
        /// 执行图标选择命令
        /// </summary>
        private void executeImageUriCommand()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "图片|*.jpg;*.png;*.gif;*.jpeg;*.bmp|所有文件(*.*)|*.*";
            openFileDialog.InitialDirectory = ConstValue.AppPath;
            if (openFileDialog.ShowDialog() == true)
            {
                UiMessage = string.Empty;
                CurrentDataModel.ImageUri = openFileDialog.FileName;
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
