namespace Desktop.HeaderMenuModule.ViewModels
{
    /// <summary>
    /// 头部菜单VM
    /// </summary>
    public class HeaderMenuViewModel : Desktop.Core.DisposableBindableBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HeaderMenuViewModel()
        {

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
