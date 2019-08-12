using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Desktop.Core
{

    /// <summary>
    /// 可分页的vm基类
    /// </summary>
    public abstract class ViewModelBase : DisposableBindableBase
    {
        public ViewModelBase()
        {
            EventAggregator = CommonServiceLocator.ServiceLocator.Current.GetInstance<Prism.Events.IEventAggregator>();
            Parameter.CollectionChanged += Parameter_CollectionChanged;
        }

        private void Parameter_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnParameterChanged(sender, e);
        }

        protected virtual void OnParameterChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }

        /// <summary>
        /// 特殊参数,弹出对话框通常是MetroWindow,修改窗体等是模型对象
        /// </summary>
        public ObservableCollection<KeyValuePair<string, object>> Parameter { get; set; } = new ObservableCollection<KeyValuePair<string, object>>();

        /// <summary>
        /// 父控件的名称
        /// </summary>
        protected const string ParentName = "Parent";

        /// <summary>
        /// 数据模型的名称
        /// </summary>
        protected const string DataModelName = "DataModel";

        /// <summary>
        /// 事件聚合器
        /// </summary>
        protected IEventAggregator EventAggregator { get; set; }

        #region 界面信息
        private string uiMessage;

        /// <summary>
        /// 界面信息
        /// </summary>
        public string UiMessage
        {
            get { return uiMessage; }
            set { SetProperty(ref uiMessage, value); }
        }
        #endregion
    }
}
