namespace OpcHelper
{
    /// <summary>
    /// 名称必须和<seealso cref="Opc.ResultID"/> 相同，，后添加的除外
    /// </summary>
    public enum OpcResult
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknow,

        /// <summary>
        /// 数据项添加
        /// </summary>
        DataItemAdded,

        /// <summary>
        /// 数据项已订阅
        /// </summary>
        DataItemRegistered,

        /// <summary>
        /// 数据项已取消订阅
        /// </summary>
        DataItemUnregistered,

        /// <summary>
        /// 服务器已关闭
        /// </summary>
        ServerShutdown,

        /// <summary>
        /// 服务器未连接
        /// </summary>
        ServerNoConnect,

        #region 以下来自 Opc.ResultID

        E_ACCESS_DENIED,
        E_FAIL,
        E_INVALIDARG,
        E_NETWORK_ERROR,
        E_NOTSUPPORTED,
        E_OUTOFMEMORY,
        E_TIMEDOUT,
        S_FALSE,
        S_OK,

        E_UNKNOWN_ITEM_NAME,

        #endregion

    }


}
