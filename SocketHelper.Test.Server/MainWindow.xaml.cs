using System;
using System.Linq;
using System.Text;
using System.Windows;
using Utility;

namespace SocketHelper.Test.Server
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Threading.Timer healthCheckTimer;// = new System.Threading.Timer(new System.Threading.TimerCallback(healthCheckTimer_Callback), null, 0, 5 * 1000);

        private bool connFlagClinet;

        private bool listingFlag;

        //Msg261001 msg261001 = new Msg261001();

        //Msg261005 msg261005 = new Msg261005();

        //Msg261006 msg261006 = new Msg261006();

        //Msg261007 msg261007 = new Msg261007();

        //Msg261011 msg261011 = new Msg261011();

        //Msg261012 msg261012 = new Msg261012();

        //Msg261013 msg261013 = new Msg261013();

        public MainWindow()
        {
            InitializeComponent();


            //NModbus.ModbusFactory modbusFactory = new NModbus.ModbusFactory();

            //var modbusMaster = modbusFactory.CreateMaster(new System.Net.Sockets.TcpClient());
            //var modbusMaster2 = modbusFactory.CreateMaster();


            LogHelper.Logger.Debug($"MD5Encrypt：{Utility.Security.MD5Encrypt("yuxiaye1")}");


            LogHelper.Logger.Debug($"RSAEncrypt：{Utility.Security.RSAEncrypt("yuxiaye1")}");

            LogHelper.Logger.Debug($"RSAEncrypt：{Utility.Security.RSAEncrypt("yuxiaye1")}");








            initSocketServer();

            btn_Connect_Click(null, null);

            healthCheckTimer = new System.Threading.Timer(new System.Threading.TimerCallback(HealthCheckTimer_Callback), null, 2, 5 * 1000);
            //healthCheckTimer = new System.Threading.Timer(new System.Threading.TimerCallback(HealthCheckTimer_Callback), null, 2, 500);
        }

        private void initSocketServer()
        {
            serverHelper.OnRequestReceived -= ServerHelper_OnRequestReceived;
            serverHelper.OnSessionClosed -= ServerHelper_OnSessionClosed;
            serverHelper.OnSessionConnected -= ServerHelper_OnSessionConnected;

            serverHelper.OnRequestReceived += ServerHelper_OnRequestReceived;
            serverHelper.OnSessionClosed += ServerHelper_OnSessionClosed;
            serverHelper.OnSessionConnected += ServerHelper_OnSessionConnected;
        }

        private void connect2SocketServer()
        {
            if (connFlagClinet)
            {
                return;
            }
            clientHelper.OnClosed -= ClientHelper_OnClosed;
            clientHelper.OnConnected -= ClientHelper_OnConnected;
            clientHelper.OnError -= ClientHelper_OnError;
            clientHelper.OnPackagReceived -= ClientHelper_OnPackagReceived;

            clientHelper.OnClosed += ClientHelper_OnClosed;
            clientHelper.OnConnected += ClientHelper_OnConnected;
            clientHelper.OnError += ClientHelper_OnError;
            clientHelper.OnPackagReceived += ClientHelper_OnPackagReceived;
            //bool bResult = socketClient.Start("192.168.50.81", 12151, ConnMode.ACTIVE);
            //bool bResult = clientHelper.ConnectAsync ("127.0.0.1", 12151, ConnMode.ACTIVE);
            bool bResult = clientHelper.ConnectAsync(new System.Net.IPEndPoint(System.Net.IPAddress.Parse("127.0.0.1"), 12151)).Result;
            if (bResult)
            {
                upUiMessage("连接铣床服务器成功！");
                LogHelper.Logger.Info("连接铣床服务器成功！");
                connFlagClinet = true;
            }
            else
            {
                //upUiConnectMessageColor(Brushes.Red);
                //upUiConnectMessage("连接MES服务器失败！自动重新连接中...");
                LogHelper.Logger.Error("连接铣床服务器失败！自动重新连接中...", null);
                //connFlagClinet = false;
            }

            if (bResult)
            {
                upUiMessage("铣床服务器连接成功！");
                connFlagClinet = true;
            }
            else
            {
                //serverConnectStatus.Text = "连接中...";
                //serverConnectStatus.Background = Brushes.Red;
                upUiMessage("铣床服务器连接失败！");
                connFlagClinet = false;
            }
            //service.Stop(ConnMode.PASSIVE);
            //btnStartServer.BackColor = Color.Lime;
            //btnStartServer.Text = "开始服务";
            //service.NewSessionConnectedAction += OnConnected;



            //socketClient.SessionClosedAction += OnSessionClosed;
            ////service.NewRequestReceivedAction += OnReceived;
            //socketClient.NewClientReceivedAction -= OnClientReceived;
            //socketClient.NewClientReceivedAction += OnClientReceived;
            //service.SessionClosedAction = new Action<string, string, string>(seeionClosed);
            //service.NewClientReceivedAction += OnReceived;

        }






        private void HealthCheckTimer_Callback(object state)
        {
            //if (listingFlag && !Equals(iPEndPointClient, null))
            //{

            //    bool IsEnableHeartBeat = false;
            //    string sEnableHeartBeat = ConfigHelper.GetAppSetting("IsEnableHeartBeat");
            //    var aa = bool.TryParse(sEnableHeartBeat == null ? "false" : sEnableHeartBeat, out IsEnableHeartBeat);
            //    if (IsEnableHeartBeat)
            //    {
            //        DateTime dateTime = DateTime.Now;
            //        MsgHeartBeat msgHeartBeat = new MsgHeartBeat();
            //        msgHeartBeat.Header.DateOfSending = dateTime.ToString(ConstValue.Datetime_yyyyMMdd);
            //        msgHeartBeat.Header.TimeOfSending = dateTime.ToString(ConstValue.Datetime_HHmmss);

            //        bool sendResult = true;// serverHelper.Send(iPEndPointClient, msgHeartBeat.ToBytes().ToArray());
            //        sendResult = serverHelper.Send(iPEndPointClient, msgHeartBeat.ToBytes().ToArray());

            //        upUiMessage($"向铣床客户端发送心跳电文：{sendResult}");
            //    }
            //}

            ///////////////////////////

            if (!clientHelper.IsConnected)
            {
                connFlagClinet = false;
            }


            if (!connFlagClinet)
            {
                connect2SocketServer();
            }

            ///////////////////////////

            ////while (true)
            ////{
            ////System.Threading.Thread.Sleep(5000);
            ////connect2SocketServer();
            //if (!listingFlag)
            //{
            //    initSocketServer();
            //    btn_Connect_Click(null, null);
            //}

            //else
            //{
            //    DateTime dateTime = DateTime.Now;
            //    MsgHeartBeat msgHeartBeat = new MsgHeartBeat();
            //    msgHeartBeat.Header.DateOfSending = dateTime.ToString(ConstValue.Datetime_yyyyMMdd);
            //    msgHeartBeat.Header.TimeOfSending = dateTime.ToString(ConstValue.Datetime_HHmmss);

            //    bool sendResult = true;// serverHelper.Send(iPEndPointClient, msgHeartBeat.ToBytes().ToArray());
            //    sendResult = serverHelper.Send(iPEndPointClient, msgHeartBeat.ToBytes().ToArray());

            //    upUiMessage($"向铣床客户端发送心跳电文：{sendResult}");

            //}
            //}

        }

        private void upUiMessage(string message)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                //tb_Message.Text = message;
                logRichTextBox.AppendText(message);
                logRichTextBox.AppendText(System.Environment.NewLine);
                logRichTextBox.ScrollToEnd();
                //MessageBox.Show(message, this.Title, MessageBoxButton.OK, MessageBoxImage.Information);

            }));
        }



        DateTime lastActiveTime;
        private void ClientHelper_OnPackagReceived(object sender, ClientPackageInfo e)
        {

            //电文头错误
            if (Equals(e, null))
            {

            }

            //结束符错误，返回错误信息给服务器
            if (e.MessageBody.Last() != 0x0a)
            {

            }
            //处理电文
            switch (e.MessageID)
            {
                case "999999"://心跳
                    {
                        lastActiveTime = DateTime.Now;
                        connFlagClinet = true;
                        upUiMessage($"收到铣床服务器心跳电文！连接正常{e.TimeOfSending }");
                        break;
                    }
                //case "261003"://给铣床发送PDI之后，相同的session 等待 回复
                //    {
                //        upUiMessage($"收到铣床PDI应答电文！");
                //        break;
                //    }
                //case "201005"://给铣床发送删除电文，收到铣床删除PDI应答电文
                //    {
                //        upUiMessage($"收到铣床删除PDI应答电文！");
                //        break;
                //    }
                default:
                    {
                        //未定义的电文，返回错误信息给服务器
                        upUiMessage($"收到铣床未定义的电文！");

                        break;
                    }


            }








            //upUiMessage("铣床服务器连接成功！");

        }

        private void ClientHelper_OnError(object sender, Exception e)
        {
            upUiMessage($"连接到铣床服务器错误！{e.Message }");


        }

        private void ClientHelper_OnConnected(object sender, EventArgs e)
        {
            upUiMessage("已经连接到铣床服务器！");


        }

        private void ClientHelper_OnClosed(object sender, EventArgs e)
        {
            upUiMessage("铣床服务器关闭连接！");

        }


        /// <summary>
        /// 服务端
        /// </summary>
        SocketHelper.ServerHelper serverHelper = new ServerHelper();

        /// <summary>
        /// 客户端
        /// </summary>
        SocketHelper.ClientHelper clientHelper = new ClientHelper();



        /// <summary>
        /// 客户端的地址
        /// </summary>
        System.Net.IPEndPoint iPEndPointClient;

        private void ServerHelper_OnSessionConnected(object sender, ConnectEventArgs e)
        {
            listingFlag = true;
            iPEndPointClient = e.IPEndPoint;
            LogHelper.Logger.Debug($"OnSessionConnected：{e.IPEndPoint.ToString()}");
        }


        private void ServerHelper_OnSessionClosed(object sender, ClosedEventArgs e)
        {
            listingFlag = false;
            LogHelper.Logger.Debug($"OnSessionClosed：{e.IPEndPoint.ToString()} CloseReason{e.CloseReason }");

        }

        private void ServerHelper_OnRequestReceived(object sender, RequestEventArgs e)
        {
            //包头错误，返回错误信息给服务器
            if (Equals(e.RequestInfo, null))
            {


            }

            //结束符错误，返回错误信息给服务器
            if (e.RequestInfo.MessageBody != null)
            {
                if (e.RequestInfo.MessageBody.Last() != 0x0a)
                {

                }
            }
            ////处理电文
            //switch (e.RequestInfo.MessageID)
            //{
            //    case "261001":
            //        {
            //            upUiMessage($"收到铣床PDI电文！");
            //            //
            //            ////////////////////////////////////////////////////////////////////////////////
            //            //
            //            //Msg261001
            //            try
            //            {
            //                //string infoStr = Encoding.ASCII.GetString(e.RequestInfo.MessageBody);
            //                string infoStr = Encoding.UTF8.GetString(e.RequestInfo.MessageBody);
            //                msg261001 = new Msg261001();
            //                //
            //                //电文头实际长度为 30
            //                //普通电文 电文体开始于47位
            //                //普通电文 电文产生时间(共17位)
            //                int iPos = 47 - 17;
            //                //
            //                msg261001.BatchFlag = infoStr.Substring(47 - iPos, 1).Trim();
            //                msg261001.SlabNo = infoStr.Substring(48 - iPos, 20).Trim();
            //                //
            //                LogHelper.Logger.Info("接收261001信息成功！");
            //            }
            //            catch (Exception ex)
            //            {
            //                string msg1 = $"接收261001信息错误！{ex.Message}";
            //                LogHelper.Logger.Error(msg1, ex);
            //            }
            //            ///
            //            ////////////////////////////////////////////////////////////////////////////////
            //            //
            //            btn_SendPDI_Click(null, null);
            //            break;
            //        }
            //    case "261003"://给铣床发送PDI之后，相同的session 等待 回复
            //        {
            //            upUiMessage($"收到铣床PDI应答电文！");
            //            //
            //            ////////////////////////////////////////////////////////////////////////////////
            //            //
            //            //Msg261003
            //            try
            //            {
            //                //string infoStr = Encoding.ASCII.GetString(e.RequestInfo.MessageBody);
            //                string infoStr = Encoding.UTF8.GetString(e.RequestInfo.MessageBody);
            //                Msg261003 msg261003 = new Msg261003();
            //                //
            //                //电文头实际长度为 30
            //                //普通电文 电文体开始于47位
            //                //普通电文 电文产生时间(共17位)
            //                int iPos = 47 - 17;
            //                //
            //                msg261003.ProcessResult = infoStr.Substring(47 - iPos, 1).Trim();
            //                msg261003.ScheduleNo = infoStr.Substring(48 - iPos, 16).Trim();
            //                msg261003.SlabNo = infoStr.Substring(64 - iPos, 20).Trim();
            //                msg261003.NextPDIFlag = infoStr.Substring(84 - iPos, 1).Trim();
            //                msg261003.Reserve = infoStr.Substring(85 - iPos, 50).Trim();
            //                //
            //                LogHelper.Logger.Info("接收261003信息成功！");
            //            }
            //            catch (Exception ex)
            //            {
            //                string msg1 = $"接收261003信息错误！{ex.Message}";
            //                LogHelper.Logger.Error(msg1, ex);
            //            }
            //            ///
            //            ////////////////////////////////////////////////////////////////////////////////
            //            //
            //            break;
            //        }
            //    case "261005"://给铣床删除PDI之后， 回复应道电文
            //        {
            //            upUiMessage($"收到铣床PDI删除应答电文！");
            //            //
            //            ////////////////////////////////////////////////////////////////////////////////
            //            //
            //            //Msg261005
            //            try
            //            {
            //                //string infoStr = Encoding.ASCII.GetString(e.RequestInfo.MessageBody);
            //                string infoStr = Encoding.UTF8.GetString(e.RequestInfo.MessageBody);
            //                Msg261005 msg261005 = new Msg261005();
            //                //
            //                //电文头实际长度为 30
            //                //普通电文 电文体开始于47位
            //                //普通电文 电文产生时间(共17位)
            //                int iPos = 47 - 17;
            //                //
            //                msg261005.ProcessResult = infoStr.Substring(47 - iPos, 1).Trim();
            //                msg261005.ScheduleNo = infoStr.Substring(48 - iPos, 16).Trim();
            //                msg261005.SlabNo = infoStr.Substring(64 - iPos, 20).Trim();
            //                msg261005.NGRessonCode = infoStr.Substring(84 - iPos, 1).Trim();
            //                //
            //                LogHelper.Logger.Info("接收261005信息成功！");
            //            }
            //            catch (Exception ex)
            //            {
            //                string msg1 = $"接收261005信息错误！{ex.Message}";
            //                LogHelper.Logger.Error(msg1, ex);
            //            }
            //            ///
            //            ////////////////////////////////////////////////////////////////////////////////
            //            //
            //            break;
            //        }
            //    case "261006"://L2铣面计划删除电文
            //        {
            //            upUiMessage($"收到L2铣面计划删除电文！");
            //            //
            //            ////////////////////////////////////////////////////////////////////////////////
            //            //
            //            //Msg261006
            //            try
            //            {
            //                //string infoStr = Encoding.ASCII.GetString(e.RequestInfo.MessageBody);
            //                string infoStr = Encoding.UTF8.GetString(e.RequestInfo.MessageBody);
            //                Msg261006 msg261006 = new Msg261006();
            //                //
            //                //电文头实际长度为 30
            //                //普通电文 电文体开始于47位
            //                //普通电文 电文产生时间(共17位)
            //                int iPos = 47 - 17;
            //                //
            //                msg261006.ScheduleNo = infoStr.Substring(47 - iPos, 16).Trim();
            //                msg261006.SlabNo = infoStr.Substring(63 - iPos, 20).Trim();
            //                msg261006.OperatorID = infoStr.Substring(83 - iPos, 12).Trim();
            //                msg261006.DeleteReasonCode = infoStr.Substring(95 - iPos, 100).Trim();
            //                //
            //                LogHelper.Logger.Info("接收261006信息成功！");
            //            }
            //            catch (Exception ex)
            //            {
            //                string msg1 = $"接收261006信息错误！{ex.Message}";
            //                LogHelper.Logger.Error(msg1, ex);
            //            }
            //            ///
            //            ////////////////////////////////////////////////////////////////////////////////
            //            //
            //            break;
            //        }
            //    case "261007":
            //        {
            //            upUiMessage($"收到L2生产实绩电文！");
            //            //
            //            ////////////////////////////////////////////////////////////////////////////////
            //            //
            //            //Msg261007
            //            try
            //            {
            //                //string infoStr = Encoding.ASCII.GetString(e.RequestInfo.MessageBody);
            //                string infoStr = Encoding.UTF8.GetString(e.RequestInfo.MessageBody);
            //                msg261007 = new Msg261007();
            //                //
            //                //电文头实际长度为 30
            //                //普通电文 电文体开始于47位
            //                //普通电文 电文产生时间(共17位)
            //                int iPos = 47 - 17;
            //                //
            //                msg261007.ScheduleNo = infoStr.Substring(47 - iPos, 16).Trim();
            //                msg261007.SlabNo = infoStr.Substring(63 - iPos, 20).Trim();
            //                //
            //                LogHelper.Logger.Info("接收261007信息成功！");
            //            }
            //            catch (Exception ex)
            //            {
            //                string msg1 = $"接收261007信息错误！{ex.Message}";
            //                LogHelper.Logger.Error(msg1, ex);
            //            }
            //            ///
            //            ////////////////////////////////////////////////////////////////////////////////
            //            //
            //            //upUiMessage($"收到L2生产实绩电文！应答发送：{ clientHelper.Send(msg102608.ToBytes().ToArray())}");
            //            btn_ResponsePDO_Click(null, null);
            //            break;
            //        }
            //    case "261011":
            //        {
            //            upUiMessage($"收到L2停机实绩电文！");
            //            //
            //            ////////////////////////////////////////////////////////////////////////////////
            //            //
            //            //Msg261011
            //            try
            //            {
            //                //string infoStr = Encoding.ASCII.GetString(e.RequestInfo.MessageBody);
            //                string infoStr = Encoding.UTF8.GetString(e.RequestInfo.MessageBody);
            //                msg261011 = new Msg261011();
            //                //
            //                //电文头实际长度为 30
            //                //普通电文 电文体开始于47位
            //                //普通电文 电文产生时间(共17位)
            //                int iPos = 47 - 17;
            //                //
            //                msg261011.StartTime = infoStr.Substring(47 - iPos, 14).Trim();
            //                msg261011.Remark = infoStr.Substring(79 - iPos, 60).Trim();
            //                //
            //                LogHelper.Logger.Info("接收261011信息成功！");
            //            }
            //            catch (Exception ex)
            //            {
            //                string msg1 = $"接收261011信息错误！{ex.Message}";
            //                LogHelper.Logger.Error(msg1, ex);
            //            }
            //            ///
            //            ////////////////////////////////////////////////////////////////////////////////
            //            //
            //            break;
            //        }
            //    case "261012":
            //        {
            //            upUiMessage($"收到L2班报实绩电文！");
            //            //
            //            ////////////////////////////////////////////////////////////////////////////////
            //            //
            //            //Msg261012
            //            try
            //            {
            //                //string infoStr = Encoding.ASCII.GetString(e.RequestInfo.MessageBody);
            //                string infoStr = Encoding.UTF8.GetString(e.RequestInfo.MessageBody);
            //                msg261012 = new Msg261012();
            //                //
            //                //电文头实际长度为 30
            //                //普通电文 电文体开始于47位
            //                //普通电文 电文产生时间(共17位)
            //                int iPos = 47 - 17;
            //                //
            //                msg261012.Date = infoStr.Substring(47 - iPos, 8).Trim();
            //                msg261012.IngotsScrapWeightTotal = infoStr.Substring(95 - iPos, 7).Trim();
            //                //
            //                LogHelper.Logger.Info("接收261012信息成功！");
            //            }
            //            catch (Exception ex)
            //            {
            //                string msg1 = $"接收261012信息错误！{ex.Message}";
            //                LogHelper.Logger.Error(msg1, ex);
            //            }
            //            ///
            //            ////////////////////////////////////////////////////////////////////////////////
            //            //
            //            break;
            //        }
            //    case "261013":
            //        {
            //            upUiMessage($"收到L2异常数据电文！");
            //            //
            //            ////////////////////////////////////////////////////////////////////////////////
            //            //
            //            //Msg261013
            //            try
            //            {
            //                //string infoStr = Encoding.ASCII.GetString(e.RequestInfo.MessageBody);
            //                string infoStr = Encoding.UTF8.GetString(e.RequestInfo.MessageBody);
            //                msg261013 = new Msg261013();
            //                //
            //                //电文头实际长度为 30
            //                //普通电文 电文体开始于47位
            //                //普通电文 电文产生时间(共17位)
            //                int iPos = 47 - 17;
            //                //
            //                msg261013.ScheduleNo = infoStr.Substring(47 - iPos, 16).Trim();
            //                msg261013.SlabNo = infoStr.Substring(63 - iPos, 20).Trim();
            //                //
            //                LogHelper.Logger.Info("接收261013信息成功！");
            //            }
            //            catch (Exception ex)
            //            {
            //                string msg1 = $"接收261013信息错误！{ex.Message}";
            //                LogHelper.Logger.Error(msg1, ex);
            //            }
            //            ///
            //            ////////////////////////////////////////////////////////////////////////////////
            //            //
            //            //upUiMessage($"收到L2异常数据电文！应答发送：{ clientHelper.Send(msg102614.ToBytes().ToArray())}");
            //            btn_ResponseException_Click(null, null);
            //            break;
            //        }
            //    default:
            //        {
            //            upUiMessage($"未定义的电文，返回错误信息给服务器！");
            //            break;
            //        }
            //}

            //string msg = "T_PrimaryData       1288 PES     SlabScal3         2018-08-29 16:04:23    000000                                                                                                                                2018-08-29 16:04:21UP2018-08-29 16:04:21SCP01     2018-08-29 16:04:21006SlabID0829160421000100011                   POID08291604215551                      Alloy1    00000101000002010000021100000221000003010000040100000501000601000701000801SlabID0829160421000200022                   POID08291604215551                      Alloy2    00000102000002020000021200000222000003020000040200000502000602000702000802SlabID0829160421000300033                   POID08291604215551                      Alloy3    00000103000002030000021300000223000003030000040300000503000603000703000803SlabID0829160421000400044                   POID08291604215551                      Alloy4    00000104000002040000021400000224000003040000040400000504000604000704000804SlabID0829160421000500055                   POID08291604215551                      Alloy5    00000105000002050000021500000225000003050000040500000505000605000705000805SlabID0829160421000600066                   POID08291604215551                      Alloy6    00000106000002060000021600000226000003060000040600000506000606000706000806";
            //var iPEndPoint = serverHelper.ClientsIPEndPoint.ToList().First();

            //bool result = serverHelper.Send(iPEndPoint, System.Text.Encoding.ASCII.GetBytes(msg));

            //LogHelper.Logger.Debug($"发送结果：{result.ToString()}");

        }

        private void btn_SendPDI_Click(object sender, RoutedEventArgs e)
        {
            ////收到铣床PDI请求，发送pdi数据
            //Msg102602 msg102602 = new Msg102602();
            ////
            //DateTime dateTime = DateTime.Now;
            //msg102602.Header.DateOfSending = dateTime.ToString(ConstValue.Datetime_yyyyMMdd);
            //msg102602.Header.TimeOfSending = dateTime.ToString(ConstValue.Datetime_HHmmss);
            //msg102602.Header.GenerateTime = dateTime.ToString(ConstValue.Datetime_yyyyMMddHHmmssfff);
            ////
            //msg102602.ProcessResult = "0";
            ////
            //if (string.IsNullOrEmpty(msg102602.ScheduleNo))
            //{
            //    msg102602.ScheduleNo = DateTime.Now.ToString(Utility.ConstValue.Datetime_yyyyMMddHHmmss);
            //}
            ////
            //msg102602.ScheduleType = "0";
            //msg102602.BatchNo = "123456";
            ////
            //if (string.IsNullOrEmpty(msg261001.SlabNo))
            //{
            //    msg102602.SlabNo = DateTime.Now.ToString(Utility.ConstValue.Datetime_yyyyMMddHHmmssffff);
            //}
            //else
            //{
            //    msg102602.SlabNo = msg261001.SlabNo;
            //    msg261001 = new Msg261001();
            //}
            ////
            ////依据测试数据填写
            //msg102602.AlloyName = "5182";
            //msg102602.AlloyTemper = "36";

            //msg102602.SlabThickness = "5";
            //msg102602.SlabWidth = "800";
            //msg102602.SlabLength = "1000";
            //msg102602.SlabWeight = "2000";

            //msg102602.SlabThicknessTarget = "5";
            //msg102602.SlabWidthTarget = "800";
            //msg102602.TopCuttingDepth = "1";
            //msg102602.BottomCuttingDepth = "2";
            //msg102602.SideType = "1";
            //msg102602.SideMillingMode = "0";

            //msg102602.SideMillingDepth = "1";
            //msg102602.SideMillingAngle = "2";

            //msg102602.CastingNo = "3";
            //msg102602.WorkOrderNo = "123456789";
            //msg102602.FinalUse = "1234";
            //msg102602.CustomerCode = "1234";
            ////依据测试数据填写
            //msg102602.FormType = "99";
            ////
            //msg102602.RetryFlag = "1";
            //msg102602.Reserve = "1111111111222222222233333333334444444444123456789宁";
            ////
            //var data = msg102602.ToBytes().ToArray();
            //upUiMessage($"PDI数据已发送{ clientHelper.Send(data)}");
        }

        private void btn_DeletePdi_Click(object sender, RoutedEventArgs e)
        {
            ////铸锭计划删除
            //Msg102604 msg102604 = new Msg102604();
            ////
            //DateTime dateTime = DateTime.Now;
            //msg102604.Header.DateOfSending = dateTime.ToString(ConstValue.Datetime_yyyyMMdd);
            //msg102604.Header.TimeOfSending = dateTime.ToString(ConstValue.Datetime_HHmmss);
            //msg102604.Header.GenerateTime = dateTime.ToString(ConstValue.Datetime_yyyyMMddHHmmssfff);
            ////
            //msg102604.ScheduleNo = "0";
            //msg102604.SlabNo = SlabNo.Text.Trim();
            //msg102604.OperatorID = "123456";
            //msg102604.DeleteReasonCode = "错误下达";
            ////
            //var data = msg102604.ToBytes().ToArray();
            //upUiMessage($"铸锭计划删除数据已发送{ clientHelper.Send(data)}");
        }

        private void btn_ResponsePDO_Click(object sender, RoutedEventArgs e)
        {
            ////磨削实绩应答
            //Msg102608 msg102608 = new Msg102608();
            ////
            //DateTime dateTime = DateTime.Now;
            //msg102608.Header.DateOfSending = dateTime.ToString(ConstValue.Datetime_yyyyMMdd);
            //msg102608.Header.TimeOfSending = dateTime.ToString(ConstValue.Datetime_HHmmss);
            //msg102608.Header.GenerateTime = dateTime.ToString(ConstValue.Datetime_yyyyMMddHHmmssfff);
            ////
            //msg102608.ProcessResult = "0";
            //msg102608.ScheduleNo = msg261007.ScheduleNo;
            //msg102608.SlabNo = msg261007.SlabNo;
            //msg102608.Reserve = "收到";
            ////
            //var data = msg102608.ToBytes().ToArray();
            //upUiMessage($"磨削实绩应答数据已发送{ clientHelper.Send(data)}");
        }



        private void btn_ResponseException_Click(object sender, RoutedEventArgs e)
        {
            ////异常应答
            //Msg102614 msg102614 = new Msg102614();
            ////
            //DateTime dateTime = DateTime.Now;
            //msg102614.Header.DateOfSending = dateTime.ToString(ConstValue.Datetime_yyyyMMdd);
            //msg102614.Header.TimeOfSending = dateTime.ToString(ConstValue.Datetime_HHmmss);
            //msg102614.Header.GenerateTime = dateTime.ToString(ConstValue.Datetime_yyyyMMddHHmmssfff);
            ////
            //msg102614.ProcessResult = "0";
            //msg102614.ScheduleNo = msg261013.ScheduleNo;
            //msg102614.SlabNo = msg261013.SlabNo;
            //msg102614.Reserve = "收到";
            ////
            //var data = msg102614.ToBytes().ToArray();
            //upUiMessage($"异常应答数据已发送{ clientHelper.Send(data)}");
        }

        private void btn_Connect_Click(object sender, RoutedEventArgs e)
        {
            if (listingFlag)
            {
                this.Dispatcher.Invoke(() =>
                {
                    serverHelper.Stop();
                    listingFlag = false;
                    this.btn_Connect.Content = "启动服务";
                });
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    initSocketServer();
                    //
                    listingFlag = serverHelper.Start(int.Parse(txt_Port.Text));
                    this.btn_Connect.Content = "停止服务";
                });

            }
        }

        //private void btn_Send_Click(object sender, RoutedEventArgs e)
        //{
        //    string msg = "T_PrimaryData       1288 PES     SlabScal3         2018-08-29 16:04:23    000000                                                                                                                                2018-08-29 16:04:21UP2018-08-29 16:04:21SCP01     2018-08-29 16:04:21006SlabID0829160421000100011                   POID08291604215551                      Alloy1    00000101000002010000021100000221000003010000040100000501000601000701000801SlabID0829160421000200022                   POID08291604215551                      Alloy2    00000102000002020000021200000222000003020000040200000502000602000702000802SlabID0829160421000300033                   POID08291604215551                      Alloy3    00000103000002030000021300000223000003030000040300000503000603000703000803SlabID0829160421000400044                   POID08291604215551                      Alloy4    00000104000002040000021400000224000003040000040400000504000604000704000804SlabID0829160421000500055                   POID08291604215551                      Alloy5    00000105000002050000021500000225000003050000040500000505000605000705000805SlabID0829160421000600066                   POID08291604215551                      Alloy6    00000106000002060000021600000226000003060000040600000506000606000706000806";
        //    bool result = serverHelper.Send(serverHelper.ClientsIPEndPoint.FirstOrDefault(), System.Text.Encoding.ASCII.GetBytes(msg));

        //    this.Title = result.ToString();
        //    //msg = "T_PrimaryData       1120 PES     SlabScal2         2018-08-29 16:03:32    000000                                                                                                                                2018-08-29 16:03:30UP2018-08-29 16:03:30SCP01     2018-08-29 16:03:30005SlabID0829160330000100011                   POID08291603306859                      Alloy1    00000101000002010000021100000221000003010000040100000501000601000701000801SlabID0829160330000200022                   POID08291603306859                      Alloy2    00000102000002020000021200000222000003020000040200000502000602000702000802SlabID0829160330000300033                   POID08291603306859                      Alloy3    00000103000002030000021300000223000003030000040300000503000603000703000803SlabID0829160330000400044                   POID08291603306859                      Alloy4    00000104000002040000021400000224000003040000040400000504000604000704000804SlabID0829160330000500055                   POID08291603306859                      Alloy5    00000105000002050000021500000225000003050000040500000505000605000705000805";
        //}




    }
}
