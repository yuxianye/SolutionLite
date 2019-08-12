using System;
using System.Linq;
using System.Text;

namespace Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class AppLog
    {
           public AppLog(){


           }
           /// <summary>
           /// Desc:编号
           /// Default:
           /// Nullable:False
           /// </summary>           
           public long Id {get;set;}

           /// <summary>
           /// Desc:时间
           /// Default:
           /// Nullable:False
           /// </summary>           
           public DateTime CreatedTime {get;set;}

           /// <summary>
           /// Desc:等级
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Level {get;set;}

           /// <summary>
           /// Desc:线程号
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public int ThreadId {get;set;}

           /// <summary>
           /// Desc:信息
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Message {get;set;}

           /// <summary>
           /// Desc:来源
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string CallSite {get;set;}

           /// <summary>
           /// Desc:异常
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Exception {get;set;}

           /// <summary>
           /// Desc:堆栈
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string StackTrace {get;set;}

    }
}
