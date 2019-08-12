using Prism.Events;

namespace Desktop.Core
{
    /// <summary>
    /// 导航事件，打开docker\popup\metro popup等页面
    /// </summary>
    public class NavigateEvent : PubSubEvent<ViewInfo>
    {

    }

    /// <summary>
    /// 关闭对话框事件，关闭popup页面
    /// </summary>
    public class ClosePopupEvent : PubSubEvent<object>
    {

    }

    /// <summary>
    /// 更新列表数据事件
    /// </summary>
    public class UpListDataEvent : PubSubEvent<object>
    {

    }


}
