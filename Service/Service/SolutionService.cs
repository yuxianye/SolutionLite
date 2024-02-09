using OpcUaHelper;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Utility;

namespace Service
{
    /// <summary>
    /// 解决方案服务运行在服务端，包含api服务，socket服务（用于远程控制，以及动态更新设备通讯等）
    /// 设备通讯配置修改后，执行命令，然后刷新设备通讯配置
    /// </summary>
    partial class SolutionService : ServiceBase
    {
        public SolutionService()
        {
            InitializeComponent();
        }

        protected async override void OnStart(string[] args)
        {
            await StartService();
        }

        public async Task<bool> StartService()
        {
            //telnet服务器，用于远程控制
            telnetServerHelper.OnSessionClosed += TelnetServerHelper_OnSessionClosed;
            telnetServerHelper.OnSessionConnected += TelnetServerHelper_OnSessionConnected;
            telnetServerHelper.OnRequestReceived += TelnetServerHelper_OnRequestReceived;
            int serverPort = 13805;
            int.TryParse(Utility.ConfigHelper.GetAppSetting("ServerPort"), out serverPort);
            bool resultSocket = telnetServerHelper.Start(serverPort);


            var resultApi = StartApiService();
            //启动调度服务
            bool resultDispathing = await StartDispatchingService();

            return resultApi && resultDispathing && resultSocket;
        }

        private bool isLogin = false;

        private async void TelnetServerHelper_OnRequestReceived(object sender, SocketHelper.TelnetServer.RequestEventArgs e)
        {
            LogHelper.Logger.Info($"收到请求：{e.IPEndPoint} {e.RequestInfo.Key}");
            if (e.RequestInfo.Key.ToUpper() == "LOGIN")
            {
                string userName = e.RequestInfo.Parameters[0];
                string userPassword = e.RequestInfo.Parameters[1];
                if (userName == Utility.ConfigHelper.GetAppSetting("TelnetUserName") && userPassword == Utility.Security.AESDecrypt(Utility.ConfigHelper.GetAppSetting("TelnetUserPassword")))
                {
                    isLogin = true;
                    telnetServerHelper.Send(e.IPEndPoint, $"login success");
                    return;
                }
            }
            else if (e.RequestInfo.Key.ToUpper() == "HELP")
            {
                try
                {
                    telnetServerHelper.Send(e.IPEndPoint, $"HELP:帮助命令");
                    telnetServerHelper.Send(e.IPEndPoint, $"LOGIN username password:登陆命令");
                    telnetServerHelper.Send(e.IPEndPoint, $"");
                    telnetServerHelper.Send(e.IPEndPoint, $"API_START:开启webapi服务");
                    telnetServerHelper.Send(e.IPEndPoint, $"API_STOP:停止webapi服务");
                    telnetServerHelper.Send(e.IPEndPoint, $"API_RESTART:重启webapi服务");
                    telnetServerHelper.Send(e.IPEndPoint, $"");
                    telnetServerHelper.Send(e.IPEndPoint, $"UA_START:开启UA通讯和调度服务");
                    telnetServerHelper.Send(e.IPEndPoint, $"UA_STOP:停止UA通讯和调度服务");
                    telnetServerHelper.Send(e.IPEndPoint, $"UA_RESTART:重启UA通讯和调度服务");
                    telnetServerHelper.Send(e.IPEndPoint, $"UA_UPDATE:动态更新UA节点");
                }
                catch (Exception ex)
                {
                    LogHelper.Logger.Error($"API_RESTART错误：{e.IPEndPoint} {e.RequestInfo.Key}", ex);
                    telnetServerHelper.Send(e.IPEndPoint, ex.Message);
                }

            }
            if (!isLogin)
            {
                telnetServerHelper.Send(e.IPEndPoint, $"invalid user name or password 无效的用户名和密码");
                return;
            }
            switch (e.RequestInfo.Key.ToUpper())
            {

                #region API
                case ("API_START"):
                    try
                    {
                        bool result = StartApiService();
                        telnetServerHelper.Send(e.IPEndPoint, $"API_START Result:{result}");
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Logger.Error($"API_START错误：{e.IPEndPoint} {e.RequestInfo.Key}", ex);
                        telnetServerHelper.Send(e.IPEndPoint, ex.Message);
                    }
                    break;
                case ("API_STOP"):
                    try
                    {
                        httpSelfHostServer?.CloseAsync()?.Wait();
                        telnetServerHelper.Send(e.IPEndPoint, $"API_STOP Result:True");
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Logger.Error($"API_STOP错误：{e.IPEndPoint} {e.RequestInfo.Key}", ex);
                        telnetServerHelper.Send(e.IPEndPoint, ex.Message);
                    }
                    break;
                case ("API_RESTART"):
                    try
                    {
                        httpSelfHostServer?.CloseAsync()?.Wait();
                        bool result = StartApiService();
                        telnetServerHelper.Send(e.IPEndPoint, $"API_RESTART Result:{result}");
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Logger.Error($"API_RESTART错误：{e.IPEndPoint} {e.RequestInfo.Key}", ex);
                        telnetServerHelper.Send(e.IPEndPoint, ex.Message);
                    }
                    break;
                #endregion

                #region UA Dispatching
                case "UA_START":
                    try
                    {
                        //启动调度服务
                        bool result = await StartDispatchingService();
                        telnetServerHelper.Send(e.IPEndPoint, $"UA_START Result:{result}");
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Logger.Error($"UA_START错误：{e.IPEndPoint} {e.RequestInfo.Key}", ex);
                        telnetServerHelper.Send(e.IPEndPoint, ex.Message);
                    }
                    break;
                case "UA_STOP":
                    try
                    {
                        OpcUaClientHelperList?.ForEach(async (a) => { await a.DisConnectAsync(); a.Dispose(); });
                        OpcUaClientHelperList.Clear();
                        telnetServerHelper.Send(e.IPEndPoint, $"UA_STOP Result:True");
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Logger.Error($"UA_STOP错误：{e.IPEndPoint} {e.RequestInfo.Key}", ex);
                        telnetServerHelper.Send(e.IPEndPoint, ex.Message);
                    }
                    break;
                case "UA_RESTART":
                    try
                    {
                        stopDispathing();
                        bool result = await StartDispatchingService();
                        telnetServerHelper.Send(e.IPEndPoint, $"UA_RESTART Result:{result}");
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Logger.Error($"UA_RESTART错误：{e.IPEndPoint} {e.RequestInfo.Key}", ex);
                        telnetServerHelper.Send(e.IPEndPoint, ex.Message);
                    }
                    break;
                case "UA_UPDATE":
                    try
                    {
                        bool result = await StartDispatchingService();
                        telnetServerHelper.Send(e.IPEndPoint, $"UA_UPDATE Result:{result}");
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Logger.Error($"UA_UPDATE错误：{e.IPEndPoint} {e.RequestInfo.Key}", ex);
                        telnetServerHelper.Send(e.IPEndPoint, ex.Message);
                    }
                    break;
                #endregion


                default:
                    telnetServerHelper.Send(e.IPEndPoint, "unknown cmd 未知命令");
                    break;
            }
        }

        private void TelnetServerHelper_OnSessionConnected(object sender, SocketHelper.TelnetServer.ConnectEventArgs e)
        {
            LogHelper.Logger.Info($"会话连接：{e.IPEndPoint}");
        }

        private void TelnetServerHelper_OnSessionClosed(object sender, SocketHelper.TelnetServer.ClosedEventArgs e)
        {
            LogHelper.Logger.Info($"会话关闭：{e.IPEndPoint}");
        }

        /// <summary>
        /// 
        /// </summary>
        private Dal.DbContext dbContext = new Dal.DbContext();

        /// <summary>
        /// 寄宿服务
        /// </summary>
        private HttpSelfHostServer httpSelfHostServer;

        private SocketHelper.TelnetServer.TelnetServerHelper telnetServerHelper = new SocketHelper.TelnetServer.TelnetServerHelper();

        /// <summary>
        /// 
        /// </summary>
        public static List<OpcUaClientHelper> OpcUaClientHelperList { get; set; } = new List<OpcUaClientHelper>();
        /// <summary>
        /// 开启web api服务
        /// </summary>
        /// <returns></returns>
        private bool StartApiService()
        {
            try
            {
                LogHelper.Logger.Info("Api服务开始启动");
                var thisServerUri = Utility.ConfigHelper.GetAppSetting("ThisServerUri");
                LogHelper.Logger.Info("成功读取Api配置文件服务器地址：" + thisServerUri);
                var config = new HttpSelfHostConfiguration(thisServerUri);
                config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
                LogHelper.Logger.Info("路由配置：" + "DefaultApi," + "api/{controller}/{id}");
                httpSelfHostServer = new HttpSelfHostServer(config);
                httpSelfHostServer?.OpenAsync()?.Wait();
                LogHelper.Logger.Info("Api服务启动成功");
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("开启WebApi时发生错误。", ex);
                return false;
            }
        }

        /// <summary>
        /// 开启设备调度服务，重复调用时将动态更新设备和相关的点表
        /// </summary>
        /// <returns></returns>
        private async Task<bool> StartDispatchingService()
        {
            try
            {
                LogHelper.Logger.Info("调度服务开始启动");

                foreach (var opcServer in dbContext.OpcServerDb.GetList(a => a.IsEnable == true && a.OpcType == Models.OpcType.OpcUa))
                {
                    //已经有此ua服务器，那么更新节点
                    if (OpcUaClientHelperList.Any(a => a.Name == opcServer.Name))
                    {
                        var opcUaClientHelper = OpcUaClientHelperList.FirstOrDefault(a => a.Name == opcServer.Name);
                        var nodes = dbContext.DeviceNodeDb.GetList(a => a.IsEnable == true && a.OpcServerId == opcServer.Id);
                        var nd = from a in nodes
                                 select new OpcUaHelper.OpcUaDataItem
                                 {
                                     Name = a.Name.StartsWith("ns=2;s=") ? a.Name : "ns=2;s=" + a.Name,
                                     UpdateRate = a.UpdateRate,
                                     ValueType = ((TypeCode)a.DataType).ToType(),
                                     OpcUaStatusCodes = OpcUaStatusCodes.Bad
                                 };
                        var result = await opcUaClientHelper.RegisterNodes(nd.ToList());
                        LogHelper.Logger.Info($"更新节点结果：{result}，{opcServer.Name}，{opcServer.Uri}");
                    }
                    else//没有此ua服务器，那么新增
                    {
                        var nodes = dbContext.DeviceNodeDb.GetList(a => a.IsEnable == true && a.OpcServerId == opcServer.Id);
                        OpcUaHelper.OpcUaClientHelper opcUaClientHelper = new OpcUaHelper.OpcUaClientHelper();
                        OpcUaClientHelperList.Add(opcUaClientHelper);
                        opcUaClientHelper.Name = opcServer.Name;
                        opcUaClientHelper.ServerUri = opcServer.Uri;
                        opcUaClientHelper.OnLogHappened += OpcUaClientHelper_OnLogHappened;
                        opcUaClientHelper.OnErrorHappened += OpcUaClientHelper_OnErrorHappened;
                        opcUaClientHelper.OnDataChanged += OpcUaClientHelper_OnDataChanged;
                        DispatchingHandler = new dispatchingDelegate(dispatchingExecute);
                        OpcUaHelper.OpcUaStatusCodes opcUaStatusCodes = await opcUaClientHelper.ConnectAsync();
                        LogHelper.Logger.Info($"调度服务启动结果：{opcUaStatusCodes}，{opcServer.Name}，{opcServer.Uri}");
                        var nd = from a in nodes
                                 select new OpcUaHelper.OpcUaDataItem
                                 {
                                     Name = a.Name.StartsWith("ns=2;s=") ? a.Name : "ns=2;s=" + a.Name,
                                     UpdateRate = a.UpdateRate,
                                     ValueType = ((TypeCode)a.DataType).ToType(),
                                     OpcUaStatusCodes = OpcUaStatusCodes.Bad
                                 };
                        await opcUaClientHelper.RegisterNodes(nd.ToList());
                    }
                }

                LogHelper.Logger.Info("调度服务启动成功");
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("", ex);
                return false;
            }
        }

        private delegate bool dispatchingDelegate(object sender, OpcUaHelper.OpcUaDataEventArgs opcUaDataEventArgs);

        private dispatchingDelegate DispatchingHandler;

        //Dal.DbContext dbContext = new Dal.DbContext();


        private bool dispatchingExecute(object sender, OpcUaDataEventArgs opcUaDataEventArgs)
        {
            //LogHelper.Logger.Info($"日志数量：{opcUaDataEventArgs}");
            //opcUaDataEventArgs.
            //System.Threading.Thread.Sleep(2000);
            //LogHelper.Logger.Info($"日志数量：{dbContext.AppLogDb.AsQueryable().Count()}");
            //this.opcUaClientHelperList;
            //opcUaDataEventArgs.
            Task.Factory.StartNew(() =>
            {
                //LogHelper.Logger.Info($"{sender.ToString()},节点：{opcUaDataEventArgs.OpcUaDataItem.ToString()}");

                if (opcUaDataEventArgs.OpcUaDataItem.Name.Equals("ns=2;s=数据类型示例.8 位设备.R 寄存器.Boolean1")
                    && opcUaDataEventArgs.OpcUaDataItem.OldValue?.ToString() != "1"
                    && opcUaDataEventArgs.OpcUaDataItem.NewValue.ObjToBool() == true)
                {
                    //值变成了1，开始读取记录数据，处理业务逻辑 
                    if (sender is OpcUaClientHelper)
                    {
                        var opcUaClientHelper = (OpcUaClientHelper)sender;
                        dbContext.DeviceNodeDb.GetList(a => a.Name == "");
                        LogHelper.Logger.Info($"已完工，读取数据节点：{opcUaDataEventArgs.OpcUaDataItem.ToString()}");

                        //opcUaClientHelper
                        //opcUaDataEventArgs.
                    }



                }


                //switch (opcUaDataEventArgs.OpcUaDataItem.Name)
                //{
                //    case "Short1":
                //        short? value = opcUaDataEventArgs.OpcUaDataItem.NewValue as Int16?;
                //        if (value.HasValue && value == 1)
                //        {
                //            //sender
                //        }

                //        break;

                //}
            });

            return false;
        }

        private void OpcUaClientHelper_OnDataChanged(object sender, OpcUaHelper.OpcUaDataEventArgs e)
        {

            Func<List<OpcUaClientHelper>, int, string> func = (a, b) =>
            {
                LogHelper.Logger.Debug("12113321");

                return "asd";
            };

            //DispatchingHandler = new dispatchingDelegate(dispatchingExecute);
            DispatchingHandler.BeginInvoke(sender, e, new AsyncCallback((a) =>
             {
                 if (a.IsCompleted)
                 {
                     try
                     {
                         //LogHelper.Logger.Debug(e.OpcUaDataItem.NewValue.ToString());


                         bool result = DispatchingHandler.EndInvoke(a);
                     }
                     catch (Exception ex)
                     {
                         LogHelper.Logger.Error("OPCUA数据回调错误", ex);
                     }
                 }
             }), null);
        }

        private void OpcUaClientHelper_OnErrorHappened(object sender, OpcUaHelper.OpcUaErrorEventArgs e)
        {
            LogHelper.Logger.Error("OPCUA通讯发生错误", e.Exception);
        }

        private void OpcUaClientHelper_OnLogHappened(object sender, OpcUaHelper.OpcUaLogEventArgs e)
        {
            LogHelper.Logger.Info(e.Log);
        }

        private void stopDispathing()
        {
            //关闭opcua服务
            OpcUaClientHelperList?.ForEach(async (a) =>
            {
                a.OnErrorHappened -= OpcUaClientHelper_OnErrorHappened;
                a.OnDataChanged -= OpcUaClientHelper_OnDataChanged;
                await a.DisConnectAsync();
                a.OnLogHappened -= OpcUaClientHelper_OnLogHappened;
                a.Dispose();
            });
            OpcUaClientHelperList.Clear();
        }

        protected override void OnStop()
        {
            //关闭服务
            httpSelfHostServer?.CloseAsync()?.Wait();
            httpSelfHostServer = null;
            stopDispathing();
            telnetServerHelper.Stop();
            telnetServerHelper.OnSessionClosed -= TelnetServerHelper_OnSessionClosed;
            telnetServerHelper.OnSessionConnected -= TelnetServerHelper_OnSessionConnected;
            telnetServerHelper.OnRequestReceived -= TelnetServerHelper_OnRequestReceived;
        }
    }
}
