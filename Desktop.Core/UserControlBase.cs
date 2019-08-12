using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Desktop.Core
{
    /// <summary>
    /// 用户控件的基类
    /// </summary>
    public class UserControlBase : UserControl, IDisposable
    {
        public UserControlBase()
        {
            this.SetResourceReference(BackgroundProperty, "ControlBackgroundBrush");
        }

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
        ~UserControlBase()
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
            //var dispose = this.DataContext as IDisposable;
            //dispose?.Dispose();
            disposeRes(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="obj"></param>
        private void disposeRes(DependencyObject dependencyObject)
        {
            DependencyObject child;
            int count = VisualTreeHelper.GetChildrenCount(dependencyObject);
            for (int i = count - 1; i >= 0; i--)
            {
                child = VisualTreeHelper.GetChild(dependencyObject, i);
                //直接释放
                var uc = child as IDisposable;
                uc?.Dispose();
                //内容控件释放
                var uc2 = child as ContentControl;
                var uc2Content = uc2?.Content as IDisposable;
                uc2Content?.Dispose();
                //控件数据库上下文释放
                var uc3 = child as Control;
                var uc3Content = uc3?.DataContext as IDisposable;
                uc3Content?.Dispose();

                if (child is DependencyObject)
                    disposeRes(child);
            }
        }
    }
}
