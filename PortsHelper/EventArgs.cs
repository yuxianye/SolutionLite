using System;
using System.IO.Ports;

namespace PortsHelper
{
    /// <summary>
    /// 数据接收事件参数
    /// </summary>
    public class DataReceivedEventArgs : EventArgs
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data">字节数组数据</param>
        public DataReceivedEventArgs(byte[] data)
        {
            this.Data = data;
        }

        /// <summary>
        /// 数据
        /// </summary>
        public byte[] Data;

    }

    public class SerialPortErrorEventArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data">字节数组数据</param>
        public SerialPortErrorEventArgs(string message)
        {
            this.Message = message;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data">字节数组数据</param>
        public SerialPortErrorEventArgs(string message, SerialErrorReceivedEventArgs serialError)
        {
            this.Message = message;
            this.SerialError = serialError;
        }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message;

        /// <summary>
        /// 串口错误
        /// </summary>
        public SerialErrorReceivedEventArgs SerialError;
    }
}
