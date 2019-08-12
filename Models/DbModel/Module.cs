using System;
using System.Linq;
using System.Text;

namespace Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class Module
    {
           public Module(){


           }
           /// <summary>
           /// Desc:GUID主键
           /// Default:newid()
           /// Nullable:False
           /// </summary>           
           public Guid Id {get;set;}

           /// <summary>
           /// Desc:模块名称（唯一）
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Name {get;set;}

           /// <summary>
           /// Desc:父节点编号,默认 00000000-0000-0000-0000-000000000000
           /// Default:00000000-0000-0000-0000-000000000000
           /// Nullable:False
           /// </summary>           
           public Guid ParentId {get;set;}

           /// <summary>
           /// Desc:排序码
           /// Default:0
           /// Nullable:True
           /// </summary>           
           public float? OrderCode {get;set;}

           /// <summary>
           /// Desc:是否在导航菜单显示，默认显示
           /// Default:1
           /// Nullable:False
           /// </summary>           
           public bool ShowInNavigateMenu {get;set;}

           /// <summary>
           /// Desc:图标
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Icon {get;set;}

           /// <summary>
           /// Desc:程序集名，例如：Desktop.UserModule
           /// Default:Desktop.UserModule
           /// Nullable:True
           /// </summary>           
           public string AssemblyName {get;set;}

           /// <summary>
           /// Desc:页面名，例如：Desktop.UserModule.Views.UserView
           /// Default:Desktop.UserModule.Views.UserView
           /// Nullable:True
           /// </summary>           
           public string ViewName {get;set;}

           /// <summary>
           /// Desc:创建时间
           /// Default:DateTime.Now
           /// Nullable:True
           /// </summary>           
           public DateTime? CreatedTime {get;set;}

           /// <summary>
           /// Desc:创建人
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string CreatorUser {get;set;}

           /// <summary>
           /// Desc:最后更新时间
           /// Default:DateTime.Now
           /// Nullable:True
           /// </summary>           
           public DateTime? LastUpdatedTime {get;set;}

           /// <summary>
           /// Desc:最后更新人
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string LastUpdatorUser {get;set;}

           /// <summary>
           /// Desc:备注
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Remark {get;set;}

    }
}
