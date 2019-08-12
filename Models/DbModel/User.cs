using System;
using System.Linq;
using System.Text;

namespace Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class User
    {
           public User(){


           }
           /// <summary>
           /// Desc:GUID主键
           /// Default:newid()
           /// Nullable:False
           /// </summary>           
           public Guid Id {get;set;}

           /// <summary>
           /// Desc:用户名
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Name {get;set;}

           /// <summary>
           /// Desc:昵称、中文名
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string NickName {get;set;}

           /// <summary>
           /// Desc:安全密码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string SecurityPassword {get;set;}

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
