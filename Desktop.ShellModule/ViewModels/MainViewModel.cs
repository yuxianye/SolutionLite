using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.ShellModule.ViewModels
{
    /// <summary>
    /// 程序壳VM
    /// </summary>
    public class ShellViewModel : Desktop.Core.ViewModelBase//, INavigationAware
    {

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
