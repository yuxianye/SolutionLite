using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpcUaHelper
{
    /// <summary>
    /// OpcUa设备帮助类
    /// </summary>
    public class OpcUaClientHelper
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OpcUaClientHelper()
        {
            initdaemonTimer();
        }

        #region Private Fields

        /// <summary>
        /// Ua应用实例
        /// </summary>
        private ApplicationInstance applicationInstance;

        /// <summary>
        /// Ua应用配置
        /// </summary>
        private ApplicationConfiguration applicationConfiguration;

        /// <summary>
        /// Ua会话
        /// </summary>
        private Session session;

        /// <summary>
        /// 守护Timer
        /// </summary>
        private System.Timers.Timer daemonTimer = new System.Timers.Timer();

        /// <summary>
        /// 自动接受
        /// </summary>
        private bool autoAccept = true;

        /// <summary>
        /// 
        /// </summary>
        private bool haveAppCertificate;

        /// <summary>
        /// 守护Timer间隔
        /// </summary>
        private int daemonInterval = 60 * 1000;

        #endregion

        /// <summary>
        /// 初始化守护timer
        /// </summary>
        private void initdaemonTimer()
        {
            daemonTimer.Interval = DaemonInterval;
            daemonTimer.Elapsed += DaemonTimer_Elapsed;
            daemonTimer.AutoReset = true;
        }

        /// <summary>
        /// 守护Timer回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DaemonTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //如果已经连接那么注册数据点
            if (IsConnected)
            {
                await reRegisterNodes();
            }
        }


        #region public 属性 事件
        /// <summary>
        /// 守护时间，默认60秒。
        /// </summary>
        public int DaemonInterval
        {
            get { return daemonInterval; }
            set
            {
                daemonInterval = value;
                daemonTimer.Interval = value;
            }
        }
        /// <summary>
        /// 数据改变事件
        /// </summary>
        public event EventHandler<OpcUaDataEventArgs> OnDataChanged;

        /// <summary>
        /// 异常事件
        /// </summary>
        public event EventHandler<OpcUaErrorEventArgs> OnErrorHappened;

        /// <summary>
        /// 日志消息事件
        /// </summary>
        public event EventHandler<OpcUaLogEventArgs> OnLogHappened;

        /// <summary>
        /// 会话重新连接句柄
        /// </summary>
        private SessionReconnectHandler sessionReconnectHandler;

        /// <summary>
        /// 是否连接
        /// </summary>
        public bool IsConnected { get; private set; } = false;

        /// <summary>
        /// 重连周期，默认10秒。
        /// </summary>
        public int ReconnectPeriod { get; set; } = 10;

        /// <summary>
        /// 地址默认opc.tcp://192.168.1.198:49320
        /// </summary>
        public string ServerUri { get; set; } = "opc.tcp://127.0.0.1:49320";

        /// <summary>
        /// 名称，OpcUaClient
        /// </summary>
        public string Name { get; set; } = "OpcUaClient";

        /// <summary>
        /// 节点数据
        /// </summary>
        public List<OpcUaDataItem> OpcUaDataItems = new List<OpcUaDataItem>();

        #endregion

        #region 重连和验证事件相关
        private void OnCertificateValidatorNotification(CertificateValidator sender, CertificateValidationEventArgs e)
        {
            if (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted)
            {
                e.Accept = autoAccept;
                if (autoAccept)
                {
                    OnLogHappened?.Invoke(this, new OpcUaLogEventArgs($"{ToString()}，Accepted Certificate: {e.Certificate.Subject}"));
                }
                else
                {
                    OnLogHappened?.Invoke(this, new OpcUaLogEventArgs($"{ToString()}，Rejected Certificate: {e.Certificate.Subject}"));
                }
            }
        }

        /// <summary>
        /// 处理保持连接心跳事件
        /// </summary>
        private void OnKeepAliveNotification(Session session, KeepAliveEventArgs e)
        {
            try
            {
                if (e.Status != null && ServiceResult.IsNotGood(e.Status))
                {
                    IsConnected = false;
                    if (ReconnectPeriod <= 0)
                    {
                        OnErrorHappened?.Invoke(this, new OpcUaErrorEventArgs((OpcUaStatusCodes)e.Status.Code, $"{ToString()}，保持连接时错误", null));
                        return;
                    }
                    if (sessionReconnectHandler == null)
                    {
                        OnLogHappened?.Invoke(this, new OpcUaLogEventArgs($"{ToString()}，重新连接中... "));
                        sessionReconnectHandler = new SessionReconnectHandler();
                        sessionReconnectHandler.BeginReconnect(session, ReconnectPeriod * 1000, Server_ReconnectComplete);
                    }
                }
            }
            catch (Exception ex)
            {
                OnErrorHappened?.Invoke(this, new OpcUaErrorEventArgs(OpcUaStatusCodes.Uncertain, $"{ToString()}，保持连接时发生未处理的异常", ex));
            }
        }

        /// <summary>
        /// 处理OPC UA服务器重连事件
        /// </summary>
        private void Server_ReconnectComplete(object sender, EventArgs e)
        {
            try
            {
                // ignore callbacks from discarded objects.
                if (!Object.ReferenceEquals(sender, sessionReconnectHandler))
                {
                    OnLogHappened?.Invoke(this, new OpcUaLogEventArgs($"{ToString()}，ignore callbacks from discarded objects"));
                    return;
                }
                session = sessionReconnectHandler.Session;
                sessionReconnectHandler.Dispose();
                sessionReconnectHandler = null;
                OnLogHappened(this, new OpcUaLogEventArgs($"{ToString()}，重新连接成功"));
                IsConnected = true;
            }
            catch (Exception ex)
            {
                OnErrorHappened?.Invoke(this, new OpcUaErrorEventArgs(OpcUaStatusCodes.Uncertain, $"{ToString()}，重新连接完成时发生未处理的异常", ex));
            }
        }

        #endregion

        ///// <summary>
        ///// 连接
        ///// </summary>
        ///// <typeparam name="Tin">集成自<seealso cref="DeviceConnectParamEntityBase"/>类型的连接参数</typeparam>
        ///// <typeparam name="Tout">集成自<seealso cref="DeviceConnectParamEntityBase"/>类型的输出参数</typeparam>
        ///// <param name="connectParam">连接参数</param>
        ///// <returns></returns>
        //public Task<Tout> Connect2<Tin, Tout>(Tin connectParam) where Tin : DeviceConnectParamEntityBase where Tout : DeviceConnectParamEntityBase
        //{
        //    var connectP = connectParam as OpcUaDeviceConnectParamEntity;
        //    serverUrl = connectP.DeviceUrl;
        //    return Task.Run(() =>
        //    {
        //        try
        //        {
        //            //实例化应用实例
        //            applicationInstance = new ApplicationInstance
        //            {
        //                ApplicationName = "OpcUaDeviceClient",
        //                ApplicationType = ApplicationType.Client,
        //                ConfigSectionName = "Solution.OpcUaClient"
        //            };
        //            //加载配置文件

        //            //var vv = System.AppDomain.CurrentDomain.RelativeSearchPath ;

        //            //vv = Assembly.GetEntryAssembly().Location;
        //            string configFilePath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.RelativeSearchPath ?? "", "Solution.OpcUaClient.Config.xml");

        //            //string configFilePath = Assembly.GetEntryAssembly().Location.Substring(0, Assembly.GetEntryAssembly().Location.LastIndexOf('\\') + 1)
        //            //+ "Solution.OpcUaClient.Config.xml";
        //            if (!File.Exists(configFilePath))
        //            {
        //                return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.BadConfigurationError, DeviceUrl = connectP.DeviceUrl, Message = $"未找到配置文件：{configFilePath}！" } as object);
        //            }

        //            applicationConfiguration = applicationInstance.LoadApplicationConfiguration(configFilePath, true).Result;
        //            if (Equals(applicationConfiguration, null))
        //            {
        //                //配置文件错误
        //                return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.BadConfigurationError, DeviceUrl = connectP.DeviceUrl, Message = "配置文件加载失败！" } as object);
        //            }
        //            //检查安全性
        //            haveAppCertificate = applicationInstance.CheckApplicationInstanceCertificate(true, 0).Result;
        //            if (!haveAppCertificate)
        //            {
        //                //未授权则返回认证无效
        //                return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.BadCertificateInvalid, DeviceUrl = connectP.DeviceUrl } as object);
        //            }
        //            //认证通过,是否自动接受
        //            if (haveAppCertificate)
        //            {
        //                applicationConfiguration.ApplicationUri = Utils.GetApplicationUriFromCertificate(applicationConfiguration.SecurityConfiguration.ApplicationCertificate.Certificate);
        //                if (applicationConfiguration.SecurityConfiguration.AutoAcceptUntrustedCertificates)
        //                {
        //                    autoAccept = true;
        //                }
        //                applicationConfiguration.CertificateValidator.CertificateValidation += new CertificateValidationEventHandler(OnCertificateValidatorNotification);
        //            }
        //            //else
        //            //{

        //            //    //未授权则返回认证无效
        //            //    return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.BadCertificateInvalid } as object);
        //            //    //Console.WriteLine("    WARN: missing application certificate, using unsecure connection.");

        //            //}

        //            //断开现有连接
        //            if (session != null)
        //            {
        //                var sc = session.Close(10000);
        //                Notification(this, new DeviceEventArgs<IDeviceParam>(new OpcUaDeviceConnectParamEntity() { DeviceUrl = connectP.DeviceUrl, StatusCode = sc.Code, Message = "" }));
        //                session = null;
        //                return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.BadSessionNotActivated, DeviceUrl = connectP.DeviceUrl } as object);
        //            }
        //            if (applicationConfiguration == null)
        //            {
        //                return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.BadConfigurationError, DeviceUrl = connectP.DeviceUrl } as object);
        //            }

        //            var selectedEndpoint = CoreClientUtils.SelectEndpoint(connectP.DeviceUrl, haveAppCertificate, 15000);
        //            var endpointConfiguration = EndpointConfiguration.Create(applicationConfiguration);
        //            var endpoint = new ConfiguredEndpoint(null, selectedEndpoint, endpointConfiguration);
        //            session = Session.Create(applicationConfiguration, endpoint, false, "OpcUaDeviceClient", 60000, new UserIdentity(new AnonymousIdentityToken()), null).Result;
        //            if (session == null)
        //            {
        //                IsConnected = true;
        //                //会话初始化失败
        //                return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.BadSessionNotActivated, DeviceUrl = connectP.DeviceUrl } as object);
        //            }
        //            else
        //            {
        //                // 保持连接句柄
        //                session.KeepAlive += new KeepAliveEventHandler(OnKeepAliveNotification);
        //                IsConnected = true;
        //                return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.Good, DeviceUrl = connectP.DeviceUrl, Message = "连接成功！" } as object);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.BadUnexpectedError, DeviceUrl = connectP.DeviceUrl, Message = ex.Message + ex.InnerException?.Message + ex.InnerException?.StackTrace + ex.StackTrace } as object);
        //        }
        //    });
        //}
        public static readonly string AppPath = Assembly.GetEntryAssembly().Location.Substring(0, Assembly.GetEntryAssembly().Location.LastIndexOf('\\') + 1);

        #region  连接 断开

        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        public async Task<OpcUaStatusCodes> ConnectAsync()
        {
            return await Task.Run(() =>
            {
                string configFilePath = System.IO.Path.Combine(AppPath ?? "", "OpcUaClient.Config.xml");
                try
                {
                    var certificateValidator = new CertificateValidator();
                    certificateValidator.CertificateValidation += OnCertificateValidatorNotification;
                    applicationInstance = new ApplicationInstance
                    {
                        ApplicationType = ApplicationType.Client,
                        ConfigSectionName = "OpcUaClient",
                        ApplicationConfiguration = new ApplicationConfiguration
                        {
                            ApplicationUri = "",
                            ApplicationName = Name,
                            ApplicationType = ApplicationType.Client,
                            CertificateValidator = certificateValidator,
                            ServerConfiguration = new ServerConfiguration
                            {
                                MaxSubscriptionCount = 100000,
                                MaxMessageQueueSize = 100000,
                                MaxNotificationQueueSize = 1002,
                                MaxPublishRequestCount = 100000
                            },
                            SecurityConfiguration = new SecurityConfiguration
                            {
                                AutoAcceptUntrustedCertificates = true,
                            },
                            TransportQuotas = new TransportQuotas
                            {
                                OperationTimeout = 600000,
                                MaxStringLength = 1048576,
                                MaxByteStringLength = 1048576,
                                MaxArrayLength = 65535,
                                MaxMessageSize = 4194304,
                                MaxBufferSize = 65535,
                                ChannelLifetime = 600000,
                                SecurityTokenLifetime = 3600000
                            },
                            ClientConfiguration = new ClientConfiguration
                            {
                                DefaultSessionTimeout = 60000,
                                MinSubscriptionLifetime = 10000
                            },
                            DisableHiResClock = true
                        }
                    };

                    //断开现有连接
                    if (session != null)
                    {
                        var sc = session.Close(10000);
                        session = null;
                        OnLogHappened?.Invoke(this, new OpcUaLogEventArgs($"{ToString()}，已断开现有会话"));
                        return OpcUaStatusCodes.Good;

                    }
                    applicationConfiguration = applicationInstance.ApplicationConfiguration;
                    var selectedEndpoint = CoreClientUtils.SelectEndpoint(ServerUri, haveAppCertificate, 15000);
                    var endpointConfiguration = EndpointConfiguration.Create(applicationConfiguration);
                    var endpoint = new ConfiguredEndpoint(null, selectedEndpoint, endpointConfiguration);
                    session = Session.Create(applicationConfiguration, endpoint, false, "OpcUaDeviceClient", 60000, new UserIdentity(new AnonymousIdentityToken()), null).Result;
                    if (session == null)
                    {
                        OnLogHappened?.Invoke(this, new OpcUaLogEventArgs($"{ToString()}，创建会话失败"));
                        IsConnected = true;
                        //会话初始化失败
                        return OpcUaStatusCodes.BadSessionIdInvalid;
                    }
                    else
                    {
                        // 保持连接句柄
                        session.KeepAlive += new KeepAliveEventHandler(OnKeepAliveNotification);
                        IsConnected = true;
                        OnLogHappened?.Invoke(this, new OpcUaLogEventArgs($"{ToString()}，创建会话成功"));

                        //连接成功后开启守护进程
                        daemonTimer.Enabled = true;
                        daemonTimer.Start();
                        return OpcUaStatusCodes.Good;
                    }
                }
                catch (ServiceResultException e)
                {
                    OnErrorHappened?.Invoke(this, new OpcUaErrorEventArgs((OpcUaStatusCodes)e.StatusCode, $"{ToString()}，连接服务器时错误", e));
                    return OpcUaStatusCodes.Uncertain;

                }
                catch (AggregateException ae)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var v in ae.InnerExceptions)
                    {
                        //var ex = v as ServiceResultException;
                        sb.AppendLine(v.Message);
                    }
                    OnErrorHappened?.Invoke(this, new OpcUaErrorEventArgs(OpcUaStatusCodes.Uncertain, $"{ToString()}，连接服务器时发生错误 {ae?.Message} {sb.ToString()}", ae));
                    sb.Clear();
                    sb = null;
                    return OpcUaStatusCodes.Uncertain;
                }
                catch (Exception ex)
                {
                    OnErrorHappened?.Invoke(this, new OpcUaErrorEventArgs(OpcUaStatusCodes.Uncertain, $"{ToString()}，未处理的异常", ex));
                    return OpcUaStatusCodes.Uncertain;
                }
            });

        }

        /// <summary>
        /// 断开
        /// </summary>
        /// <returns></returns>
        public async Task<OpcUaStatusCodes> DisConnectAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    // stop any reconnect operation.
                    if (sessionReconnectHandler != null)
                    {
                        sessionReconnectHandler.Dispose();
                        sessionReconnectHandler = null;
                    }
                    //断开现有连接
                    if (session != null)
                    {
                        var sc = session.Close(10000);
                        session.Dispose();
                        session = null;
                        IsConnected = false;
                        OnLogHappened?.Invoke(this, new OpcUaLogEventArgs($"{ToString()}，关闭连接成功"));
                        return OpcUaStatusCodes.Good;
                    }
                    else
                    {
                        IsConnected = false;
                        return OpcUaStatusCodes.Good;
                    }

                }
                catch (Exception ex)
                {
                    return OpcUaStatusCodes.Uncertain;
                }
                finally
                {
                    daemonTimer.Enabled = false;
                    daemonTimer.Stop();
                }
            });
        }

        #endregion

        #region 写入数据

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="opcUaDataItem"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public async Task<OpcUaStatusCodes> Write(OpcUaDataItem opcUaDataItem, object newValue)
        {
            return await Task.Run(() =>
            {
                WriteValue valueToWrite = new WriteValue()
                {
                    NodeId = new NodeId(opcUaDataItem.Name),
                    AttributeId = Attributes.Value
                };
                valueToWrite.Value.Value = Convert.ChangeType(newValue, opcUaDataItem.ValueType ?? typeof(object));
                valueToWrite.Value.StatusCode = StatusCodes.Good;
                valueToWrite.Value.ServerTimestamp = DateTime.MinValue;
                valueToWrite.Value.SourceTimestamp = DateTime.MinValue;
                WriteValueCollection valuesToWrite = new WriteValueCollection
                {
                    valueToWrite
                };
                try
                {
                    session.Write(null, valuesToWrite, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos);
                    ClientBase.ValidateResponse(results, valuesToWrite);
                    ClientBase.ValidateDiagnosticInfos(diagnosticInfos, valuesToWrite);
                    opcUaDataItem.OpcUaStatusCodes = (OpcUaStatusCodes)results[0].Code;
                    opcUaDataItem.OldValue = opcUaDataItem.NewValue;
                    if (results[0].Code == 0)
                    {
                        opcUaDataItem.NewValue = newValue;
                    }
                    return (OpcUaStatusCodes)results[0].Code;
                }
                catch (ServiceResultException e)
                {
                    OnErrorHappened?.Invoke(this, new OpcUaErrorEventArgs((OpcUaStatusCodes)e.StatusCode, $"写入数据时错误,{opcUaDataItem.ToString()}", e));
                    return OpcUaStatusCodes.BadServerHalted;
                }
                catch (Exception ex)
                {
                    OnErrorHappened?.Invoke(this, new OpcUaErrorEventArgs(OpcUaStatusCodes.Uncertain, $"写入数据时发生未知错误,{opcUaDataItem.ToString()}", ex));
                    return OpcUaStatusCodes.Uncertain;
                }
            });
        }

        ///// <summary>
        ///// 批量写入
        ///// </summary>
        ///// <typeparam name="Tin"></typeparam>
        ///// <typeparam name="Tout"></typeparam>
        ///// <param name="writeParams"></param>
        ///// <returns></returns>
        //public async Task<OpcUaStatusCodes[]> Writes(IList<OpcUaDataItem> opcUaDataItems, IList<object> newValues)
        //{
        //    return await Task.Run(() =>
        //    {
        //        //var writeNodes = writeParams as DeviceInputParamEntityBase[];
        //        //OpcUaDeviceOutParamEntity[] opcUaDeviceOutParamEntitys = new OpcUaDeviceOutParamEntity[writeNodes.Count()];
        //        if (Equals(opcUaDataItems, null) || Equals(opcUaDataItems, null))
        //        {
        //            return new OpcUaStatusCodes[1];
        //        }

        //        if (opcUaDataItems.Count != newValues.Count)
        //        {
        //            OnLogHappened?.Invoke(this, new OpcUaLogEventArgs($"写入节点和值的数量不匹配{opcUaDataItems.Count}/{ newValues.Count}"));
        //        }
        //        WriteValueCollection valuesToWrite = new WriteValueCollection(opcUaDataItems.Count());
        //        foreach (var writeNode in opcUaDataItems)
        //        {
        //            WriteValue valueToWrite = new WriteValue();
        //            valueToWrite.NodeId = new NodeId(writeNode.Name);
        //            valueToWrite.AttributeId = Attributes.Value;
        //            valueToWrite.Value.Value = Convert.ChangeType(writeNode.NewValue, writeNode.ValueType ?? typeof(object));
        //            valueToWrite.Value.StatusCode = StatusCodes.Good;
        //            valueToWrite.Value.ServerTimestamp = DateTime.MinValue;
        //            valueToWrite.Value.SourceTimestamp = DateTime.MinValue;
        //            valuesToWrite.Add(valueToWrite);
        //        }
        //        try
        //        {
        //            session.Write(null, valuesToWrite, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos);
        //            ClientBase.ValidateResponse(results, valuesToWrite);
        //            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, valuesToWrite);
        //            for (int i = 0; i < results.Count; i++)
        //            {


        //                opcUaDataItems[i].OpcUaStatusCodes = (OpcUaStatusCodes)results[i].Code;
        //                //opcUaDataItem.Message = results[0].StatusCode.ToString();
        //                opcUaDataItems[i].OldValue = opcUaDataItems[i].NewValue;
        //                if (results[i].Code == 0)
        //                {
        //                    opcUaDataItems[i].NewValue = newValues[i];
        //                }

        //                //return (OpcUaStatusCodes)results[0].Code;








        //                //OpcUaDeviceOutParamEntity opcUaDeviceOutParamEntity = new OpcUaDeviceOutParamEntity();
        //                //opcUaDeviceOutParamEntity.NodeId = writeNodes[i].NodeId;
        //                //opcUaDeviceOutParamEntity.Value = writeNodes[i].Value;
        //                //opcUaDeviceOutParamEntity.ValueType = valuesToWrite[i].Value.WrappedValue.TypeInfo.BuiltInType.GetTypeCode().ToType();
        //                //opcUaDeviceOutParamEntity.StatusCode = results[i].Code;
        //                //opcUaDeviceOutParamEntity.Message = OpcUaStatusCodes.GetBrowseName(results[i].Code);
        //                //opcUaDeviceOutParamEntitys[i] = opcUaDeviceOutParamEntity;



        //            }
        //            //return null;

        //        }
        //        catch (ServiceResultException e)
        //        {
        //            OnErrorHappened?.Invoke(this, new OpcUaErrorEventArgs((OpcUaStatusCodes)e.StatusCode, $"批量读取数据时错误", e));
        //            return new OpcUaStatusCodes[1];

        //        }
        //        catch (Exception ex)
        //        {
        //            OnErrorHappened?.Invoke(this, new OpcUaErrorEventArgs(OpcUaStatusCodes.Uncertain, $"批量读取数据时发生未知错误", ex));
        //            return new OpcUaStatusCodes[1];

        //        }
        //    });
        //}

        #endregion

        #region 读取数据

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="opcUaDataItem"></param>
        /// <returns></returns>
        public async Task<OpcUaDataItem> Read(OpcUaDataItem opcUaDataItem)
        {
            return await Task.Run(() =>
            {
                ReadValueId nodeToRead = new ReadValueId()
                {
                    NodeId = new NodeId(opcUaDataItem.Name),
                    AttributeId = Attributes.Value
                };
                ReadValueIdCollection nodesToRead = new ReadValueIdCollection
                {
                    nodeToRead
                };
                try
                {
                    session.Read(null, 0, TimestampsToReturn.Neither, nodesToRead, out DataValueCollection results, out DiagnosticInfoCollection diagnosticInfos);
                    ClientBase.ValidateResponse(results, nodesToRead);
                    ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);
                    opcUaDataItem.OpcUaStatusCodes = (OpcUaStatusCodes)results[0].StatusCode.Code;
                    //opcUaDataItem.Message = results[0].StatusCode.ToString();
                    opcUaDataItem.OldValue = opcUaDataItem.NewValue;
                    opcUaDataItem.NewValue = results[0].Value;
                    //opcUaDataItem.NodeId = readNode.NodeId;
                    //opcUaDataItem.ValueType = results[0].WrappedValue.TypeInfo.BuiltInType.GetTypeCode().ToType();
                    return opcUaDataItem;

                }
                catch (ServiceResultException e)
                {
                    OnErrorHappened?.Invoke(this, new OpcUaErrorEventArgs((OpcUaStatusCodes)e.StatusCode, $"读取数据时错误,{opcUaDataItem.ToString()}", e));
                    return null;
                }
                catch (Exception ex)
                {
                    OnErrorHappened?.Invoke(this, new OpcUaErrorEventArgs(OpcUaStatusCodes.Uncertain, $"读取数据时发生未知错误,{opcUaDataItem.ToString()}", ex));
                    return null;
                }
            });
        }

        /// <summary>
        /// 批量读取
        /// </summary>
        /// <param name="opcUaDataItems"></param>
        /// <returns></returns>
        public async Task<IList<OpcUaDataItem>> Reads(IList<OpcUaDataItem> opcUaDataItems)
        {
            return await Task.Run(() =>
            {
                ReadValueIdCollection nodesToRead = new ReadValueIdCollection(opcUaDataItems.Count());
                foreach (var readNode in opcUaDataItems)
                {
                    ReadValueId nodeToRead = new ReadValueId()
                    {
                        NodeId = new NodeId(readNode.Name),
                        AttributeId = Attributes.Value
                    };
                    nodesToRead.Add(nodeToRead);
                }
                try
                {
                    session.Read(null, 0, TimestampsToReturn.Neither, nodesToRead, out DataValueCollection results, out DiagnosticInfoCollection diagnosticInfos);
                    ClientBase.ValidateResponse(results, nodesToRead);
                    ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);
                    for (int i = 0; i < results.Count; i++)
                    {
                        opcUaDataItems[i].OldValue = opcUaDataItems[i].NewValue;
                        opcUaDataItems[i].NewValue = results[i].Value;
                        opcUaDataItems[i].OpcUaStatusCodes = (OpcUaStatusCodes)results[i].StatusCode.Code;
                        //opcUaDataItems[i].ValueType = results[0].WrappedValue.TypeInfo.BuiltInType.GetTypeCode().ToType();
                    }
                    return opcUaDataItems;
                }
                catch (ServiceResultException e)
                {
                    OnErrorHappened?.Invoke(this, new OpcUaErrorEventArgs((OpcUaStatusCodes)e.StatusCode, $"批量读取数据时错误", e));
                    return null;
                }
                catch (Exception ex)
                {
                    OnErrorHappened?.Invoke(this, new OpcUaErrorEventArgs(OpcUaStatusCodes.Uncertain, $"批量读取数据时发生未知错误", ex));
                    return null;
                }

            });
        }

        #endregion

        /// <summary>
        /// 注册监控的数据点
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public async Task<OpcUaStatusCodes> RegisterNodes(List<OpcUaDataItem> nodes)
        {
            this.OpcUaDataItems = nodes;
            daemonTimer.Enabled = false;
            daemonTimer.Stop();
            //如果未连接那么返回
            if (!this.IsConnected)
            {
                OnErrorHappened?.Invoke(this, new OpcUaErrorEventArgs(OpcUaStatusCodes.Bad, $"{ToString()}，未连接OPCUA服务器，请先连接在订阅数据项,", null));
                return OpcUaStatusCodes.Bad;
            }
            return await Task<OpcUaStatusCodes>.Run(() =>
            {
                OpcUaStatusCodes opcUaStatusCodes = OpcUaStatusCodes.Bad;
                if (session == null)
                {
                    opcUaStatusCodes = OpcUaStatusCodes.BadSessionNotActivated;
                }
                try
                {
                    if (Equals(null, nodes) && nodes.Count() < 1)
                    {
                        //没有数据
                        opcUaStatusCodes = OpcUaStatusCodes.BadInvalidArgument;
                    }
                    // 根据更新频率分组
                    var OpcUaNodeGroups = from a in nodes
                                          group a by a.UpdateRate into g
                                          select new { UpdateRate = g.Key, OpcUaNodes = g };
                    foreach (var OpcUaNodeGroup in OpcUaNodeGroups)
                    {
                        var subscription = session.Subscriptions.FirstOrDefault(a => a.DisplayName == OpcUaNodeGroup.UpdateRate.ToString());
                        //未找到已经订阅的组，那么新建组并添加订阅项,并关联监视事件句柄
                        if (Equals(null, subscription))
                        {
                            #region 增加订阅
                            //创建订阅组。订阅组状态的名称是更新频率
                            //subscription = new Subscription();
                            subscription = new Subscription(session.DefaultSubscription);
                            bool isAddSession = session.AddSubscription(subscription);
                            subscription.Create();
                            subscription.PublishingInterval = OpcUaNodeGroup.UpdateRate;
                            subscription.PublishingEnabled = true;
                            subscription.LifetimeCount = 0;
                            subscription.KeepAliveCount = 0;
                            subscription.DisplayName = OpcUaNodeGroup.UpdateRate.ToString();
                            subscription.Priority = byte.MaxValue;
                            List<MonitoredItem> monitoredItems = new List<MonitoredItem>();

                            foreach (var v in OpcUaNodeGroup.OpcUaNodes)
                            {
                                monitoredItems.Add(new MonitoredItem()
                                {
                                    StartNodeId = new NodeId(v.Name),
                                });
                            }
                            //关联监视
                            monitoredItems.ForEach(a => a.Notification += OnMonitoredNotification);
                            //foreach (var monitoredItem in monitoredItems)
                            //{
                            //    monitoredItem.Notification += OnMonitoredNotification;
                            //}
                            subscription.AddItems(monitoredItems);
                            subscription.ApplyChanges();

                            if (isAddSession)
                            {
                                opcUaStatusCodes = OpcUaStatusCodes.BadSubscriptionIdInvalid;
                            }
                            else
                            {

                                opcUaStatusCodes = OpcUaStatusCodes.BadSubscriptionIdInvalid;
                            }
                            #endregion 增加订阅
                        }
                        else//已经有订阅组，那么更新订阅项,没有的订阅移除，原有的订阅保留，新增的订阅增加
                        {
                            //查询要删除的点
                            #region 更新订阅

                            //移除
                            //查询要删除的点
                            List<MonitoredItem> deleteItems = new List<MonitoredItem>();
                            foreach (var v in subscription.MonitoredItems)
                            {
                                if (!OpcUaNodeGroup.OpcUaNodes.Any(a => a.Name == v.StartNodeId))
                                {
                                    deleteItems.Add(v);
                                }
                            }
                            deleteItems.ForEach(a => a.Notification -= OnMonitoredNotification);
                            OnLogHappened?.BeginInvoke(this, new OpcUaLogEventArgs($"{ToString()}，要删除的点数：{deleteItems.Count } "), new AsyncCallback((ia) =>
                            {
                                if (ia.IsCompleted)
                                {
                                    OnLogHappened.EndInvoke(ia);
                                }
                            }), null);

                            if (!Equals(null, deleteItems) && deleteItems.Count() > 0)
                            {
                                subscription.RemoveItems(deleteItems);
                                OnLogHappened?.Invoke(this, new OpcUaLogEventArgs($"{ToString()}，已经删除的点数：{deleteItems.Count } "));
                                subscription.ApplyChanges();
                            }
                            //新增
                            //查询要新增加的点，
                            IList<OpcUaDataItem> addItems = new List<OpcUaDataItem>();
                            foreach (var v in OpcUaNodeGroup.OpcUaNodes)
                            {
                                if (!subscription.MonitoredItems.Any(a => a.StartNodeId.ToString() == v.Name))
                                {
                                    addItems.Add(v);
                                }
                            }
                            var addMonitoredItems = from a in addItems
                                                    select new MonitoredItem
                                                    {
                                                        StartNodeId = a.Name,
                                                        AttributeId = Attributes.Value,
                                                        DisplayName = a.Name,
                                                        SamplingInterval = OpcUaNodeGroup.UpdateRate,
                                                        MonitoringMode = MonitoringMode.Reporting,
                                                    };
                            //关联监视
                            //addMonitoredItems.ToList().ForEach(a => a.Notification += OnMonitoredNotification);
                            foreach (var monitoredItem in addMonitoredItems)
                            {
                                monitoredItem.Notification += OnMonitoredNotification;
                            }
                            //OnLogHappened?.Invoke(this, new OpcUaLogEventArgs($"{ToString()}，要增加的点数：{addMonitoredItems.Count() } "));
                            OnLogHappened?.BeginInvoke(this, new OpcUaLogEventArgs($"{ToString()}，要增加的点数：{addMonitoredItems.Count() } "), new AsyncCallback((ia) =>
                            {
                                if (ia.IsCompleted)
                                {
                                    OnLogHappened.EndInvoke(ia);
                                }
                            }), null);

                            subscription.AddItems(addMonitoredItems);
                            //OnLogHappened?.Invoke(this, new OpcUaLogEventArgs($"{ToString()}，已经增加的点数：{addMonitoredItems.Count() } "));
                            OnLogHappened?.BeginInvoke(this, new OpcUaLogEventArgs($"{ToString()}，已经增加的点数：{addMonitoredItems.Count() } "), new AsyncCallback((ia) =>
                            {
                                if (ia.IsCompleted)
                                {
                                    OnLogHappened.EndInvoke(ia);
                                }
                            }), null);
                            //subscription.Create();
                            subscription.ApplyChanges();
                            #endregion 增加订阅
                        }
                    }//endforeach

                    //删除订阅项后没有item了，那么删除订阅subscription
                    for (int i = session.Subscriptions.Count() - 1; i >= 0; i--)
                    {
                        var subscription = session.Subscriptions.ElementAt(i);
                        OnLogHappened?.Invoke(this, new OpcUaLogEventArgs($"{ToString()}，Subscription:{subscription.DisplayName}，MonitoredItemCount:{subscription.MonitoredItemCount}"));

                        if (subscription.MonitoredItemCount < 1)
                        {
                            subscription.Delete(false);
                            session.RemoveSubscription(subscription);
                            OnLogHappened?.Invoke(this, new OpcUaLogEventArgs($"{ToString()}，Subscription:{subscription.DisplayName}，MonitoredItemCount:{subscription.MonitoredItemCount}"));
                        }
                        //订阅组组减少时，需要删除组内的监视项，释放Notification，删除，订阅项
                        if (!OpcUaNodeGroups.Any(a => a.UpdateRate.ToString() == subscription.DisplayName))
                        {
                            if (subscription.MonitoredItemCount > 0)
                            {
                                foreach (var monitoredItem in subscription.MonitoredItems)
                                {
                                    monitoredItem.Notification -= OnMonitoredNotification;
                                }
                                OnLogHappened?.Invoke(this, new OpcUaLogEventArgs($"{ToString()}，删除的点数：{subscription.MonitoredItems.Count() }"));
                                subscription.RemoveItems(subscription.MonitoredItems);
                            }
                            subscription.Delete(false);
                            session.RemoveSubscription(subscription);
                        }
                    }

                    foreach (var subs in session.Subscriptions)
                    {
                        StringBuilder stringBuilder = new StringBuilder();

                        var badCount = 0;
                        foreach (var v in subs.MonitoredItems)
                        {
                            if (!Equals(v.Status.Error, null) && v.Status.Error.StatusCode.Code != StatusCodes.Good)
                            {
                                stringBuilder.Append("点名：");
                                stringBuilder.Append(v?.StartNodeId?.ToString());
                                stringBuilder.Append("，状态：");
                                stringBuilder.Append(v?.Status?.Error?.StatusCode.ToString());
                                stringBuilder.AppendLine();
                                badCount = badCount + 1;
                            }
                        }
                        int tolal = subs.MonitoredItems.Count();
                        OnLogHappened?.
                        Invoke(this, new OpcUaLogEventArgs($"{ToString()}，订阅组名：{subs.DisplayName}，节点质量统计(Good/NotGood/Total)：{tolal - badCount}/{badCount}/{tolal}{System.Environment.NewLine}{stringBuilder.ToString()}"));
                    }
                    opcUaStatusCodes = OpcUaStatusCodes.Good;
                }
                catch (ServiceResultException e)
                {
                    OnErrorHappened?.Invoke(this, new OpcUaErrorEventArgs((OpcUaStatusCodes)e.StatusCode, $"{ToString()}，订阅数据时错误", e));
                    opcUaStatusCodes = OpcUaStatusCodes.BadServerHalted;

                }
                catch (Exception ex)
                {
                    OnErrorHappened?.Invoke(this, new OpcUaErrorEventArgs(OpcUaStatusCodes.Uncertain, $"{ToString()}，订阅数据时发生未知错误", ex));
                    opcUaStatusCodes = OpcUaStatusCodes.Uncertain;
                }
                finally
                {
                    daemonTimer.Enabled = true;
                    daemonTimer.Start();
                }
                return opcUaStatusCodes;
            });
        }

        /// <summary>
        /// 重新注册数据点，有不良点时才重新注册
        /// </summary>
        private async Task reRegisterNodes()
        {
            if (IsConnected
               && !Equals(null, this.OpcUaDataItems)
                && OpcUaDataItems.Count() > 0
                && OpcUaDataItems.Count(a => a.OpcUaStatusCodes != OpcUaStatusCodes.Good) > 0)
            {
                //OnLogHappened?.Invoke(this, new OpcUaLogEventArgs($"{ToString()}，开始重新注册数据点"));
                OnLogHappened?.BeginInvoke(this, new OpcUaLogEventArgs($"{ToString()}，开始重新注册数据点"), new AsyncCallback((a) =>
                {
                    if (a.IsCompleted)
                    {
                        OnLogHappened?.EndInvoke(a);
                    }
                }), null);

                var registerResult = await RegisterNodes(OpcUaDataItems);
                OnLogHappened?.Invoke(this, new OpcUaLogEventArgs($"{ToString()}，重新注册数据点结果：{registerResult}"));
                if (registerResult == OpcUaStatusCodes.Good)
                {
                    //注册成功后后重新读取
                    await Reads(OpcUaDataItems.Where(a => a.OpcUaStatusCodes != OpcUaStatusCodes.Good).ToList());
                }
            }
        }

        /// <summary>
        /// 处理订阅事件
        /// </summary>
        /// <param name="item"></param>
        /// <param name="e"></param>
        private void OnMonitoredNotification(MonitoredItem item, MonitoredItemNotificationEventArgs e)
        {
            MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
            var node = e.NotificationValue as NodeId;
            var opcUaDataItem = this.OpcUaDataItems.FirstOrDefault(a => a.Name == item.ResolvedNodeId.ToString());
            if (!Equals(opcUaDataItem, null))
            {
                opcUaDataItem.OldValue = opcUaDataItem.NewValue;
                opcUaDataItem.NewValue = notification?.Value.Value;
                opcUaDataItem.OpcUaStatusCodes = (OpcUaStatusCodes)notification?.Value.StatusCode.Code;
                OpcUaDataEventArgs args = new OpcUaDataEventArgs(opcUaDataItem.OpcUaStatusCodes, opcUaDataItem);
                OnDataChanged?.BeginInvoke(this, args, new AsyncCallback((a) =>
                {
                    if (a.IsCompleted)
                    {
                        OnDataChanged?.EndInvoke(a);
                    }
                }), null);
            }
        }

        #region IDisposable
        private bool _disposed;

        /// <summary>
        /// 释放对象，用于外部调用
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放当前对象时释放资源
        /// </summary>
        ~OpcUaClientHelper()
        {
            Dispose(false);
        }

        /// <summary>
        /// 重写以实现释放对象的逻辑
        /// </summary>
        /// <param name="disposing">是否要释放对象</param>
        protected virtual async void Dispose(bool disposing)
        {

            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                await DisConnectAsync();
                this.session = null;
                this.applicationConfiguration = null;
                this.applicationInstance = null;
                this.ServerUri = null;
                this.sessionReconnectHandler = null;
            }
            _disposed = true;


        }

        #endregion

        public override string ToString()
        {
            return $"名称(Name):{this.Name},地址(ServerUri):{this.ServerUri} ";
        }
    }
}
