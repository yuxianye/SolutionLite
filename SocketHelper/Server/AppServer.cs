using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;

namespace SocketHelper
{
    /// <summary>
    /// 服务端
    /// </summary>
    internal class SocketServer : AppServer<AppSession, RequestInfo>
    {
        public SocketServer()
            : base(new DefaultReceiveFilterFactory<ServerReceiveFilter, RequestInfo>())
        {

        }

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            return base.Setup(rootConfig, config);
        }
    }
}
