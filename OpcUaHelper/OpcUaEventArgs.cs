using System;

namespace OpcUaHelper
{
    /// <summary>
    /// OpcUa异常事件参数
    /// </summary>
    public class OpcUaErrorEventArgs : EventArgs
    {
        public OpcUaErrorEventArgs(OpcUaStatusCodes opcUaStatusCodes, string message, Exception exception)
        {
            this.OpcUaStatusCodes = opcUaStatusCodes;
            this.Message = message;
            this.Exception = exception;
        }

        public OpcUaStatusCodes OpcUaStatusCodes { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }
    }

    /// <summary>
    /// OpcUa数据项事件参数
    /// </summary>
    public class OpcUaDataEventArgs : EventArgs
    {
        public OpcUaDataEventArgs(OpcUaStatusCodes opcUaStatusCodes, OpcUaDataItem opcUaDataItem)
        {
            this.OpcUaStatusCodes = opcUaStatusCodes;
            this.OpcUaDataItem = opcUaDataItem;
        }

        public OpcUaStatusCodes OpcUaStatusCodes { get; set; }

        public OpcUaDataItem OpcUaDataItem { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class OpcUaLogEventArgs : EventArgs
    {
        public OpcUaLogEventArgs(string log)
        {
            this.Log = log;
        }
        public string Log { get; set; }
    }

}
