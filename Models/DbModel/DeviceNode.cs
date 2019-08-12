using System;
using System.Linq;
using System.Text;

namespace Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class DeviceNode
    {
           public DeviceNode(){


           }
           /// <summary>
           /// Desc:GUID主键
           /// Default:
           /// Nullable:False
           /// </summary>           
           public Guid Id {get;set;}

           /// <summary>
           /// Desc:OPC服务器GUID主键（与Name组合唯一）
           /// Default:
           /// Nullable:False
           /// </summary>           
           public Guid OpcServerId {get;set;}

           /// <summary>
           /// Desc:节点名称（与OPC ServerId组合唯一）
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Name {get;set;}

           /// <summary>
           /// Desc:更新频率
           /// Default:100
           /// Nullable:False
           /// </summary>           
           public int UpdateRate {get;set;}

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
