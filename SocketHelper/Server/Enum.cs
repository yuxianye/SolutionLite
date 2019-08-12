namespace SocketHelper
{
    /// <summary>
    /// 服务状态枚举
    /// </summary>
    public enum ServerState
    {

        /// <summary>
        /// Not initialized
        /// </summary>
        NotInitialized = 0,

        /// <summary>
        /// In initializing
        /// </summary>
        Initializing = 1,

        /// <summary>
        /// Has been initialized, but not started
        /// </summary>
        NotStarted = 2,

        /// <summary>
        /// In starting
        /// </summary>
        Starting = 3,

        /// <summary>
        /// In running
        /// </summary>
        Running = 4,

        /// <summary>
        /// In stopping
        /// </summary>
        Stopping = 5
    }

    /// <summary>
    /// 关闭原因枚举
    /// </summary>
    public enum CloseReason
    {
        /// <summary>
        /// The socket is closed for unknown reason
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Close for server shutdown
        /// </summary>
        ServerShutdown = 1,

        /// <summary>
        /// The client close the socket
        /// </summary>
        ClientClosing = 2,

        /// <summary>
        /// The server side close the socket
        /// </summary>
        ServerClosing = 3,

        /// <summary>
        /// Application error
        /// </summary>
        ApplicationError = 4,

        /// <summary>
        /// The socket is closed for a socket error
        /// </summary>
        SocketError = 5,

        /// <summary>
        /// The socket is closed by server for timeout
        /// </summary>
        TimeOut = 6,

        /// <summary>
        /// Protocol error
        /// </summary>
        ProtocolError = 7,

        /// <summary>
        /// SuperSocket internal error
        /// </summary>
        InternalError = 8
    }
}
