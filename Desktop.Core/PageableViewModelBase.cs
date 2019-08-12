using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace Desktop.Core
{
    /// <summary>
    /// 可分页的vm基类
    /// </summary>
    public abstract class PageableViewModelBase : ViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PageableViewModelBase()
        {
            PageChangedCommand = new Prism.Commands.DelegateCommand<PageChangedEventArgs>(OnExecutePageChangedCommand);

            int.TryParse(Utility.ConfigHelper.GetAppSetting("PageSize"), out pageSize);
        }

        private int pageSize = 200;

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { SetProperty(ref pageSize, value); }

        }

        /// <summary>
        /// 翻页命令,当前页数变化时执行此命令
        /// </summary>
        public ICommand PageChangedCommand { get; private set; }

        #region 当前浏览页面的页码

        private int totalCounts = 0;
        /// <summary>
        /// 所有记录的个数
        /// </summary>
        public int TotalCounts
        {
            get { return totalCounts; }
            set { SetProperty(ref totalCounts, value); }

        }
        #endregion

        /// <summary>
        /// 分页改变时重写此方法，然后根据分页参数从服务端取得数据
        /// </summary>
        public virtual void OnExecutePageChangedCommand(PageChangedEventArgs e)
        {
            PageSize = e.PageSize;
        }
        #region 数据列表的右键快捷菜单

        private List<MenuItem> menuItems = new List<MenuItem>();

        /// <summary>
        /// 右键菜单
        /// </summary>
        public List<MenuItem> MenuItems
        {
            get { return menuItems; }
            set { SetProperty(ref menuItems, value); }
        }
        #endregion
    }
}
