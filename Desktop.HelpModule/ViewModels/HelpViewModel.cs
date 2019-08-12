using Desktop.Core;
using Models;
using Prism.Commands;
using SqlSugar;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq.Expressions;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Xps.Packaging;

namespace Desktop.HelpModule.ViewModels
{
    /// <summary>
    /// 帮助VM
    /// </summary>
    public class HelpViewModel : Desktop.Core.ViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HelpViewModel()
        {
            loadDoc();
        }

        ///// <summary>
        ///// 数据访问上下文
        ///// </summary>
        //private static readonly Dal.DbContext dbContext = new Dal.DbContext();

        #region 帮助文件
        private IDocumentPaginatorSource helpDocument;
        /// <summary>
        /// 帮助文件
        /// </summary>
        public IDocumentPaginatorSource HelpDocument
        {
            get { return helpDocument; }
            set { SetProperty(ref helpDocument, value); }
        }
        #endregion

        /// <summary>
        /// 加载帮助文件
        /// </summary>
        private void loadDoc()
        {
            try
            {
                var xpsPath = Utility.ConfigHelper.GetAppSetting("HelpDocumentName");
                HelpDocument = new XpsDocument(System.IO.Path.Combine(Utility.ConstValue.AppPath, xpsPath), FileAccess.Read).GetFixedDocumentSequence();
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error($"加载帮助文件时错误", ex);
            }
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
