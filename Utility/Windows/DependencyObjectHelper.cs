using System.Windows;
using System.Windows.Media;

namespace Utility.Windows
{
    /// <summary>
    /// 依赖对象帮助类
    /// </summary>
    public class DependencyObjectHelper
    {
        /// <summary>
        /// 查找依赖对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
            {
                if (source is Visual || source is System.Windows.Media.Media3D.Visual3D)
                    source = VisualTreeHelper.GetParent(source);
            }
            return source;

        }
    }
}
