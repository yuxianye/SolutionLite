using System;
using System.Windows;

namespace Desktop.App
{
    /// <summary>
    /// 主窗体
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.Closed += MainWindow_Closed;
            this.Loaded += MetroWindow_Loaded;
        }

        /// <summary>
        /// 应用、窗体关闭时释放资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            var dispose = this.mainContentControl.Content as IDisposable;
            dispose?.Dispose();
        }

        /// <summary>
        ///窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowButtonCommands.MinHeight = 30;
            this.WindowButtonCommands.VerticalAlignment = VerticalAlignment.Top;
        }
    }
}
