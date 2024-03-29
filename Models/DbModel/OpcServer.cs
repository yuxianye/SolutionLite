﻿using System;
using System.Linq;
using System.Text;

namespace Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class OpcServer
    {
           public OpcServer(){


           }
           /// <summary>
           /// Desc:GUID主键
           /// Default:newid()
           /// Nullable:False
           /// </summary>           
           public Guid Id {get;set;}

           /// <summary>
           /// Desc:名称（唯一）
           /// Default:OPC Server
           /// Nullable:False
           /// </summary>           
           public string Name {get;set;}

           /// <summary>
           /// Desc:Uri地址（唯一）
           /// Default:Kepware.KEPServerEX.V6
           /// Nullable:False
           /// </summary>           
           public string Uri {get;set;}

           /// <summary>
           /// Desc:是否启用
           /// Default:1
           /// Nullable:False
           /// </summary>           
           public bool IsEnable {get;set;}

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
