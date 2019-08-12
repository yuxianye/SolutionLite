using System;
using System.Windows;
using System.Windows.Controls;

namespace Desktop.PublicControl
{
    /// <summary>
    /// 图片按钮
    /// </summary>
    public class ImageButton : Button, IDisposable
    {
        static ImageButton()
        {
            var uri = new Uri(@"pack://application:,,,/Desktop.PublicControl;component/ImageButton/ImageButton.xaml", UriKind.RelativeOrAbsolute);
            ResourceDictionary res = new ResourceDictionary { Source = uri };
            Application.Current.Resources.MergedDictionaries.Add(res);
        }

        /// <summary>
        /// 图标
        /// </summary>
        public Control Icon { get; set; }

        #region IDisposable

        private bool _disposed;

        /// <summary>
        /// 释放对象，用于外部调用
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放当前对象时释放资源
        /// </summary>
        ~ImageButton()
        {
            Dispose(false);
        }

        /// <summary>
        /// 重写以实现释放对象的逻辑
        /// </summary>
        /// <param name="disposing">是否要释放对象</param>
        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                Disposing();
            }
            _disposed = true;
        }

        /// <summary>
        /// 重写以实现释放派生类资源的逻辑
        /// </summary>
        protected virtual void Disposing()
        {

        }

        #endregion
    }
}