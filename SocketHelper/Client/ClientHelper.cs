using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketHelper
{
    /// <summary>
    /// Socket客户端帮助类
    /// </summary>
    public class ClientHelper : Core.Disposable
    {
        /// <summary>
        /// 够赞函数
        /// </summary>
        public ClientHelper()
        {
            init();
        }

        #region 私有变量定义

        private SuperSocket.ClientEngine.EasyClient easyClient = new SuperSocket.ClientEngine.EasyClient();

        #endregion

        #region 初始化变量

        /// <summary>
        /// 初始化总入口
        /// </summary>
        private void init()
        {
            initEasyClient();
        }

        /// <summary>
        /// 初始化客户端
        /// </summary>
        private void initEasyClient()
        {
            easyClient.NoDelay = false;
            easyClient.Closed += EasyClient_Closed;
            easyClient.Connected += EasyClient_Connected;
            easyClient.Error += EasyClient_Error;
            easyClient.Initialize<ClientPackageInfo>(new ClientReceiveFilter(), new Action<ClientPackageInfo>(PackageHandler));
        }

        private void EasyClient_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            OnError?.Invoke(this, e.Exception);
        }

        private void EasyClient_Connected(object sender, EventArgs e)
        {
            OnConnected?.Invoke(this, e);
        }

        private void EasyClient_Closed(object sender, EventArgs e)
        {
            OnClosed?.Invoke(this, e);
        }

        #endregion

        #region 事件

        /// <summary>
        /// 收到数据包事件
        /// </summary>
        public event EventHandler<ClientPackageInfo> OnPackagReceived;

        /// <summary>
        /// 错误事件
        /// </summary>
        public event EventHandler<Exception> OnError;

        /// <summary>
        /// 关闭事件
        /// </summary>
        public event EventHandler OnClosed;

        /// <summary>
        /// 连接事件
        /// </summary>
        public event EventHandler OnConnected;

        #endregion

        #region 属性

        /// <summary>
        /// 是否连接
        /// </summary>
        public bool IsConnected
        {
            get { return easyClient.IsConnected; }
        }

        #endregion

        /// <summary>
        /// 处理数据包
        /// </summary>
        /// <param name="packageInfo"></param>
        private void PackageHandler(ClientPackageInfo packageInfo)
        {
            OnPackagReceived?.Invoke(this, packageInfo);
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="iPEndPoint"></param>
        /// <returns></returns>
        public Task<bool> ConnectAsync(System.Net.IPEndPoint iPEndPoint)
        {
            return easyClient.ConnectAsync(iPEndPoint);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <returns></returns>
        public Task<bool> CloseAsync()
        {
            return easyClient.Close();
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="data"></param>
        public bool Send(byte[] data)
        {
            try
            {
                easyClient.Send(data);
                //
                string infoStr1 = Encoding.ASCII.GetString(data).Substring(0, 6);
                string infoStr2 = Encoding.ASCII.GetString(data);
                string infoStr3 = Encoding.UTF8.GetString(data);
                //
                LogHelper.Logger.Info($"发送消息完成！");
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error($"发送消息时错误，数据内容（字符）：{System.Text.Encoding.Unicode.GetString(data)}{System.Environment.NewLine}数据内容（十六进制）：{string.Join(" ", data)}");
                OnError?.Invoke(this, ex);
                return false;
            }
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="segment"></param>
        public bool Send(ArraySegment<byte> segment)
        {
            try
            {
                easyClient.Send(segment);
                LogHelper.Logger.Info($"发送消息完成！");
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error($"发送消息时错误，segment Count：{segment.Count()}", ex);
                OnError?.Invoke(this, ex);
                return false;
            }
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="segments"></param>
        public bool Send(List<ArraySegment<byte>> segments)
        {
            try
            {
                easyClient.Send(segments);
                LogHelper.Logger.Info($"发送消息完成！");

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error($"发送消息时错误，segments Count：{segments.Count()}", ex);
                OnError?.Invoke(this, ex);
                return false;
            }
        }

        protected override void Disposing()
        {
            this.easyClient.Close();
        }
    }
}
