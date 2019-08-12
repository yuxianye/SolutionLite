using SuperSocket.SocketBase.Protocol;
using System;

namespace SocketHelper
{
    /// <summary>
    /// 请求信息
    /// </summary>
    public class RequestInfo : Core.Disposable, IRequestInfo
    {
        /// <summary>
        /// [不使用]
        /// </summary>
        public string Key { get; set; }


        private string message;
        /// <summary>
        /// 电文号C6,2+2+2，两位发送者+两位接受者+两位电文功能码
        /// <para>发送者接受者代码10/26</para>
        /// <para>10：MES L3,三级系统代码</para>
        /// <para>26：SCA L2，二级系统代码</para>
        /// <para>功能代码01/02/03/04/05/06/07/08/11/12/13/14</para>
        /// <para>01：生产命令数据请求电文</para>
        /// <para>02：PDI数据电文</para>
        /// <para>03：生产命令应答电文</para>
        /// <para>04：铣削计划删除</para>
        /// <para>05：L3铣削计划删除应答电文</para>
        /// <para>06：L2铣削计划删除</para>
        /// <para>07：铣削实绩电文</para>
        /// <para>08：铣削实绩应答电文</para>
        /// <para>11：停机实绩电文</para>
        /// <para>12：班报实绩</para>
        /// <para>13：异常数据</para>
        /// <para>14：异常数据应答</para>
        /// </summary>
        public String MessageID
        {
            get
            {
                return message;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (value.Length > 6)
                    {
                        throw new Exception("MessageID长度不能大于6！");
                    }
                }
                message = value;
            }
        }

        private string dateOfSending;
        /// <summary>
        /// 电文发送日期C8,年月日20180312
        /// </summary>
        public String DateOfSending
        {
            get
            {
                return dateOfSending;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (value.Length > 8)
                    {
                        throw new Exception("DateOfSending长度不能大于8！");
                    }
                }
                dateOfSending = value;
            }
        }

        private string timeOfSending;
        /// <summary>
        /// 电文发送时间C6，24小时制231659
        /// </summary>
        public String TimeOfSending
        {
            get
            {
                return timeOfSending;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (value.Length > 6)
                    {
                        throw new Exception("TimeOfSending长度不能大于6！");
                    }
                }
                timeOfSending = value;
            }
        }

        private string sender;
        /// <summary>
        /// 发送者C2，发送端主机描述码
        /// </summary>
        public String Sender
        {
            get
            {
                return sender;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (value.Length > 2)
                    {
                        throw new Exception("Sender长度不能大于2！");
                    }
                }
                sender = value;
            }
        }

        private string receiver;
        /// <summary>
        /// 接收者C2，接收端主机的描述码
        /// </summary>
        public String Receiver
        {
            get
            {
                return receiver;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (value.Length > 2)
                    {
                        throw new Exception("Receiver长度不能大于2！");
                    }
                }
                receiver = value;
            }
        }

        private string functionCode;
        /// <summary>
        /// 传送功能码C1，表示电文的目的，大些字母Db表示发送数据
        /// </summary>
        public String FunctionCode
        {
            get
            {
                return functionCode;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (value.Length > 1)
                    {
                        throw new Exception("FunctionCode长度不能大于2！");
                    }
                }
                functionCode = value;
            }
        }

        private string length;
        /// <summary>
        /// 电文长度C4，整个电文长度，包含结束符
        /// </summary>
        public String Length
        {
            get
            {
                return length;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (value.Length > 4)
                    {
                        throw new Exception("Length长度不能大于4！");
                    }
                }
                length = value;
            }
        }

        //private string generateTime;
        ///// <summary>
        ///// 电文产生时间C17，年月日 时分秒 yyyyMMdd HHmmss
        ///// </summary>
        //public String GenerateTime
        //{
        //    get
        //    {
        //        return generateTime;
        //    }
        //    set
        //    {
        //        if (!string.IsNullOrWhiteSpace(value))
        //        {
        //            if (value.Length > 4)
        //            {
        //                throw new Exception("GenerateTime长度不能大于17！");
        //            }
        //        }
        //        generateTime = value;
        //    }
        //}

        /// <summary>
        /// 消息体C17+C4000+C1,内容域允许ASCII码（0x20－0x7e）和国标码（0xa1a1－0xf7fe）,含结束符0x0a
        /// </summary>
        public byte[] MessageBody { get; set; }

        protected override void Disposing()
        {
            this.Key = null;
            this.MessageID = null;
            this.DateOfSending = null;
            this.Sender = null;
            this.Receiver = null;
            this.FunctionCode = null;
            this.Length = null;
            this.MessageBody = null;
        }
    }
}
