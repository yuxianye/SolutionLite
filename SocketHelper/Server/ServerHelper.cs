using SuperSocket.SocketBase.Config;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocketHelper
{
    /// <summary>
    /// Socket服务端帮助类
    /// </summary>
    public class ServerHelper : Core.Disposable
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ServerHelper()
        {
            init();
        }

        #region 私有变量定义

        /// <summary>
        /// Socket服务端
        /// </summary>
        private SocketServer socketServer = new SocketServer();

        #endregion

        #region 事件

        /// <summary>
        /// 收到数据包事件
        /// </summary>
        public event EventHandler<RequestEventArgs> OnRequestReceived;

        /// <summary>
        /// 错误事件
        /// </summary>
        public event EventHandler<ConnectEventArgs> OnSessionConnected;

        /// <summary>
        /// 关闭事件
        /// </summary>
        public event EventHandler<ClosedEventArgs> OnSessionClosed;

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化总入口
        /// </summary>
        private void init()
        {
            initSocketServer();
        }

        /// <summary>
        /// 初始化SocketServer
        /// </summary>
        private void initSocketServer()
        {
            socketServer.NewRequestReceived += SocketServer_NewRequestReceived;
            socketServer.NewSessionConnected += SocketServer_NewSessionConnected;
            socketServer.SessionClosed += SocketServer_SessionClosed;
        }

        #endregion

        #region 公有属性

        /// <summary>
        /// 获取 服务状态
        /// </summary>
        public ServerState ServerState
        {
            get { return (ServerState)(int)socketServer.State; }
        }

        /// <summary>
        /// 获取 客户端的数量
        /// </summary>
        public int ClientCount
        {
            get { return socketServer.SessionCount; }
        }


        /// <summary>
        /// 获取 客户端的终结点地址
        /// </summary>
        public List<System.Net.IPEndPoint> ClientsIPEndPoint { get; set; } = new List<System.Net.IPEndPoint>();

        #endregion

        #region 公有操作方法

        /// <summary>
        /// 开始
        /// </summary>
        /// <returns></returns>
        public bool Start(int port)
        {
            if (socketServer.State == SuperSocket.SocketBase.ServerState.NotInitialized)
            {
                socketServer.Setup(new ServerConfig()
                {
                    MaxRequestLength = 10240,
                    Port = port,
                });
            }
            //
            //
            if (socketServer.State == SuperSocket.SocketBase.ServerState.NotStarted)
            {
                return socketServer.Start();
            }
            else if (socketServer.State == SuperSocket.SocketBase.ServerState.Starting)
            {
                return true;
            }
            else if (socketServer.State == SuperSocket.SocketBase.ServerState.Running)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            socketServer.Stop();

        }

        /// <summary>
        /// 发送字符串
        /// </summary>
        /// <param name="message"></param>
        public bool Send(System.Net.IPEndPoint iPEndPoint, string message)
        {
            try
            {
                var sessions = socketServer.GetSessions(a => a.RemoteEndPoint.ToString() == iPEndPoint.ToString());
                if (Equals(sessions, null) || !sessions.Any())
                {
                    LogHelper.Logger.Info($"未找到客户端：{iPEndPoint.ToString()}，发送消息失败！");
                    return false;
                }
                else
                {
                    sessions.FirstOrDefault()?.Send(message);
                    return true;

                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error($"给客户端：{iPEndPoint.ToString()}发送消息时错误，{ex.Message}.数据内容：{message}");

                return false;
            }
        }

        /// <summary>
        /// 发送字节数组
        /// </summary>
        /// <param name="data"></param>
        public bool Send(System.Net.IPEndPoint iPEndPoint, byte[] data)
        {
            try
            {
                var sessions = socketServer.GetSessions(a => a.RemoteEndPoint.ToString() == iPEndPoint.ToString());
                if (Equals(sessions, null) || !sessions.Any())
                {
                    LogHelper.Logger.Info($"未找到客户端：{iPEndPoint.ToString()}，发送消息失败！");
                    return false;
                }
                else
                {
                    sessions.FirstOrDefault()?.Send(new ArraySegment<byte>(data));
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error($"给客户端：{iPEndPoint.ToString()}发送消息时错误，{ex.Message}.数据内容（字符）：{System.Text.Encoding.Unicode.GetString(data)}{System.Environment.NewLine}数据内容（十六进制）：{string.Join(" ", data)}");
                return false;
            }
        }

        /// <summary>
        /// 发送字节数组的特定段的内容
        /// </summary>
        /// <param name="iPEndPoint"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public bool Send(System.Net.IPEndPoint iPEndPoint, byte[] data, int offset, int length)
        {
            try
            {
                var sessions = socketServer.GetSessions(a => a.RemoteEndPoint.ToString() == iPEndPoint.ToString());
                if (Equals(sessions, null) || !sessions.Any())
                {
                    LogHelper.Logger.Info($"未找到客户端：{iPEndPoint.ToString()}，发送消息失败！");
                    return false;
                }
                else
                {
                    sessions.FirstOrDefault()?.Send(data, offset, length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error($"给客户端：{iPEndPoint.ToString()}发送消息时错误，data length{data?.Length }offset：{offset}，length{length}", ex);
                return false;
            }
        }

        /// <summary>
        /// 发送一维字节数组
        /// </summary>
        /// <param name="segment"></param>
        public bool Send(System.Net.IPEndPoint iPEndPoint, ArraySegment<byte> segment)
        {
            try
            {
                var sessions = socketServer.GetSessions(a => a.RemoteEndPoint.ToString() == iPEndPoint.ToString());
                if (Equals(sessions, null) || !sessions.Any())
                {
                    LogHelper.Logger.Info($"未找到客户端：{iPEndPoint.ToString()}，发送消息失败！");
                    return false;
                }
                else
                {
                    sessions.FirstOrDefault()?.Send(segment);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error($"给客户端：{iPEndPoint.ToString()}发送消息时错误，segment Count：{segment.Count()}", ex);
                return false;
            }
        }

        /// <summary>
        /// 发送多维字节数组
        /// </summary>
        /// <param name="data"></param>
        public bool Send(System.Net.IPEndPoint iPEndPoint, List<ArraySegment<byte>> segments)
        {
            try
            {
                var sessions = socketServer.GetSessions(a => a.RemoteEndPoint.ToString() == iPEndPoint.ToString());
                if (Equals(sessions, null) || !sessions.Any())
                {
                    LogHelper.Logger.Info($"未找到客户端：{iPEndPoint.ToString()}，发送消息失败！");
                    return false;
                }
                else
                {
                    sessions.FirstOrDefault()?.Send(segments);
                    return true;

                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error($"给客户端：{iPEndPoint.ToString()}发送消息时错误，segments Count：{segments.Count()}", ex);
                return false;
            }
        }

        #endregion

        /// <summary>
        /// 关闭事件处理器
        /// </summary>
        /// <param name="session"></param>
        /// <param name="closeReason"></param>
        private void SocketServer_SessionClosed(AppSession session, SuperSocket.SocketBase.CloseReason closeReason)
        {
            ClientsIPEndPoint.Remove(session.RemoteEndPoint);
            LogHelper.Logger.Info($"SocketServer_SessionClosed,SessionID:{session.SessionID} CloseReason:{closeReason}");
            OnSessionClosed?.Invoke(this, new ClosedEventArgs(session.RemoteEndPoint, (CloseReason)(int)closeReason));
        }

        /// <summary>
        /// 连接事件处理器
        /// </summary>
        /// <param name="session"></param>
        private void SocketServer_NewSessionConnected(AppSession session)
        {
            ClientsIPEndPoint.Add(session.RemoteEndPoint);
            LogHelper.Logger.Info($"SocketServer_NewSessionConnected,SessionID:{session.SessionID}");
            OnSessionConnected?.Invoke(this, new ConnectEventArgs(session.RemoteEndPoint));
        }

        /// <summary>
        /// 请求事件处理器
        /// </summary>
        /// <param name="session"></param>
        /// <param name="requestInfo"></param>
        private void SocketServer_NewRequestReceived(AppSession session, RequestInfo requestInfo)
        {
            //LogHelper.Logger.Info($"SocketServer_NewRequestReceived,SessionID:{session.SessionID} {Utility.TypeExtensions.GetPropertiesValue(requestInfo)}");
            LogHelper.Logger.Info($"SocketServer_NewRequestReceived,SessionID:{session.SessionID}");
            OnRequestReceived?.Invoke(this, new RequestEventArgs(session.RemoteEndPoint, requestInfo));
            //OnRequestReceived?.BeginInvoke(this, new RequestEventArgs(session.RemoteEndPoint, requestInfo), (result) =>
            //{
            //    OnRequestReceived?.EndInvoke(result);
            //}, null);
        }

        protected override void Disposing()
        {
            if (!Equals(socketServer, null))
            {
                socketServer.Dispose();
            }
        }
    }
}
