using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace PortsHelper
{
    public class SerialPortHelper : Core.Disposable
    {
        public SerialPortHelper()
        {
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.ErrorReceived += SerialPort_ErrorReceived;
        }


        #region 私有变量定义

        /// <summary>
        /// 串口
        /// </summary>
        private System.IO.Ports.SerialPort serialPort = new System.IO.Ports.SerialPort();

        #endregion

        #region 事件句柄定义

        /// <summary>
        /// 数据接收
        /// </summary>
        public EventHandler<DataReceivedEventArgs> OnDataReceived;

        /// <summary>
        /// 错误
        /// </summary>
        public EventHandler<SerialPortErrorEventArgs> OnError;

        #endregion

        #region 属性

        /// <summary>
        /// 窗口名称，默认COM1
        /// </summary>
        public string PortName { get; set; } = "COM1";

        /// <summary>
        /// 波特率，默认9600
        /// </summary>
        public int BaudRate = (int)SerialPortBaudRates.BaudRate_9600;

        /// <summary>
        /// 奇偶校验位
        /// </summary>
        public Parity Parity = Parity.None;

        /// <summary>
        /// 数据位
        /// </summary>
        public SerialPortDatabits DataBits { get; set; } = SerialPortDatabits.EightBits;

        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits StopBits { get; set; } = StopBits.One;

        /// <summary>
        /// 能否触发数据接收事件
        /// </summary>
        public bool CanRaiseDataReceivedEvent { get; set; } = true;

        #endregion

        /// <summary>
        /// 打开端口
        /// </summary>
        /// <returns></returns>
        public bool OpenPort()
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
                serialPort.PortName = PortName;
                serialPort.BaudRate = (int)BaudRate;
                serialPort.Parity = Parity;
                serialPort.DataBits = (int)DataBits;
                serialPort.StopBits = StopBits;
                serialPort.Open();
                return true;
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, new SerialPortErrorEventArgs(ex.Message));
                return false;
            }
        }

        /// <summary>
        /// 关闭端口
        /// </summary>
        public bool ClosePort()
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, new SerialPortErrorEventArgs(ex.Message));
                return false;
            }
        }

        /// <summary>
        /// 丢弃来自串行驱动程序的接收和发送缓冲区的数据
        /// </summary>
        public void DiscardBuffer()
        {
            serialPort.DiscardInBuffer();
            serialPort.DiscardOutBuffer();
        }

        /// <summary>
        /// 结束标志
        /// </summary>
        public byte EndByte { get; set; } = 0xd;

        /// <summary>
        /// 数据接收处理,根据业务接收格式处理接收数据
        /// </summary>
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //禁止接收事件时直接退出
            if (!CanRaiseDataReceivedEvent) return;

            try
            {
                //#region 根据结束字节来判断是否全部获取完成
                //List<byte> _byteData = new List<byte>();
                //bool found = false;//是否检测到结束符号
                //while (serialPort.BytesToRead > 0 || !found)
                //{
                //    byte[] readBuffer = new byte[serialPort.ReadBufferSize + 1];
                //    int count = serialPort.Read(readBuffer, 0, serialPort.ReadBufferSize);
                //    for (int i = 0; i < count; i++)
                //    {
                //        _byteData.Add(readBuffer[i]);


                //        if (readBuffer[i] == EndByte)
                //        {
                //            //校验和是否设置？结束符之后还有一个校验和

                //            found = true;
                //        }
                //    }
                //}
                //#endregion
                //
                //
                #region 根据结束字节来判断是否全部获取完成
                List<byte> _byteData = new List<byte>();
                while (serialPort.BytesToRead > 0)
                {
                    byte[] readBuffer = new byte[serialPort.ReadBufferSize + 1];
                    int count = serialPort.Read(readBuffer, 0, serialPort.ReadBufferSize);
                    for (int i = 0; i < count; i++)
                    {
                        _byteData.Add(readBuffer[i]);
                    }
                }
                #endregion

                //字符转换
                string readString = System.Text.Encoding.Default.GetString(_byteData.ToArray(), 0, _byteData.Count);

                //触发整条记录的处理
                //if (DataReceived != null)
                //{
                //    DataReceived(new DataReceivedEventArgs(readString));
                //}
                OnDataReceived?.Invoke(this, new DataReceivedEventArgs(_byteData.ToArray()));
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, new SerialPortErrorEventArgs(ex.Message));
            }
        }

        /// <summary>
        /// 错误处理函数
        /// </summary>
        private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            OnError?.Invoke(this, new SerialPortErrorEventArgs("串口错误！", e));
        }

        #region 数据写入操作

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="data"></param>
        public bool WriteData(string data)
        {
            try
            {
                if (!(serialPort.IsOpen))
                {
                    serialPort.Open();
                }

                serialPort.Write(data);
                return true;
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, new SerialPortErrorEventArgs(ex.Message));
                return false;
            }

        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="data">写入端口的字节数组</param>
        public bool WriteData(byte[] data)
        {
            try
            {
                if (!(serialPort.IsOpen))
                {
                    serialPort.Open();
                }
                serialPort.Write(data, 0, data.Length);
                return true;
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, new SerialPortErrorEventArgs(ex.Message));
                return false;
            }
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="data">包含要写入数据的字节数组</param>
        /// <param name="offset">参数从0字节开始的字节偏移量</param>
        /// <param name="count">要写入的字节数</param>
        public bool WriteData(byte[] data, int offset, int count)
        {
            try
            {
                if (!(serialPort.IsOpen))
                {
                    serialPort.Open();
                }
                serialPort.Write(data, offset, count);
                return true;
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, new SerialPortErrorEventArgs(ex.Message));
                return false;
            }
        }

        /// <summary>
        /// 发送串口命令，并在超时时间内等待应答的数据
        /// </summary>
        /// <param name="sendData">发送数据</param>
        /// <param name="receiveData">接收数据,需要先实例化</param>
        /// <param name="timeout">超时时间（毫秒）</param>
        /// <returns>读取的字节数</returns>
        public int SendCommand(byte[] sendData, ref byte[] receiveData, int timeout)
        {
            if (!(serialPort.IsOpen))
            {
                serialPort.Open();
            }
            CanRaiseDataReceivedEvent = false;        //关闭接收事件
            serialPort.DiscardInBuffer();      //清空接收缓冲区                 
            serialPort.Write(sendData, 0, sendData.Length);

            int num = 0, ret = 0;
            while (num++ < timeout)
            {
                if (serialPort.BytesToRead >= receiveData.Length)
                {
                    break;
                }
                System.Threading.Thread.Sleep(1);
            }
            if (serialPort.BytesToRead >= receiveData.Length)
            {
                ret = serialPort.Read(receiveData, 0, receiveData.Length);
            }
            CanRaiseDataReceivedEvent = true;//打开事件
            return ret;
        }
        #endregion

        #region  静态方法

        /// <summary>
        /// 检查端口名称是否存在
        /// </summary>
        /// <param name="portName"></param>
        /// <returns></returns>
        public static bool Exists(string portName)
        {

            return SerialPort.GetPortNames().Contains(portName);
            //foreach (string port in SerialPort.GetPortNames())
            //{
            //    if (port == portName) return true;
            //}
            //return false;
        }

        /// <summary>
        /// 封装获取串口号列表
        /// </summary>
        /// <returns></returns>
        public static string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>
        /// 转换字节数组到十六进制字符串
        /// </summary>
        /// <param name="comByte">待转换字节数组</param>
        /// <returns>十六进制字符串</returns>
        public static string ByteToHex(byte[] comByte)
        {
            StringBuilder builder = new StringBuilder(comByte.Length * 3);
            foreach (byte data in comByte)
            {
                builder.Append(Convert.ToString(data, 16).PadLeft(2, '0').PadRight(3, ' '));
            }
            return builder.ToString().ToUpper();
        }
        #endregion

        public override string ToString()
        {
            return String.Format("{0} ({1},{2},{3},{4},{5})",
            serialPort.PortName, serialPort.BaudRate, serialPort.DataBits, serialPort.StopBits, serialPort.Parity, serialPort.Handshake);
        }


        protected override void Disposing()
        {
            serialPort.Close();
            serialPort.Dispose();
        }


    }
}
