using SuperSocket.SocketBase.Protocol;
using System;

namespace SocketHelper.TelnetServer
{
    /// <summary>
    /// 服务端请求事件参数
    /// </summary>
    public class RequestEventArgs : EventArgs
    {

        public RequestEventArgs()
        {
        }

        public RequestEventArgs(System.Net.IPEndPoint iPEndPoint, StringRequestInfo requestInfo)
        {
            IPEndPoint = iPEndPoint;
            RequestInfo = requestInfo;
        }

        /// <summary>
        /// 客户端的地址
        /// </summary>
        public System.Net.IPEndPoint IPEndPoint { get; set; }

        /// <summary>
        /// 客户端的请求信息
        /// </summary>
        public StringRequestInfo RequestInfo { get; set; }
    }


    /// <summary>
    /// 服务端接收到客户端连接事件参数
    /// </summary>
    public class ConnectEventArgs : EventArgs
    {
        public ConnectEventArgs()
        {
        }

        public ConnectEventArgs(System.Net.IPEndPoint iPEndPoint)
        {
            IPEndPoint = iPEndPoint;
        }

        /// <summary>
        /// 客户端的地址
        /// </summary>
        public System.Net.IPEndPoint IPEndPoint { get; set; }
    }

    /// <summary>
    /// 服务端接收到客户端关闭事件参数
    /// </summary>
    public class ClosedEventArgs : EventArgs
    {
        public ClosedEventArgs()
        {
        }

        public ClosedEventArgs(System.Net.IPEndPoint iPEndPoint, CloseReason closeReason)
        {
            IPEndPoint = iPEndPoint;
            CloseReason = closeReason;
        }

        /// <summary>
        /// 客户端的地址
        /// </summary>
        public System.Net.IPEndPoint IPEndPoint { get; set; }

        /// <summary>
        /// 关闭原因
        /// </summary>
        public CloseReason CloseReason { get; set; }
    }

}
