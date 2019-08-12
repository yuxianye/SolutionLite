using Desktop.Core;
using MahApps.Metro;
using MahApps.Metro.Controls;
using Models;
using Prism.Commands;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

namespace Desktop.SettingModule.ViewModels
{
    /// <summary>
    /// 设置VM
    /// </summary>
    public class SettingViewModel : Desktop.Core.ViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SettingViewModel()
        {
            int.TryParse(Utility.ConfigHelper.GetAppSetting("PageSize"), out pageSize);
#if DEBUG
            ConnectionString = Utility.ConfigHelper.GetAppSetting("ConnectionStringDebug");
#else
            ConnectionString = Utility.Security.AESDecrypt(Utility.ConfigHelper.GetAppSetting("ConnectionString"));
#endif
            SelectedAppTheme = ThemeManager.AppThemes.FirstOrDefault(a => a.Name == Utility.ConfigHelper.GetAppSetting("Theme"));
            SelectedAccent = ThemeManager.Accents.FirstOrDefault(a => a.Name == Utility.ConfigHelper.GetAppSetting("Accent"));
            initCommand();
        }

        /// <summary>
        /// 数据访问上下文
        /// </summary>
        private Dal.DbContext dbContext = new Dal.DbContext();

        #region 连接地址
        private string connectionString;

        /// <summary>
        /// 连接地址
        /// </summary>
        public string ConnectionString
        {
            get { return connectionString; }
            set { SetProperty(ref connectionString, value); }
        }
        #endregion

        #region ConnectString界面信息
        private string connectStringUiMessage;

        /// <summary>
        /// connectStringUiMessage界面信息
        /// </summary>
        public string ConnectStringUiMessage
        {
            get { return connectStringUiMessage; }
            set { SetProperty(ref connectStringUiMessage, value); }
        }
        #endregion

        #region PageSize界面信息
        private string pageSizeUiMessage;

        /// <summary>
        /// PageSize界面信息
        /// </summary>
        public string PageSizeUiMessage
        {
            get { return pageSizeUiMessage; }
            set { SetProperty(ref pageSizeUiMessage, value); }
        }
        #endregion

        #region PageSize
        private int pageSize = 200;

        /// <summary>
        /// PageSize
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { SetProperty(ref pageSize, value); }
        }
        #endregion

        #region PageSizeList
        private List<int> pageSizeList = new List<int>() { 20, 50, 100, 200, 500, 1000, 5000, 10000 };

        /// <summary>
        /// PageSize
        /// </summary>
        public List<int> PageSizeList
        {
            get { return pageSizeList; }
            set { SetProperty(ref pageSizeList, value); }
        }
        #endregion




        public IEnumerable<AppTheme> AppThemes { get; set; } = ThemeManager.AppThemes;

        private AppTheme selectedAppTheme;

        public AppTheme SelectedAppTheme
        {
            get { return selectedAppTheme; }
            set
            {
                SetProperty(ref selectedAppTheme, value);
                var theme = ThemeManager.DetectAppStyle(Application.Current);
                var appTheme = ThemeManager.GetAppTheme(value.Name);
                ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, appTheme);
            }
        }


        public IEnumerable<Accent> AccentList { get; set; } = ThemeManager.Accents;

        private Accent selectedAccent;

        public Accent SelectedAccent
        {
            get { return selectedAccent; }
            set
            {
                SetProperty(ref selectedAccent, value);
                var theme = ThemeManager.DetectAppStyle(Application.Current);
                var accent = ThemeManager.GetAccent(value.Name);
                ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);
            }
        }



        #region 命令定义和初始化

        /// <summary>
        /// 数据库连接确定命令
        /// </summary>
        public DelegateCommand ConnectionStringConfirmCommand { get; set; }

        /// <summary>
        /// 页面确定命令
        /// </summary>
        public DelegateCommand PageSizeConfirmCommand { get; set; }

        /// <summary>
        /// 主题确定命令
        /// </summary>
        public DelegateCommand ThemeCommand { get; set; }


        /// <summary>
        /// 关闭命令
        /// </summary>
        public DelegateCommand CancelCommand { get; set; }

        /// <summary>
        /// 初始化命令,关闭命令始终可用。
        /// </summary>
        private void initCommand()
        {
            ConnectionStringConfirmCommand = new DelegateCommand(executeConnectStringConfirmCommand);
            PageSizeConfirmCommand = new DelegateCommand(executePageSizeConfirmCommand);
            ThemeCommand = new DelegateCommand(executeThemeCommand);

            CancelCommand = new DelegateCommand(executeCancelCommand);
        }

        #endregion

        #region 命令和消息等执行函数

        private void executeConnectStringConfirmCommand()
        {
            try
            {
#if DEBUG
                Utility.ConfigHelper.AddAppSetting("ConnectionStringDebug", ConnectionString);

#else
                Utility.ConfigHelper.AddAppSetting("ConnectionString", Utility.Security.AESEncrypt(ConnectionString));
#endif

                ConnectStringUiMessage = "保存成功，重启程序后生效";
            }
            catch (Exception ex)
            {
                ConnectStringUiMessage = "修改设置失败！请与管理员联系！" + System.Environment.NewLine + ex.Message;
                LogHelper.Logger.Error(UiMessage, ex);
            }
        }

        /// <summary>
        /// 执行确认命令
        /// </summary>
        private void executePageSizeConfirmCommand()
        {
            try
            {
                Utility.ConfigHelper.AddAppSetting("PageSize", PageSize.ToString());
                PageSizeUiMessage = "保存成功";
            }
            catch (Exception ex)
            {
                PageSizeUiMessage = "修改设置失败！请与管理员联系！" + System.Environment.NewLine + ex.Message;
                LogHelper.Logger.Error(UiMessage, ex);
            }
        }

        /// <summary>
        /// 执行主题确认命令
        /// </summary>
        private void executeThemeCommand()
        {
            try
            {
                Utility.ConfigHelper.AddAppSetting("Theme", SelectedAppTheme.Name);
                Utility.ConfigHelper.AddAppSetting("Accent", SelectedAccent.Name);
                PageSizeUiMessage = "保存成功";
            }
            catch (Exception ex)
            {
                PageSizeUiMessage = "修改设置失败！请与管理员联系！" + System.Environment.NewLine + ex.Message;
                LogHelper.Logger.Error(UiMessage, ex);
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
            dbContext.Db.Close();
            dbContext.Db.Dispose();
            dbContext = null;
            LogHelper.Logger.Debug($"释放资源：{this.ToString()}");
        }

    }

}
