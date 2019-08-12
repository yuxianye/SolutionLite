using SuperSocket.Common;
using SuperSocket.Facility.Protocol;
using System;
using System.Text;

namespace SocketHelper
{
    /// <summary>
    /// 服务端接收过滤器
    /// </summary>
    internal class ServerReceiveFilter : FixedHeaderReceiveFilter<RequestInfo>
    {
        /// <summary>
        /// 头长度
        /// </summary>
        private const int heaserSize = 29;

        public ServerReceiveFilter() : base(heaserSize)
        {
            //base.ProcessMatchedRequest
        }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            int bodyLength = 0;
            try
            {
                bodyLength = Convert.ToInt32(Encoding.ASCII.GetString(header, offset + 25, 4)) - heaserSize;
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("服务端GetBodyLengthFromHeader错误！", ex);
            }
            return bodyLength;
        }

        protected override RequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            RequestInfo requestInfo = new RequestInfo();
            try
            {
                String infoStr = Encoding.ASCII.GetString(header.Array, header.Offset, heaserSize);
                requestInfo.MessageID = infoStr.Substring(0, 6).Trim();
                requestInfo.DateOfSending = infoStr.Substring(6, 8).Trim();
                requestInfo.TimeOfSending = infoStr.Substring(14, 6).Trim();
                requestInfo.Sender = infoStr.Substring(20, 2).Trim();
                requestInfo.Receiver = infoStr.Substring(22, 2).Trim();
                requestInfo.FunctionCode = infoStr.Substring(24, 1).Trim();
                requestInfo.Length = infoStr.Substring(25, 4).Trim();
                if (length > 0)
                {
                    //////////////////////////////////
                    //if (requestInfo.MessageID.Equals("102602"))
                    //{
                    //length = length * 3;
                    //if (length > bodyBuffer.Length - offset)
                    //{
                    //    length = bodyBuffer.Length - offset;
                    //}
                    ////}
                    //byte b1 = 0x0a;
                    //int i = bodyBuffer.IndexOf(b1, offset, length);
                    //length = i - offset + 1;
                    //////////////////////////////////
                    //
                    byte[] bodyTempBuffer = bodyBuffer.CloneRange(offset, length);
                    requestInfo.MessageBody = bodyTempBuffer;
                    //
                    string infoStr1 = requestInfo.MessageID;
                    string infoStr2 = Encoding.ASCII.GetString(requestInfo.MessageBody);
                    string infoStr3 = Encoding.UTF8.GetString(requestInfo.MessageBody);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("服务端ResolveRequestInfo错误！", ex);
                requestInfo = null;
            }
            return requestInfo;
        }
    }
}
