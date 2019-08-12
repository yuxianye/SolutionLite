using Models;
using System;
using System.Linq;
using System.Windows;
using System.Text;
namespace Desktop.StatusBarModule.ViewModels
{
    /// <summary>
    /// 状态栏VM
    /// </summary>
    public class StatusBarViewModel : Desktop.Core.ViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StatusBarViewModel()
        {
            CurrentUser = StaticData.CurrentUser;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
        }
        /// <summary>
        /// 调度计时器
        /// </summary>
        private System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        /// <summary>
        /// 数据访问对象
        /// </summary>
        private Dal.DbContext dbContext = new Dal.DbContext();
        /// <summary>
        /// 公告文字
        /// </summary>
        private StringBuilder noticeBuilder = new StringBuilder();
        /// <summary>
        /// 更新时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [System.Diagnostics.DebuggerStepThrough()]
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            this.CurrentTime = dbContext.Db.GetDate();

            foreach (var v in dbContext.NoticeDb.GetList(a => a.IsEnable == true)?.OrderByDescending(a => a.OrderCode))
            {
                noticeBuilder?.Append(v?.Content);
            }
            UiMessage = noticeBuilder.ToString();
            noticeBuilder.Clear();
            if (string.IsNullOrWhiteSpace(UiMessage))
            {
                Application.Current.Resources["NoticeMessage"] = string.Empty;
            }
            else
            {
                Application.Current.Resources["NoticeMessage"] = UiMessage;
            }
        }

        #region 当前用户
        private User user;

        public User CurrentUser
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }
        #endregion

        #region 版本

        private string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        /// <summary>
        /// 当前版本
        /// </summary>
        public string Version
        {
            get { return version; }
            set { SetProperty(ref version, value); }
        }
        #endregion

        #region 时间

        private DateTime currentTime = DateTime.Now;
        /// <summary>
        /// 当前版本
        /// </summary>
        public DateTime CurrentTime
        {
            get { return currentTime; }
            set { SetProperty(ref currentTime, value); }
        }
        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Disposing()
        {
            //释放相关的资源
            Version = null;
            dispatcherTimer.Stop();
            dbContext.Db.Close();
            dbContext.Db.Dispose();
            dbContext = null;
            LogHelper.Logger.Debug($"释放资源：{this.ToString()}");
        }
    }

}
