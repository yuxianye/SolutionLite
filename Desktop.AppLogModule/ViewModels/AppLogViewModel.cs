using Desktop.Core;
using Models;
using Prism.Commands;
using SqlSugar;
using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Windows.Input;

namespace Desktop.AppLogModule.ViewModels
{
    /// <summary>
    /// 应用日志VM
    /// </summary>
    public class AppLogViewModel : Desktop.Core.PageableViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppLogViewModel()
        {
            SearchCommand = new DelegateCommand(executeSearchCommand);
            getPageData(1, PageSize);

            int appLogAutoRefreshInterval = 3;
            int.TryParse(Utility.ConfigHelper.GetAppSetting("AppLogAutoRefreshInterval"), out appLogAutoRefreshInterval);
            dispatcherTimer.Interval = new TimeSpan(0, 0, appLogAutoRefreshInterval < 1 ? 1 : appLogAutoRefreshInterval);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
        }

        /// <summary>
        /// 数据访问上下文
        /// </summary>
        private Dal.DbContext dbContext = new Dal.DbContext();

        /// <summary>
        /// 调度计时器
        /// </summary>
        private System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        /// <summary>
        /// 自动更新日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [System.Diagnostics.DebuggerStepThrough()]
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                EndTime = dbContext.Db.GetDate().AddMinutes(1);
                StartTime = EndTime.AddMinutes(-30);
                executeSearchCommand();
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("自动刷新日志时错误！", ex);
            }
        }

        #region 查询命令

        /// <summary>
        /// 查询命令
        /// </summary>
        public ICommand SearchCommand { get; }

        /// <summary>
        /// 执行查询命令事件函数
        /// </summary>
        private void executeSearchCommand()
        {
            getPageData(1, PageSize);
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
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getPageData(int pageIndex, int pageSize)
        {
            UiMessage = "加载数据中...";

            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    Expression<Func<AppLog, bool>> expression;
                    if (SelectLevel == "All")//所有日志
                    {
                        if (string.IsNullOrWhiteSpace(SearchText))//文字空，则全部
                        {
                            expression = (a) =>
                            a.CreatedTime >= StartTime
                            && a.CreatedTime <= EndTime;
                        }
                        else//文字不空，则包含
                        {
                            expression = (a) =>
                           a.CreatedTime >= StartTime
                           && a.CreatedTime <= EndTime
                           && a.Message.Contains(SearchText);
                        }
                    }
                    else//特定级别日志
                    {
                        if (string.IsNullOrWhiteSpace(SearchText))//文字空，则全部
                        {
                            expression = (a) =>
                            a.CreatedTime >= StartTime
                            && a.CreatedTime <= EndTime
                            && a.Level == SelectLevel;
                        }
                        else//文字不空，则包含
                        {
                            expression = (a) =>
                           a.CreatedTime >= StartTime
                           && a.CreatedTime <= EndTime
                           && a.Message.Contains(SearchText)
                           && a.Level == SelectLevel;
                        }
                    }
                    var dbContext = new Dal.DbContext();
                    DataList = new ObservableCollection<AppLog>(dbContext.AppLogDb.GetPageList(expression,
                        new PageModel() { PageIndex = pageIndex, PageSize = pageSize }, a => a.Id, OrderByType.Desc));
                    TotalCounts = dbContext.AppLogDb.Count(expression);
                    UiMessage = "数据加载完成";

                }
                catch (InvalidOperationException iex)
                {
                    UiMessage = $"数据加载时错误，{iex.Message }";
                    LogHelper.Logger.Error(UiMessage, iex);
                    dbContext.Db.Open();
                }
                catch (Exception ex)
                {
                    UiMessage = $"数据加载时错误，{ex.Message }";
                    LogHelper.Logger.Error(UiMessage, ex);
                }
            });
        }

        #region 列表数据
        private ObservableCollection<AppLog> dataList = new ObservableCollection<AppLog>();

        /// <summary>
        /// 数据日志信息信息数据
        /// </summary>
        public ObservableCollection<AppLog> DataList
        {
            get { return dataList; }
            set { SetProperty(ref dataList, value); }
        }
        #endregion

        #region 日志级别列表数据
        private ObservableCollection<string> levelList = new ObservableCollection<string>()
        {
            "All",
            "Debug",
            "Info",
            "Error",
            "Fatal",
            "Warn",
        };

        /// <summary>
        /// 日志级别列表数据
        /// </summary>
        public ObservableCollection<string> LevelList
        {
            get { return levelList; }
            set { SetProperty(ref levelList, value); }
        }
        #endregion

        #region 选择的日志级别
        private string selectLevel = "All";

        /// <summary>
        /// 选择的日志级别
        /// </summary>
        public string SelectLevel
        {
            get { return selectLevel; }
            set { SetProperty(ref selectLevel, value); }
        }
        #endregion


        #region 开始时间
        private DateTime startTime = DateTime.Now.AddHours(-8);

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get { return startTime; }
            set { SetProperty(ref startTime, value); }
        }
        #endregion


        #region 结束时间
        private DateTime endTime = DateTime.Now;

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get { return endTime; }
            set { SetProperty(ref endTime, value); }
        }
        #endregion


        #region 查询的内容文本
        private string searchText;

        /// <summary>
        /// 查询的内容文本
        /// </summary>
        public string SearchText
        {
            get { return searchText; }
            set { SetProperty(ref searchText, value); }
        }
        #endregion


        #region 自动刷新
        private bool isAutoRefresh = false;

        /// <summary>
        /// 自动刷新
        /// </summary>
        public bool IsAutoRefresh
        {
            get { return isAutoRefresh; }
            set
            {

                SetProperty(ref isAutoRefresh, value);

                if (value)
                {

                    dispatcherTimer.Start();
                }
                else
                {
                    dispatcherTimer.Stop();
                }
            }
        }
        #endregion


        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Disposing()
        {
            //释放相关的资源
            levelList?.Clear();
            levelList = null;
            SearchText = null;
            SelectLevel = null;
            DataList?.Clear();
            DataList = null;
            dbContext.Db.Close();
            dbContext.Db.Dispose();
            dbContext = null;
            LogHelper.Logger.Debug($"释放资源：{this.ToString()}");
        }

    }

}
