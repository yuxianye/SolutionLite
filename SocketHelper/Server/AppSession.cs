using SuperSocket.SocketBase;
using System;

namespace SocketHelper
{

    /// <summary>
    /// 
    /// </summary>
    internal class AppSession : AppSession<AppSession, RequestInfo>
    {

        protected override void HandleException(Exception e)
        {
            LogHelper.Logger.Error($"AppSession HandleException", e);
        }
    }
}
