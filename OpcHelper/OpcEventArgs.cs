using System;

namespace OpcHelper
{
    /// <summary>
    /// Opc异常事件参数
    /// </summary>
    public class OpcErrorEventArgs : EventArgs
    {
        public OpcErrorEventArgs(OpcResult opcResult, string message, Exception exception)
        {
            this.OpcResult = opcResult;
            this.Message = message;
            this.Exception = exception;
        }

        public OpcResult OpcResult { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }
    }


    /// <summary>
    /// Opc数据项事件参数
    /// </summary>
    public class OpcDataEventArgs : EventArgs
    {
        public OpcDataEventArgs(OpcResult opcResult, OpcDataItem opcDataItem)
        {
            this.OpcResult = opcResult;
            this.OpcDataItem = opcDataItem;
        }
        public OpcResult OpcResult { get; set; }

        public OpcDataItem OpcDataItem { get; set; }
    }

    public class OpcLogEventArgs : EventArgs
    {
        public OpcLogEventArgs(string log)
        {
            this.Log = log;
        }
        public string Log { get; set; }
    }

}
