namespace Desktop.Core
{
    /// <summary>
    /// 页面信息
    /// </summary>
    public class ViewInfo
    {
        public ViewInfo(ViewType viewType, object module, object parameter = null)
        {
            this.ViewType = viewType;
            this.Module = module;
            this.Parameter = parameter;
        }

        /// <summary>
        /// 页面类型
        /// </summary>
        public ViewType ViewType { get; set; }

        /// <summary>
        /// 模块
        /// </summary>
        public object Module { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public object Parameter { get; set; }

    }

}
