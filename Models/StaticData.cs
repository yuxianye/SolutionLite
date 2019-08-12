using System.Collections.Generic;

namespace Models
{
    /// <summary>
    /// 全局静态数据
    /// </summary>
    public class StaticData
    {
        /// <summary>
        /// 处理页面的延时时间
        /// </summary>
        public const int Delay = 2000;

        /// <summary>
        /// 当前登录用户信息
        /// </summary>
        public static User CurrentUser { get; set; }

        /// <summary>
        /// 当前用户可用的模块
        /// </summary>
        public static readonly List<Module> Module = new List<Module>();
    }
}
