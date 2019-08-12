using SuperSocket.ProtoBase;
using System;
using System.Text;

namespace SocketHelper
{
    /// <summary>
    /// 客户端接收过滤器
    /// </summary>
    public class ClientReceiveFilter : FixedHeaderReceiveFilter<ClientPackageInfo>
    {
        public ClientReceiveFilter() : base(29)
        {
        }

        /// <summary>
        /// 从包头取得消息体的长度
        /// </summary>
        /// <param name="bufferStream"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected override int GetBodyLengthFromHeader(IBufferStream bufferStream, int length)
        {
            int bodyLength = 0;
            try
            {
                bodyLength = Convert.ToInt32(Encoding.ASCII.GetString(bufferStream.Buffers, 25, 4)) - base.HeaderSize;
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("客户端GetBodyLengthFromHeader错误！", ex);
            }
            return bodyLength;
        }

        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="bufferStream"></param>
        /// <returns></returns>
        public override ClientPackageInfo ResolvePackage(IBufferStream bufferStream)
        {
            ClientPackageInfo clientPackageInfo = new ClientPackageInfo();
            try
            {
                String infoStr = Encoding.ASCII.GetString(bufferStream.Buffers, 0, base.HeaderSize);
                clientPackageInfo.MessageID = infoStr.Substring(0, 6).Trim();
                clientPackageInfo.DateOfSending = infoStr.Substring(6, 8).Trim();
                clientPackageInfo.TimeOfSending = infoStr.Substring(14, 6).Trim();
                clientPackageInfo.Sender = infoStr.Substring(20, 2).Trim();
                clientPackageInfo.Receiver = infoStr.Substring(22, 2).Trim();
                clientPackageInfo.FunctionCode = infoStr.Substring(24, 1).Trim();
                clientPackageInfo.Length = infoStr.Substring(25, 4).Trim();

                int bodyLength = Convert.ToInt32(Encoding.ASCII.GetString(bufferStream.Buffers, 25, 4)) - base.HeaderSize;
                if (bodyLength > 0)
                {
                    ArraySegment<byte> result = new ArraySegment<byte>(new byte[bodyLength]);
                    bufferStream.Buffers.CopyTo(result, base.HeaderSize, bodyLength);
                    clientPackageInfo.MessageBody = result.Array;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("客户端ResolvePackage错误！", ex);
                clientPackageInfo = null;
            }
            return clientPackageInfo;
        }
    }
}
