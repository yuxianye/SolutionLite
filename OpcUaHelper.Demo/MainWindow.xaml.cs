using OpcUaHelper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace OpcUaHelper.Demo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            intiOpcUaClientHelper();
            initOpcUaDataItems();
            upMessageDelgate = new UpMessageDelgate(upMessage);
            upOpcUaDataItemsDelgate = new UpOpcUaDataItemsDelgate(upOpcUaDataItems);
        }

        #region 变量定义

        private OpcUaHelper.OpcUaClientHelper opcUaClientHelper;

        private const int updateRateGroup1 = 1000;
        private const int updateRateGroup2 = 1000;
        private const int updateRateGroup3 = 1000;

        /// <summary>
        /// 更新消息代理
        /// </summary>
        /// <param name="a"></param>
        private delegate void UpMessageDelgate(string a);

        /// <summary>
        /// 更新消息代理
        /// </summary>
        private UpMessageDelgate upMessageDelgate;

        private ObservableCollection<OpcUaDataItem> dataGridDataSource = new ObservableCollection<OpcUaDataItem>();

        private delegate void UpOpcUaDataItemsDelgate(IList<OpcUaDataItem> opcUaDataItems);

        private UpOpcUaDataItemsDelgate upOpcUaDataItemsDelgate;

        //private delegate void UpOpcUaDataItemDelgate(OpcUaDataItem opcUaDataItem);
        //private UpOpcUaDataItemDelgate upOpcUaDataItemDelgate;

        private const string dateString = "yyyy-MM-dd HH:mm:ss ffff ";

        private ObservableCollection<OpcUaDataItem> opcUaDataItems { get; set; } = new ObservableCollection<OpcUaDataItem>();

        public UInt16 i = 0;
        public DateTime dateTime = DateTime.Now;
        public Type type;//= i.GetType();
        public Type type2;//= dateTime.GetType();

        #endregion


        #region 初始化数据和变量

        /// <summary>
        /// 初始化ua实例
        /// </summary>
        private void intiOpcUaClientHelper()
        {
            opcUaClientHelper = new OpcUaClientHelper();
            opcUaClientHelper.ServerUri = cboxOpcServers.Text;
            opcUaClientHelper.OnLogHappened += OpcUaClienthelper_OnLogHappened;
            opcUaClientHelper.OnErrorHappened += OpcUaClienthelper_OnErrorHappened;
            opcUaClientHelper.OnDataChanged += OpcUaClienthelper_OnDataChanged;
        }

        //FileStream fsLog = new FileStream($"{DateTime.Now.ToString("yyyyMMdd")}.log", FileMode.OpenOrCreate);
        //StreamWriter swLog;
        ///// <summary>
        ///// 写文件
        ///// </summary>
        ///// <param name="xContent">内容</param>
        ///// <param name="xFilePath">路径</param>
        //public void WriteFile(string xContent)
        //{
        //    //lock (lockObject)
        //    //{
        //    logStringQueue.Enqueue(xContent);

        //    //swLog.WriteAsync(xContent);

        //    //}

        //}

        //private void writeLogAsync()
        //{
        //    Task.Factory.StartNew(() =>
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        while (true)
        //        {
        //            try
        //            {

        //                while (logStringQueue.Any())
        //                {
        //                    logStringQueue.TryDequeue(out string log);
        //                    sb.Append(log);
        //                }

        //                swLog.WriteAsync(sb.ToString());
        //                sb.Clear();

        //            }
        //            catch (Exception ex)
        //            {
        //                //Logger.Error($"处理订阅信息异常！ + \n {ex.ToString()}");
        //            }
        //            //Task.Delay(10);

        //            System.Threading.Thread.Sleep(1000);
        //        }
        //    });
        //}
        //ConcurrentQueue<string> logStringQueue = new ConcurrentQueue<string>();


        //FileStream fs = new FileStream("log.log", FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
        //StreamWriter swData;

        /// <summary>
        /// 初始化数据点
        /// </summary>
        private void initOpcUaDataItems()
        {
            StringBuilder sb = new StringBuilder();

            //#if DEBUG
            //            sb.Append("ns=2;s=TestChannel.TestDevice.Agv_TaskStatus;1000;False;True;Unknow");
            //            sb.AppendLine();
            //            sb.Append("Channel_1.Device_1.Tag_1;1000;0;0;Unknow");
            //            sb.AppendLine();
            //            sb.Append("Channel_1.Device_1.Tag_2;1000;0;0;Unknow");
            //            sb.AppendLine();
            //            sb.Append("Channel_1.Device_1.Tag_3;1000;0;0;Unknow");
            //            sb.AppendLine();
            //            sb.Append("Channel_1.Device_1.Tag_4;1000;0;0;Unknow");
            //            sb.AppendLine();
            //            sb.Append("Channel_1.Device_1.Tag_5;1000;0;0;Unknow");
            //            sb.AppendLine();
            //            sb.Append("Channel1.Device1.Tag1;1000;0;0;Unknow");
            //            sb.AppendLine();
            //            sb.Append("Channel1.Device1.Tag2;1000;0;0;Unknow");
            //            sb.AppendLine();
            //            sb.Append("S7:[S7 connection_52]DB800,X0.1;1000;0;0;Unknow");
            //            sb.AppendLine();
            //            sb.Append("S7 [S7_connection_52]DB800,X0.2;1000;0;0;Unknow");
            //            sb.AppendLine();
            //            //txtOpcDataItems.Text = sb.ToString();
            //#else
            //            txtOpcDataItems.Text = System.IO.File.ReadAllText("数据点.txt", Encoding.Default);
            //#endif
            type = i.GetType();
            type2 = dateTime.GetType();

            //OpcUaDataItems.Add(new OpcUaDataItem($"{Opc.Ua.VariableIds.ServerStatusType_CurrentTime}", 1000, type2, i, i, OpcUaStatusCodes.Bad));
            //i=2259i=2258
            opcUaDataItems.Add(new OpcUaDataItem("ns=2;s=TestChannel.TestDevice.Agv_TaskStatus", 1000, typeof(UInt16), i, i, OpcUaStatusCodes.Bad));
            opcUaDataItems.Add(new OpcUaDataItem("ns=2;s=TestChannel.TestDevice.Agv_SettingSpeed", 1000, typeof(UInt16), i, i, OpcUaStatusCodes.Bad));
            opcUaDataItems.Add(new OpcUaDataItem("ns=2;s=数据类型示例.8 位设备.R 寄存器.Short1", 1000, typeof(UInt16), i, i, OpcUaStatusCodes.Bad));
            opcUaDataItems.Add(new OpcUaDataItem("ns=2;s=数据类型示例.8 位设备.R 寄存器.Short2", 1000, typeof(UInt16), i, i, OpcUaStatusCodes.Bad));

            //string path = @"C:\Users\PEPE\Desktop\证据接口结构.xml";
            //FileStream stream = new FileStream(path, FileMode.Open);
            //EvidenceFilesModel dep = (EvidenceFilesModel)XMLParser.Deserialize(typeof(EvidenceFilesModel), stream);
            //stream.Close();
            ////txtOpcDataItems.Text = System.IO.File.ReadAllText("数据点.txt", Encoding.Default);

            sb.Clear();
            sb = null;

            //WriteFile("数据点加载完成！");
        }

        #endregion

        #region OpcUaClienthelper 回调

        private void OpcUaClienthelper_OnLogHappened(object sender, OpcUaHelper.OpcUaLogEventArgs e)
        {
            string message = DateTime.Now.ToString(dateString) + e.Log + System.Environment.NewLine;
            try
            {
                asyncUpMessage(message);
            }
            catch (AggregateException ex)
            {
                asyncUpMessage(DateTime.Now.ToString(dateString) + ex.Message + System.Environment.NewLine);
            }
        }

        private void OpcUaClienthelper_OnErrorHappened(object sender, OpcUaHelper.OpcUaErrorEventArgs e)
        {



            string message = DateTime.Now.ToString(dateString) + e.Message + (e.Exception == null ? "" : e.Exception.StackTrace) + System.Environment.NewLine;
            try
            {
                asyncUpMessage(message);
            }
            catch (AggregateException ex)
            {
                //asyncUpMessage(DateTime.Now.ToString(dateString) + ex.Message + System.Environment.NewLine);
            }
        }


        private void OpcUaClienthelper_OnDataChanged(object sender, OpcUaHelper.OpcUaDataEventArgs e)
        {
            string message = DateTime.Now.ToString(dateString) + e.OpcUaStatusCodes + " " + (e.OpcUaDataItem == null ? " " : e.OpcUaDataItem.ToString()) + System.Environment.NewLine;


            try
            {
                //int.TryParse(e.OpcDataItem.OldValue.ToString(), out int old);
                //int.TryParse(e.OpcDataItem.NewValue.ToString(), out int newv);
                //if (old + 3 != newv)
                //{
                //    System.Diagnostics.Debug.Print($"值不连续{e.OpcDataItem.ToString()}");
                //}
                //else
                //{
                //    //System.Diagnostics.Debug.Print($"连续{e.OpcDataItem.ToString()}");
                //    WriteFile(message);

                //}

                asyncUpMessage(message);



                this.Dispatcher.Invoke(new Action(() =>
                {
                    var oldItem = opcUaDataItems.FirstOrDefault(a => a.Name == e.OpcUaDataItem.Name);

                    int index = opcUaDataItems.IndexOf(oldItem);
                    opcUaDataItems.Insert(index, e.OpcUaDataItem);
                    opcUaDataItems.Remove(oldItem);
                    gvOpcUaDataItems.ItemsSource = null;
                    gvOpcUaDataItems.ItemsSource = opcUaDataItems;
                }));






                //asyncUpOpcUaDataItems(this.opcUaClientHelper.OpcUaDataItems);

                //upOpcDataItemDelgate(e.OpcDataItem);
            }
            catch (AggregateException ex)
            {
                //asyncUpMessage(DateTime.Now.ToString(dateString) + ex.Message + System.Environment.NewLine);
            }
        }

        #endregion

        #region 更新界面消息
        /// <summary>
        /// 异步更新消息
        /// </summary>
        /// <param name="message"></param>
        private void asyncUpMessage(string message)
        {
            upMessageDelgate.BeginInvoke(message, new AsyncCallback((result) =>
            {
                upMessageDelgate.EndInvoke(result);

            }), message);
            //this.Dispatcher.BeginInvoke(upMessageDelgate, message);
            //this.Dispatcher.Invoke(new Action(() =>
            //{
            //    this.txtMessage.Text = txtMessage.Text.Insert(0, message);
            //    int index = this.txtMessage.Text.LastIndexOf('\n');
            //    if (this.txtMessage.Text.LastIndexOf('\n') > 20000)
            //    {
            //        this.txtMessage.Text = this.txtMessage.Text.Remove(index);
            //    }
            //}));
        }

        /// <summary>
        /// 更新消息
        /// </summary>
        /// <param name="message"></param>
        private void upMessage(string message)
        {
            //lock (lockObject)
            //{
            //sw.WriteAsync(message);
            //    //sw.Write(message);

            //}

            this.Dispatcher.Invoke(new Action(() =>
            {
                this.txtMessage.Text = txtMessage.Text.Insert(0, message);
                int index = this.txtMessage.Text.LastIndexOf('\n');
                if (this.txtMessage.Text.LastIndexOf('\n') > 20000)
                {
                    this.txtMessage.Text = this.txtMessage.Text.Remove(index);
                }
            }));
            //System.Console.Write(message);
            //System.Diagnostics.Debug.Print(message);
            //WriteFile( message);
            //System.IO.File.AppendAllText("log.log", message);

            //this.txtMessage.Text = txtMessage.Text.Insert(0, message);
            //int index = this.txtMessage.Text.LastIndexOf('\n');
            //if (this.txtMessage.Text.LastIndexOf('\n') > 20000)
            //{
            //    this.txtMessage.Text = this.txtMessage.Text.Remove(index);
            //}
        }

        #endregion

        #region 更新界面数据项

        private void asyncUpOpcUaDataItems(IEnumerable<OpcUaDataItem> opcDataItem)
        {
            this.Dispatcher.BeginInvoke(upOpcUaDataItemsDelgate, opcDataItem);
        }

        private void upOpcUaDataItems(IEnumerable<OpcUaDataItem> opcDataItem)
        {
            //this.txtb.Text = "(" + opcDataItem.Count(a => a.Quality == OpcResult.S_OK) + "/" + opcDataItem.Count() + ")";

            //System.IO.File.AppendAllText("log.log", message);

            //gvOpcDataItems.ItemsSource = null;
            //gvOpcDataItems.ItemsSource = opcDataItem;
            //this.txtb.Text = "(" + opcDataItem.Count(a => a.Quality == OpcResult.S_OK) + "/" + opcDataItem.Count() + ")";
            //this.Dispatcher.BeginInvoke );
            this.Dispatcher.Invoke(new Action(() =>
            {
                //dataGridDataSource
                //gvOpcUaDataItems.ItemsSource = null;
                //gvOpcUaDataItems.ItemsSource = dataGridDataSource;
                this.txtb.Text = "(" + opcDataItem.Count(a => a.OpcUaStatusCodes == OpcUaStatusCodes.Good) + "/" + opcDataItem.Count() + ")";
            }));
        }

        //OpcDataItem tm = new OpcDataItem("test", 1000, "0", "0", OpcResult.Unknow);
        private void upOpcDataItem(OpcUaDataItem opcDataItem)
        {
            //this.txtb.Text = "(" + opcDataItem.Count(a => a.Quality == OpcResult.S_OK) + "/" + opcDataItem.Count() + ")";

            //System.IO.File.AppendAllText("log.log", message);

            //gvOpcDataItems.ItemsSource = null;
            //gvOpcDataItems.ItemsSource = opcDataItem;
            //this.txtb.Text = "(" + opcDataItem.Count(a => a.Quality == OpcResult.S_OK) + "/" + opcDataItem.Count() + ")";
            //this.Dispatcher.BeginInvoke );
            this.Dispatcher.Invoke(new Action(() =>
            {
                //dataGridDataSource
                //var v =  dataGridDataSource.First(a => a.Name == opcDataItem.Name) ;
                //  v = opcDataItem;

                //dataGridDataSource.Add(opcDataItem);

                //dataGridDataSource.Remove(opcDataItem);
                //var v = dataGridDataSource.FirstOrDefault(a => a.Name == opcDataItem.Name);
                //v = opcDataItem;
                //gvOpcDataItems.ItemsSource = null;
                //gvOpcDataItems.ItemsSource = dataGridDataSource;
                this.txtb.Text = "(" + dataGridDataSource.Count(a => a.OpcUaStatusCodes == OpcUaStatusCodes.Good) + "/" + dataGridDataSource.Count() + ")";
            }));



        }

        #endregion

        #region 按钮事件

        /// <summary>
        /// 查询服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchOpcServer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //var servers1 = OpcUaHelper.OpcuaClientHelper.GetOpcServers();
                ////var servers2 = OpcUaHelper.OpcClientHelper.GetOpcServers("127.0.0.1");
                //if (!Equals(null, servers1) && servers1.Count() > 0)
                //{
                //    foreach (var v in servers1)
                //    {
                //        string message = DateTime.Now.ToString(dateString) + "可用的OPC服务器：" + v + System.Environment.NewLine;
                //        asyncUpMessage(message);
                //    }

                //    cboxOpcServers.ItemsSource = null;

                //    cboxOpcServers.ItemsSource = servers1;
                //    if (servers1.Count() > 0)
                //    {
                //        cboxOpcServers.SelectedIndex = 0;
                //    }
                //}
                //else
                //{
                //    asyncUpMessage(DateTime.Now.ToString(dateString) + "未找到可用的OPC服务器。" + System.Environment.NewLine);
                //}

            }
            catch (Exception ex)
            {
                //asyncUpMessage(DateTime.Now.ToString(dateString) + ex.Message + System.Environment.NewLine);
            }
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnConnectOpcServer_Click(object sender, RoutedEventArgs e)
        {
            //opcClienthelper.Connect(cboxOpcServers.SelectedItem == null ? null : cboxOpcServers.SelectedItem.ToString());
            //opcClienthelper.OnLogHappened += OpcClienthelper_OnLogHappened;
            //opcClienthelper.OnErrorHappened += OpcClienthelper_OnErrorHappened;
            //opcClienthelper.OnDataChanged += OpcClienthelper_OnDataChanged;

            //opcClienthelper.OnLogHappened += OpcClienthelper_OnLogHappened;
            //opcClienthelper.OnErrorHappened += OpcClienthelper_OnErrorHappened;
            //opcClienthelper.OnDataChanged += OpcClienthelper_OnDataChanged;

            opcUaClientHelper.ServerUri = cboxOpcServers.Text;
            var opcUaStatusCodes = await opcUaClientHelper.ConnectAsync();

            string msg = $"{ DateTime.Now.ToString(dateString)}服务器连接结果：{opcUaStatusCodes}{System.Environment.NewLine}";


            asyncUpMessage(msg);


        }

        /// <summary>
        /// 断开服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnDisConnectOpcServer_Click(object sender, RoutedEventArgs e)
        {
            //opcClienthelper.OnDataChanged -= OpcClienthelper_OnDataChanged;
            //opcClienthelper.OnErrorHappened -= OpcClienthelper_OnErrorHappened;
            //opcClienthelper.OnLogHappened -= OpcClienthelper_OnLogHappened;
            //opcClienthelper.DisConnect();
            //opcClienthelper.DisConnectAsync();
            //opcClienthelper.OnLogHappened += OpcClienthelper_OnLogHappened;
            //opcClienthelper.OnErrorHappened += OpcClienthelper_OnErrorHappened;
            //opcClienthelper.OnDataChanged += OpcClienthelper_OnDataChanged;

            var opcUaStatusCodes = await opcUaClientHelper.DisConnectAsync();
            string msg = $"{ DateTime.Now.ToString(dateString)}服务器断开结果：{opcUaStatusCodes}{System.Environment.NewLine}";
            asyncUpMessage(msg);

        }

        /// <summary>
        /// 订阅数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddDataItems_Click(object sender, RoutedEventArgs e)
        {

            //opcClienthelper.RegisterOpcDataItemsAsync(new List<OpcHelper.OpcDataItem> {
            //    new OpcHelper.OpcDataItem ("Channel_1.Device_1.Tag_1",updateRateGroup3,"","", OpcHelper.OpcResult.Unknow),
            //    new OpcHelper.OpcDataItem ("Channel_1.Device_1.Bool_1",updateRateGroup1,"","",OpcHelper.OpcResult.Unknow),
            //});
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDispose_Click(object sender, RoutedEventArgs e)
        {
            opcUaClientHelper.Dispose();
        }

        /// <summary>
        /// 增加订阅数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>;
        private void btnReAddDataItems_Click(object sender, RoutedEventArgs e)
        {
            //opcUaClientHelper.RegisterOpcDataItemsAsync(new List<OpcHelper.OpcDataItem> {
            //    new OpcHelper.OpcDataItem ("Channel_1.Device_1.Tag_1",updateRateGroup1,"","", OpcHelper.OpcResult.Unknow),
            //    new OpcHelper.OpcDataItem ("Channel_1.Device_1.Tag_2",updateRateGroup2,"","", OpcHelper.OpcResult.Unknow),
            //    new OpcHelper.OpcDataItem ("Channel_1.Device_1.Bool_1",updateRateGroup2,"","",OpcHelper.OpcResult.Unknow),
            //});
        }

        /// <summary>
        /// 减少订阅数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>;
        private void btnDeleteDataItems_Click(object sender, RoutedEventArgs e)
        {
            //opcClienthelper.RegisterOpcDataItemsAsync(new List<OpcHelper.OpcDataItem> {
            //    new OpcHelper.OpcDataItem ("Channel_1.Device_1.Tag_2",updateRateGroup1,"","", OpcHelper.OpcResult.Unknow),
            //    new OpcHelper.OpcDataItem ("Channel_1.Device_1.Bool_1",updateRateGroup1,"","",OpcHelper.OpcResult.Unknow),
            //});
        }

        /// <summary>
        /// 取消所有订阅数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>;
        private void btnbtnNoDataItems_Click(object sender, RoutedEventArgs e)
        {
            //opcClienthelper.RegisterOpcDataItemsAsync(new List<OpcHelper.OpcDataItem>
            //{
            //    //new OpcUaHelper.OpcDataItem ("Channel_1.Device_1.Tag_2",100,"","", OpcUaHelper.OpcResult.Unknow),
            //    //new OpcUaHelper.OpcDataItem ("Channel_1.Device_1.Bool_1",200,"","",OpcUaHelper.OpcResult.Unknow),
            //});
        }

        /// <summary>
        /// 增加无效订阅数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>;
        private void btnAddInvalidDataItems_Click(object sender, RoutedEventArgs e)
        {
            //opcClienthelper.RegisterOpcDataItemsAsync(new List<OpcHelper.OpcDataItem> {
            //    new OpcHelper.OpcDataItem ("Channel_1.Device_1.Tag_20",updateRateGroup1,"","", OpcHelper.OpcResult.Unknow),
            //    new OpcHelper.OpcDataItem ("Channel_1.Device_1.Bool_1",updateRateGroup1,"","",OpcHelper.OpcResult.Unknow),

            //});
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>;
        private async void btnWriteDataItem_Click(object sender, RoutedEventArgs e)
        {
            string message = null;
            if (!opcUaClientHelper.IsConnected)
            {
                message = DateTime.Now.ToString(dateString) + "请先连接服务器" + System.Environment.NewLine;
                asyncUpMessage(message);
                return;
            }
            OpcUaDataItem opcUaDataItem = null;
            OpcUaStatusCodes opcUaStatusCodes;
            if (Equals(null, opcUaClientHelper.OpcUaDataItems) || opcUaClientHelper.OpcUaDataItems.Count < 1)
            {
                message = DateTime.Now.ToString(dateString) + "没有数据点" + System.Environment.NewLine;
            }
            else
            {
                opcUaDataItem = opcUaClientHelper.OpcUaDataItems.FirstOrDefault().Clone() as OpcUaDataItem;
                //bool newValue = (DateTime.Now.Millisecond % 2) == 0 ? true : false;
                //bool newValue = !tmpValue;
                //tmpValue = newValue;
                //System.Diagnostics.Debug.Print(tmpValue.ToString());
                opcUaStatusCodes = await opcUaClientHelper.Write(opcUaDataItem, 11);
                message = DateTime.Now.ToString(dateString) + "写入完成 " + opcUaStatusCodes + " " + (opcUaDataItem == null ? " " : opcUaDataItem.ToString()) + System.Environment.NewLine;

            }
            asyncUpMessage(message);
        }

        /// <summary>
        /// 读取实时数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnReadDataItem_Click(object sender, RoutedEventArgs e)
        {
            string message = null;
            if (!opcUaClientHelper.IsConnected)
            {
                message = DateTime.Now.ToString(dateString) + "请先连接服务器" + System.Environment.NewLine;
                asyncUpMessage(message);
                return;
            }
            OpcUaDataItem opcUaDataItem;
            if (Equals(null, opcUaClientHelper.OpcUaDataItems) || opcUaClientHelper.OpcUaDataItems.Count < 1)
            {
                message = DateTime.Now.ToString(dateString) + "没有数据点" + System.Environment.NewLine;
            }
            else
            {
                //正常读取
                opcUaDataItem = opcUaClientHelper.OpcUaDataItems.FirstOrDefault().Clone() as OpcUaDataItem;
                opcUaDataItem.Name = opcUaDataItem.Name;
                opcUaDataItem = await opcUaClientHelper.Read(opcUaDataItem);
                message = DateTime.Now.ToString("HH:mm:ss ffff ") + "单个读完成 " + (opcUaDataItem == null ? " " : opcUaDataItem.ToString()) + System.Environment.NewLine;
                asyncUpMessage(message);
                var results = await opcUaClientHelper.Reads(opcUaClientHelper.OpcUaDataItems);
                foreach (var v in results)
                {
                    message = DateTime.Now.ToString("HH:mm:ss ffff ") + "批量读完成 " + (v == null ? " " : v.ToString()) + System.Environment.NewLine;
                    asyncUpMessage(message);
                }
            }



            //if (!Equals(null, opcUaClientHelper.OpcUaDataItems) && opcUaClientHelper.OpcUaDataItems.Count > 0)
            //{
            //    //无效读取
            //    var opcUaDataItem2 = opcUaClientHelper.OpcUaDataItems.LastOrDefault().Clone() as OpcUaDataItem;
            //    opcUaDataItem2.Name = opcUaDataItem2.Name + "xxx";
            //    opcUaDataItem2 = await opcUaClientHelper.Read(opcUaDataItem2);
            //    message = DateTime.Now.ToString(dateString) + "读完成 " + (opcUaDataItem2 == null ? " " : opcUaDataItem2.ToString()) + System.Environment.NewLine;
            //    asyncUpMessage(message);
            //}
        }

        /// <summary>
        /// 读取缓存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadCacheDataItems_Click(object sender, RoutedEventArgs e)
        {
            string message;
            if (!opcUaClientHelper.IsConnected)
            {
                message = DateTime.Now.ToString(dateString) + "请先连接服务器" + System.Environment.NewLine;
                asyncUpMessage(message);
                return;
            }
            if (Equals(null, opcUaClientHelper.OpcUaDataItems) || opcUaClientHelper.OpcUaDataItems.Count < 1)
            {
                message = DateTime.Now.ToString(dateString) + "没有数据点" + System.Environment.NewLine;
            }
            else
            {
                var opcDataItem = opcUaClientHelper.OpcUaDataItems.FirstOrDefault().Clone() as OpcUaDataItem;
                message = DateTime.Now.ToString(dateString) + "读完成 " + (opcDataItem == null ? " " : opcDataItem.ToString()) + System.Environment.NewLine;
            }
            asyncUpMessage(message);
        }

        private async void btnUpdateDataItems_Click(object sender, RoutedEventArgs e)
        {
            string message;
            if (!opcUaClientHelper.IsConnected)
            {
                message = DateTime.Now.ToString(dateString) + "请先连接服务器" + System.Environment.NewLine;
                asyncUpMessage(message);
                return;
            }
            //var strList = txtOpcDataItems.Text.Split('\r', '\n');

            //List<OpcUaDataItem> opcUaDataItems = new List<OpcUaDataItem>(strList.Count());
            ////txtOpcDataItems .Text.Split (System.Environment.NewLine ):
            //foreach (var strOpcDataItem in strList)
            //{
            //    var strOpcDataItemTmp = strOpcDataItem.Split(';');
            //    if (strOpcDataItemTmp.Count() < 2)
            //    {
            //        continue;
            //    }
            //    OpcDataItem opcDataItem =
            //        new OpcDataItem(strOpcDataItemTmp[0], int.Parse(strOpcDataItemTmp[1]), strOpcDataItemTmp[2], strOpcDataItemTmp[3], (OpcResult)Enum.Parse(typeof(OpcResult), strOpcDataItemTmp[4]));
            //    opcDataItems.Add(opcDataItem);
            //}
            //opcClienthelper.RegisterOpcDataItemsAsync(opcDataItems);
            //dataGridDataSource = new ObservableCollection<OpcDataItem>(opcDataItems);
            //gvOpcUaDataItems.ItemsSource = dataGridDataSource;
            //this.txtb.Text = "(" + dataGridDataSource.Count(a => a.Quality == OpcResult.S_OK) + "/" + dataGridDataSource.Count() + ")";
            var opcUaStatusCodes = await opcUaClientHelper.RegisterNodes(opcUaDataItems.ToList());
            string msg = $"{ DateTime.Now.ToString(dateString)}订阅数据结果：{opcUaStatusCodes}{System.Environment.NewLine}";
            asyncUpMessage(msg);
            gvOpcUaDataItems.ItemsSource = opcUaDataItems;
        }

        private void btnCreateData_Click(object sender, RoutedEventArgs e)
        {

            if (System.IO.File.Exists("数据点.txt"))
            {
                System.IO.File.Copy("数据点.txt", $"数据点{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt");
            }

            int lineCount = 2000;
            int.TryParse(txtCreateDataCount.Text, out lineCount);
            lineCount = lineCount + 1;
            List<string> lines = new List<string>(lineCount);
            for (int i = 1; i < lineCount; i++)
            {
                lines.Add($"通道 1.设备 1.标记 {i.ToString().PadLeft(5, '0')};100; False; True; Unknow");
            }
            System.IO.File.WriteAllLines("数据点.txt", lines, Encoding.Default);
            //sb.Append("Channel_1.Device_1.Bool_1;1000;False;True;Unknow");
            //sb.AppendLine();
            //sb.Append("Channel_1.Device_1.Tag_1;1000;0;0;Unknow");
            //sb.AppendLine();
            //sb.Append("Channel_1.Device_1.Tag_2;1000;0;0;Unknow");
            //sb.AppendLine();
            //sb.Append("Channel_1.Device_1.Tag_3;1000;0;0;Unknow");
            //sb.AppendLine();
            //sb.Append("Channel_1.Device_1.Tag_4;1000;0;0;Unknow");
            //sb.AppendLine();
            //sb.Append("Channel_1.Device_1.Tag_5;1000;0;0;Unknow");
            //sb.AppendLine();
            //sb.Append("Channel1.Device1.Tag1;1000;0;0;Unknow");
            //sb.AppendLine();
            //sb.Append("Channel1.Device1.Tag2;1000;0;0;Unknow");
            //sb.AppendLine();
            //sb.Append("S7:[S7 connection_52]DB800,X0.1;1000;0;0;Unknow");
            //sb.AppendLine();
            //sb.Append("S7 [S7_connection_52]DB800,X0.2;1000;0;0;Unknow");
            //sb.AppendLine();
            //txtOpcDataItems.Text = System.IO.File.ReadAllText("数据点.txt");
        }

        #endregion


        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>;
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (opcUaClientHelper.IsConnected)
                {
                    if (MessageBox.Show("正在通讯中，确定要退出么？\r\r退出后所有通讯将关闭！", "OPC测试助手", MessageBoxButton.OKCancel, MessageBoxImage.Question)
                          == MessageBoxResult.OK)
                    {
                        //e.Cancel = true;
                        opcUaClientHelper.Dispose();
                        //sw.Close();
                        //fs.Close();
                        //fs.Flush();
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }


                //swLog.Flush();
                //swLog.Close();
                //fsLog.Close();

            }
            catch
            {

            }

            //opcClienthelper.OnDataChanged -= OpcClienthelper_OnDataChanged;
            //opcClienthelper.OnLogHappened -= OpcClienthelper_OnLogHappened;
            //opcClienthelper.OnDataChanged -= OpcClienthelper_OnDataChanged;
            //opcClienthelper.DisConnect();
            //opcClienthelper.Dispose();
        }

    }
}
