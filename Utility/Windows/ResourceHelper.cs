using System.Windows;

namespace Utility.Windows
{
    /// <summary>
    /// 资源帮助类
    /// </summary>
    public class ResourceHelper
    {
        /// <summary>
        /// 查找字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string LoadString(string key)
        {
            return Application.Current.TryFindResource(key) as string;
        }

        /// <summary>
        /// 查找资源
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object FindResource(string key)
        {
            return Application.Current.TryFindResource(key);
        }
    }
}
