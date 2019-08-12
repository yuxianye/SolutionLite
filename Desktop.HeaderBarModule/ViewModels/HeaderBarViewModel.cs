using System.Windows;
using System.Windows.Input;

namespace Desktop.HeaderBarModule.ViewModels
{
    /// <summary>
    /// 头部VM
    /// </summary>
    public class HeaderBarViewModel : Desktop.Core.DisposableBindableBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HeaderBarViewModel()
        {
            MoveCommand = new Prism.Commands.DelegateCommand(executeMoveCommand);
        }

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

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Disposing()
        {
            //释放相关的资源
            LogHelper.Logger.Debug($"释放资源：{this.ToString()}");
        }
    }

}
