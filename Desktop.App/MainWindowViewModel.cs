using Desktop.Core;
using System.Windows.Input;

namespace Desktop.App
{
    /// <summary>
    /// 主窗体VM
    /// </summary>
    public class MainWindowViewModel : Desktop.Core.ViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindowViewModel()
        {
            initCommand();
        }

        /// <summary>
        /// 初始化命令
        /// </summary>
        private void initCommand()
        {
            ModifyPasswordCommand = new Prism.Commands.DelegateCommand(OnExecuteModifyPasswordCommand);
            SettingCommand = new Prism.Commands.DelegateCommand(OnExecuteSettingCommand);
            HelperCommand = new Prism.Commands.DelegateCommand(OnExecuteHelperCommand);
        }

        #region 修改密码相关
        /// <summary>
        /// 修改密码
        /// </summary>
        public ICommand ModifyPasswordCommand { get; private set; }

        /// <summary>
        /// 执行修改密码命令
        /// </summary>
        private void OnExecuteModifyPasswordCommand()
        {
            EventAggregator.GetEvent<NavigateEvent>().Publish(new ViewInfo(ViewType.Popup, new Models.Module() { Name = "修改密码", AssemblyName = "Desktop.UserModule", ViewName = "Desktop.UserModule.Views.ModifyPasswordView" }));

        }
        #endregion

        #region 设置命令相关

        /// <summary>
        /// 设置命令
        /// </summary>
        public ICommand SettingCommand { get; private set; }
        /// <summary>
        /// 执行设置命令
        /// </summary>
        private void OnExecuteSettingCommand()
        {
            EventAggregator.GetEvent<NavigateEvent>().Publish(new ViewInfo(ViewType.Popup, new Models.Module() { Name = "系统设置", AssemblyName = "Desktop.SettingModule", ViewName = "Desktop.SettingModule.Views.SettingView" }));
        }

        #endregion

        #region 帮助命令相关

        /// <summary>
        /// 帮助命令
        /// </summary>
        public ICommand HelperCommand { get; private set; }
        /// <summary>
        /// 执行帮助命令
        /// </summary>
        private void OnExecuteHelperCommand()
        {
            EventAggregator.GetEvent<NavigateEvent>().Publish(new ViewInfo(ViewType.SingleWindow, new Models.Module() { Name = "系统帮助", AssemblyName = "Desktop.HelpModule", ViewName = "Desktop.HelpModule.Views.HelpView" }));
        }
        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Disposing()
        {
            //释放相关的资源

        }
    }

}
