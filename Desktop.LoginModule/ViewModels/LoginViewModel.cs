using Models;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Desktop.LoginModule.ViewModels
{
    /// <summary>
    /// 登陆VM
    /// </summary>
    public class LoginViewModel : Desktop.Core.ViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public LoginViewModel()
        {
            initConfigData();
            initCommand();
        }

        /// <summary>
        /// 数据访问上下文
        /// </summary>
        private Dal.DbContext dbContext = new Dal.DbContext();

        /// <summary>
        /// 初始化配置
        /// </summary>
        private void initConfigData()
        {
            try
            {
                bool.TryParse(Utility.ConfigHelper.GetAppSetting("IsRemberUserName"), out bool _isRemberUserName);
                IsRemberUserName = _isRemberUserName;

                bool.TryParse(Utility.ConfigHelper.GetAppSetting("IsRemberUserPassword"), out bool _isRemberUserPassword);
                IsRemberUserPassword = _isRemberUserPassword;

                CurrentDataModel.Name = Utility.ConfigHelper.GetAppSetting("RemberUserName");

                if (string.IsNullOrWhiteSpace(Utility.ConfigHelper.GetAppSetting("RemberUserPassword")))
                {
                    CurrentDataModel.SecurityPassword = null;
                }
                else
                {
                    CurrentDataModel.SecurityPassword = Utility.Security.AESDecrypt(Utility.ConfigHelper.GetAppSetting("RemberUserPassword"));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("初始化登陆配置文件异常。", ex);
            }
        }

        /// <summary>
        /// 初始化命令
        /// </summary>
        private void initCommand()
        {
            MoveCommand = new Prism.Commands.DelegateCommand(executeMoveCommand);
            LoginCommand = new Prism.Commands.DelegateCommand(executeLoginCommand, canExecuteLoginCommand);
            ExitCommand = new Prism.Commands.DelegateCommand(OnExecuteExitCommand);
        }

        #region 移动命令相关
        /// <summary>
        /// 移动命令
        /// </summary>
        public ICommand MoveCommand { get; private set; }

        /// <summary>
        /// 执行移动命令
        /// </summary>
        private void executeMoveCommand()
        {
            Application.Current.MainWindow.DragMove();
        }
        #endregion

        #region 退出命令相关
        /// <summary>
        /// 退出命令
        /// </summary>
        public ICommand ExitCommand { get; private set; }

        /// <summary>
        /// 退出命令执行函数
        /// </summary>
        private void OnExecuteExitCommand()
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region 登陆命令相关
        /// <summary>
        /// 登陆命令
        /// </summary>
        public DelegateCommand LoginCommand { get; private set; }





        /// <summary>
        /// 登陆命令执行函数
        /// </summary>
        private void executeLoginCommand()
        {
            AuthenticateModel authenticateModel = new Models.AuthenticateModel()
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe",
                RememberClient = true,
            };

            var obj = Utility.HttpClientHelper.PostResponse<AjaxResponse<AuthenticateResultModel>>(
                Utility.ConfigHelper.GetAppSetting("ApiUri") + @"/TokenAuth/Authenticate",
                Utility.JsonHelper.ToJson(authenticateModel));


            //() => (Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MenuModule>>>(GlobalData.ServerRootUri + "Identity/Login", Utility.JsonHelper.ToJson(LoginUser)))











            isCanExecuteLoginCommand = false;
            LoginCommand.RaiseCanExecuteChanged();










            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(CurrentDataModel.Name))
                    {
                        UiMessage = "请输入用户名";
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(CurrentDataModel.SecurityPassword))
                    {
                        UiMessage = "请输入密码";
                        return;
                    }
                    UiMessage = "正在核对用户名和密码...";
                    var user = dbContext.UserDb.GetSingle(a => a.Name == CurrentDataModel.Name.Trim());
                    if (Equals(user, null))
                    {
                        UiMessage = $"未找到用户{CurrentDataModel.Name}";
                    }
                    else//找到用户，验证密码
                    {
                        if (CurrentDataModel.SecurityPassword.Trim() == Utility.Security.AESDecrypt(user.SecurityPassword))
                        {
                            UiMessage = "登陆成功，正在加载...";

                            LogHelper.Logger.Info($"{user?.Name}/{user?.NickName},登陆成功");

                            StaticData.CurrentUser = user;
                            Utility.ConfigHelper.AddAppSetting("IsRemberUserName", IsRemberUserName.ToString());
                            Utility.ConfigHelper.AddAppSetting("IsRemberUserPassword", IsRemberUserPassword.ToString());

                            if (IsRemberUserName)
                            {
                                Utility.ConfigHelper.AddAppSetting("RemberUserName", CurrentDataModel.Name);
                            }
                            else
                            {
                                Utility.ConfigHelper.AddAppSetting("RemberUserName", "");
                            }
                            if (IsRemberUserPassword)
                            {
                                Utility.ConfigHelper.AddAppSetting("RemberUserPassword", Utility.Security.AESEncrypt(CurrentDataModel.SecurityPassword.Trim()));
                            }
                            else
                            {
                                Utility.ConfigHelper.AddAppSetting("RemberUserPassword", "");
                            }

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                regionManager.RequestNavigate("ShellContentRegion", "ShellView");
                                Application.Current.Resources["TitlebarHeight"] = Utility.Windows.ResourceHelper.FindResource("TitlebarHeight2");
                                Application.Current.MainWindow.WindowState = WindowState.Maximized;
                                Application.Current.MainWindow.ResizeMode = ResizeMode.CanResize;
                            });
                        }
                        else
                        {
                            UiMessage = $"密码错误！";
                        }
                    }
                }
                catch (Exception ex)
                {
                    UiMessage = ex.Message;
                    LogHelper.Logger.Error($"{CurrentDataModel.Name}登陆异常！请与管理员联系！", ex);
                }
                finally
                {
                    isCanExecuteLoginCommand = true;
                    LoginCommand.RaiseCanExecuteChanged();
                }
            });

        }

        /// <summary>
        /// 登陆按钮是否可用
        /// </summary>
        private bool isCanExecuteLoginCommand = true;

        /// <summary>
        /// 执行登陆按钮是否可用
        /// </summary>
        /// <returns></returns>
        private bool canExecuteLoginCommand()
        {
            return isCanExecuteLoginCommand;
        }

        #endregion

        /// <summary>
        /// 区域管理
        /// </summary>
        private IRegionManager regionManager = CommonServiceLocator.ServiceLocator.Current.GetInstance<IRegionManager>();

        #region 用户实体
        private LoginUserModel currentDataModel = new LoginUserModel()
        {
#if DEBUG
            Name = "admin",
            SecurityPassword = "yuxianye1"
#endif
        };

        /// <summary>
        /// 用户实体
        /// </summary>
        public LoginUserModel CurrentDataModel
        {
            get { return currentDataModel; }
            set { SetProperty(ref currentDataModel, value); }
        }
        #endregion

        #region 是否记住用户名
        private bool isRemberUserName = true;

        /// <summary>
        /// 是否记住用户名
        /// </summary>
        public bool IsRemberUserName
        {
            get { return isRemberUserName; }
            set
            {
                SetProperty(ref isRemberUserName, value);
                if (!value) IsRemberUserPassword = false;
            }
        }
        #endregion

        #region 是否记住用户密码
        private bool isRemberUserPassword = true;

        /// <summary>
        /// 是否记住用户密码
        /// </summary>
        public bool IsRemberUserPassword
        {
            get { return isRemberUserPassword; }
            set
            {
                SetProperty(ref isRemberUserPassword, value);
                if (value) IsRemberUserName = true;
            }
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

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Disposing()
        {
            //释放相关的资源
            UiMessage = null;
            CurrentDataModel = null;
            dbContext.Db.Close();
            dbContext.Db.Dispose();
            dbContext = null;
            LogHelper.Logger.Debug($"释放资源：{this.ToString()}");
        }
    }

}
