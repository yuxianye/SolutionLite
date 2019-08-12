using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class Module : ModelDalBase
    {
        #region 模块名称（唯一）
        private string name;
        /// <summary>
        /// Desc:模块名称（唯一）
        /// Default:
        /// Nullable:False
        /// </summary>  
        [Required(ErrorMessage = "必填项，不能重复，长度小于50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        #endregion

        #region 父节点编号,默认 00000000-0000-0000-0000-000000000000
        private Guid parentId;
        /// <summary>
        /// Desc:父节点编号,默认 00000000-0000-0000-0000-000000000000
        /// Default:00000000-0000-0000-0000-000000000000
        /// Nullable:False
        /// </summary> 
        public Guid ParentId
        {
            get { return parentId; }
            set { SetProperty(ref parentId, value); }
        }
        #endregion

        #region 排序码
        private float? orderCode = 0;
        /// <summary>
        /// Desc:排序码
        /// Default:0
        /// Nullable:True
        /// </summary>        
        public float? OrderCode
        {
            get { return orderCode; }
            set { SetProperty(ref orderCode, value); }
        }
        #endregion

        #region 是否在导航菜单显示，默认显示
        private bool showInNavigateMenu;
        /// <summary>
        /// Desc:是否在导航菜单显示，默认显示
        /// Default:1
        /// Nullable:False
        /// </summary>    
        public bool ShowInNavigateMenu
        {
            get { return showInNavigateMenu; }
            set { SetProperty(ref showInNavigateMenu, value); }
        }
        #endregion

        #region 图标
        private string icon;
        /// <summary>
        /// Desc:图标
        /// Default:
        /// Nullable:True
        /// </summary>    
        [Required(ErrorMessage = "必填项，长度小于100个字符"), MaxLength(100, ErrorMessage = "长度小于100个字符")]
        public string Icon
        {
            get { return icon; }
            set { SetProperty(ref icon, value); }
        }
        #endregion

        #region 程序集名，例如：Desktop.UserModule
        private string assemblyName;
        /// <summary>
        /// Desc:程序集名，例如：Desktop.UserModule
        /// Default:Desktop.UserModule
        /// Nullable:True
        /// </summary> 
        [Required(ErrorMessage = "必填项，长度小于100个字符"), MaxLength(100, ErrorMessage = "长度小于100个字符")]
        public string AssemblyName
        {
            get { return assemblyName; }
            set { SetProperty(ref assemblyName, value); }
        }
        #endregion

        #region 页面名，例如：Desktop.UserModule.Views.UserView
        private string viewName;
        /// <summary>
        /// Desc:页面名，例如：Desktop.UserModule.Views.UserView
        /// Default:Desktop.UserModule.Views.UserView
        /// Nullable:True
        /// </summary>      
        [Required(ErrorMessage = "必填项，长度小于100个字符"), MaxLength(100, ErrorMessage = "长度小于100个字符")]
        public string ViewName
        {
            get { return viewName; }
            set { SetProperty(ref viewName, value); }
        }
        #endregion

    }
}
