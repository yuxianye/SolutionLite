using System;
using System.Linq;
using System.Text;

namespace Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class Equipment
    {
           public Equipment(){


           }
           /// <summary>
           /// Desc:GUID主键
           /// Default:newid()
           /// Nullable:False
           /// </summary>           
           public Guid Id {get;set;}

           /// <summary>
           /// Desc:设备编码（唯一）
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Code {get;set;}

           /// <summary>
           /// Desc:设备名称
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Name {get;set;}

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
           /// Desc:最后修改人
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
