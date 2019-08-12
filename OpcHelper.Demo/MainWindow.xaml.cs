using OpcHelper;
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

namespace OpcHelper.Demo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //sw = new StreamWriter(fs, Encoding.Default);

            swLog = new StreamWriter(fsLog, Encoding.Default);
            swLog.AutoFlush = true;

            System.Threading.Tasks.Task.Run(new Action(intiOpcClientHelper));

            InitOpcDataitems();
            upMessageDelgate = new UpMessageDelgate(upMessage);
            upOpcDataItemsDelgate = new UpOpcDataItemsDelgate(upOpcDataItems);
            upOpcDataItemDelgate = new UpOpcDataItemDelgate(upOpcDataItem);


            System.Threading.Tasks.Task.Run(new Action(writeLogAsync));


        }

        FileStream fsLog = new FileStream($"{DateTime.Now.ToString("yyyyMMdd")}.log", FileMode.OpenOrCreate);
        StreamWriter swLog;
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="xContent">内容</param>
        /// <param name="xFilePath">路径</param>
        public void WriteFile(string xContent)
        {
            //lock (lockObject)
            //{
            logStringQueue.Enqueue(xContent);

            //swLog.WriteAsync(xContent);

            //}

        }

        private void writeLogAsync()
        {
            Task.Factory.StartNew(() =>
            {
                StringBuilder sb = new StringBuilder();
                while (true)
                {
                    try
                    {

                        while (logStringQueue.Any())
                        {
                            logStringQueue.TryDequeue(out string log);
                            sb.Append(log);
                        }

                        swLog.WriteAsync(sb.ToString());
                        sb.Clear();

                    }
                    catch (Exception ex)
                    {
                        //Logger.Error($"处理订阅信息异常！ + \n {ex.ToString()}");
                    }
                    //Task.Delay(10);

                    System.Threading.Thread.Sleep(1000);
                }
            });
        }
        ConcurrentQueue<string> logStringQueue = new ConcurrentQueue<string>();


        //FileStream fs = new FileStream("log.log", FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
        //StreamWriter swData;
        private delegate void UpMessageDelgate(string a);
        private UpMessageDelgate upMessageDelgate;

        private delegate void UpOpcDataItemsDelgate(IList<OpcDataItem> opcDataItems);
        private UpOpcDataItemsDelgate upOpcDataItemsDelgate;

        private delegate void UpOpcDataItemDelgate(OpcDataItem opcDataItem);
        private UpOpcDataItemDelgate upOpcDataItemDelgate;

        private const string dateString = "yyyy-MM-dd HH:mm:ss ffff ";

        private void InitOpcDataitems()
        {
            StringBuilder sb = new StringBuilder();

#if DEBUG
            sb.Append("Channel_1.Device_1.Bool_1;1000;False;True;Unknow");
            sb.AppendLine();
            sb.Append("Channel_1.Device_1.Tag_1;1000;0;0;Unknow");
            sb.AppendLine();
            sb.Append("Channel_1.Device_1.Tag_2;1000;0;0;Unknow");
            sb.AppendLine();
            sb.Append("Channel_1.Device_1.Tag_3;1000;0;0;Unknow");
            sb.AppendLine();
            sb.Append("Channel_1.Device_1.Tag_4;1000;0;0;Unknow");
            sb.AppendLine();
            sb.Append("Channel_1.Device_1.Tag_5;1000;0;0;Unknow");
            sb.AppendLine();
            sb.Append("Channel1.Device1.Tag1;1000;0;0;Unknow");
            sb.AppendLine();
            sb.Append("Channel1.Device1.Tag2;1000;0;0;Unknow");
            sb.AppendLine();
            sb.Append("S7:[S7 connection_52]DB800,X0.1;1000;0;0;Unknow");
            sb.AppendLine();
            sb.Append("S7 [S7_connection_52]DB800,X0.2;1000;0;0;Unknow");
            sb.AppendLine();
            txtOpcDataItems.Text = sb.ToString();
#else 
            txtOpcDataItems.Text = System.IO.File.ReadAllText("数据点.txt", Encoding.Default);
#endif
            txtOpcDataItems.Text = System.IO.File.ReadAllText("数据点.txt", Encoding.Default);

            sb.Clear();
            sb = null;

            WriteFile("数据点加载完成！");
        }

        private OpcHelper.OpcClientHelper opcClienthelper;

        private const int updateRateGroup1 = 1000;
        private const int updateRateGroup2 = 1000;
        private const int updateRateGroup3 = 1000;

        private void intiOpcClientHelper()
        {
            opcClienthelper = new OpcClientHelper();
            opcClienthelper.OnLogHappened += OpcClienthelper_OnLogHappened;
            opcClienthelper.OnErrorHappened += OpcClienthelper_OnErrorHappened;
            opcClienthelper.OnDataChanged += OpcClienthelper_OnDataChanged;
            //var servers1 = OpcHelper.OpcClientHelper.GetOpcServers();
            //var servers2 = OpcHelper.OpcClientHelper.GetOpcServers("127.0.0.1");
            //clienthelper.Connect(servers1.First());
        }

        private void OpcClienthelper_OnLogHappened(object sender, OpcHelper.OpcLogEventArgs e)
        {
            string message = DateTime.Now.ToString(dateString) + e.Log + System.Environment.NewLine;
            try
            {
                asyncUpMessage(message);
            }
            catch (AggregateException ex)
            {
                //asyncUpMessage(DateTime.Now.ToString(dateString) + ex.Message + System.Environment.NewLine);
            }
        }

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

        private void asyncUpOpcDataItems(IEnumerable<OpcDataItem> opcDataItem)
        {
            this.Dispatcher.BeginInvoke(upOpcDataItemsDelgate, opcDataItem);
        }

        ObservableCollection<OpcDataItem> dataGridDataSource = new ObservableCollection<OpcDataItem>();
        private void upOpcDataItems(IEnumerable<OpcDataItem> opcDataItem)
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
                gvOpcDataItems.ItemsSource = null;
                gvOpcDataItems.ItemsSource = dataGridDataSource;
                this.txtb.Text = "(" + opcDataItem.Count(a => a.Quality == OpcResult.S_OK) + "/" + opcDataItem.Count() + ")";
            }));
        }
        OpcDataItem tm = new OpcDataItem("test", 1000, "0", "0", OpcResult.Unknow);
        private void upOpcDataItem(OpcDataItem opcDataItem)
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
                this.txtb.Text = "(" + dataGridDataSource.Count(a => a.Quality == OpcResult.S_OK) + "/" + dataGridDataSource.Count() + ")";
            }));



        }

        private void OpcClienthelper_OnErrorHappened(object sender, OpcHelper.OpcErrorEventArgs e)
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

        private void OpcClienthelper_OnDataChanged(object sender, OpcHelper.OpcDataEventArgs e)
        {
            string message = DateTime.Now.ToString(dateString) + e.OpcResult + " " + (e.OpcDataItem == null ? " " : e.OpcDataItem.ToString()) + System.Environment.NewLine;


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

                asyncUpOpcDataItems(this.opcClienthelper.OpcDataItems);

                //upOpcDataItemDelgate(e.OpcDataItem);
            }
            catch (AggregateException ex)
            {
                //asyncUpMessage(DateTime.Now.ToString(dateString) + ex.Message + System.Environment.NewLine);
            }
        }



        /// <summary>
        /// 查询服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchOpcServer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var servers1 = OpcHelper.OpcClientHelper.GetOpcServers();
                //var servers2 = OpcHelper.OpcClientHelper.GetOpcServers("127.0.0.1");
                if (!Equals(null, servers1) && servers1.Count() > 0)
                {
                    foreach (var v in servers1)
                    {
                        string message = DateTime.Now.ToString(dateString) + "可用的OPC服务器：" + v + System.Environment.NewLine;
                        asyncUpMessage(message);
                    }

                    cboxOpcServers.ItemsSource = null;

                    cboxOpcServers.ItemsSource = servers1;
                    if (servers1.Count() > 0)
                    {
                        cboxOpcServers.SelectedIndex = 0;
                    }
                }
                else
                {
                    asyncUpMessage(DateTime.Now.ToString(dateString) + "未找到可用的OPC服务器。" + System.Environment.NewLine);
                }

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
        private void btnConnectOpcServer_Click(object sender, RoutedEventArgs e)
        {
            opcClienthelper.Connect(cboxOpcServers.SelectedItem == null ? null : cboxOpcServers.SelectedItem.ToString());
            //opcClienthelper.OnLogHappened += OpcClienthelper_OnLogHappened;
            //opcClienthelper.OnErrorHappened += OpcClienthelper_OnErrorHappened;
            //opcClienthelper.OnDataChanged += OpcClienthelper_OnDataChanged;

            //opcClienthelper.OnLogHappened += OpcClienthelper_OnLogHappened;
            //opcClienthelper.OnErrorHappened += OpcClienthelper_OnErrorHappened;
            //opcClienthelper.OnDataChanged += OpcClienthelper_OnDataChanged;
        }

        /// <summary>
        /// 断开服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisConnectOpcServer_Click(object sender, RoutedEventArgs e)
        {
            //opcClienthelper.OnDataChanged -= OpcClienthelper_OnDataChanged;
            //opcClienthelper.OnErrorHappened -= OpcClienthelper_OnErrorHappened;
            //opcClienthelper.OnLogHappened -= OpcClienthelper_OnLogHappened;
            //opcClienthelper.DisConnect();
            opcClienthelper.DisConnectAsync();
            //opcClienthelper.OnLogHappened += OpcClienthelper_OnLogHappened;
            //opcClienthelper.OnErrorHappened += OpcClienthelper_OnErrorHappened;
            //opcClienthelper.OnDataChanged += OpcClienthelper_OnDataChanged;
        }

        /// <summary>
        /// 订阅数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddDataItems_Click(object sender, RoutedEventArgs e)
        {

            opcClienthelper.RegisterOpcDataItemsAsync(new List<OpcHelper.OpcDataItem> {
                new OpcHelper.OpcDataItem ("Channel_1.Device_1.Tag_1",updateRateGroup3,"","", OpcHelper.OpcResult.Unknow),
                new OpcHelper.OpcDataItem ("Channel_1.Device_1.Bool_1",updateRateGroup1,"","",OpcHelper.OpcResult.Unknow),
            });
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDispose_Click(object sender, RoutedEventArgs e)
        {
            opcClienthelper.Dispose();
        }

        /// <summary>
        /// 增加订阅数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>;
        private void btnReAddDataItems_Click(object sender, RoutedEventArgs e)
        {
            opcClienthelper.RegisterOpcDataItemsAsync(new List<OpcHelper.OpcDataItem> {
                new OpcHelper.OpcDataItem ("Channel_1.Device_1.Tag_1",updateRateGroup1,"","", OpcHelper.OpcResult.Unknow),
                new OpcHelper.OpcDataItem ("Channel_1.Device_1.Tag_2",updateRateGroup2,"","", OpcHelper.OpcResult.Unknow),
                new OpcHelper.OpcDataItem ("Channel_1.Device_1.Bool_1",updateRateGroup2,"","",OpcHelper.OpcResult.Unknow),
            });
        }

        /// <summary>
        /// 减少订阅数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>;
        private void btnDeleteDataItems_Click(object sender, RoutedEventArgs e)
        {
            opcClienthelper.RegisterOpcDataItemsAsync(new List<OpcHelper.OpcDataItem> {
                new OpcHelper.OpcDataItem ("Channel_1.Device_1.Tag_2",updateRateGroup1,"","", OpcHelper.OpcResult.Unknow),
                new OpcHelper.OpcDataItem ("Channel_1.Device_1.Bool_1",updateRateGroup1,"","",OpcHelper.OpcResult.Unknow),
            });
        }

        /// <summary>
        /// 取消所有订阅数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>;
        private void btnbtnNoDataItems_Click(object sender, RoutedEventArgs e)
        {
            opcClienthelper.RegisterOpcDataItemsAsync(new List<OpcHelper.OpcDataItem>
            {
                //new OpcHelper.OpcDataItem ("Channel_1.Device_1.Tag_2",100,"","", OpcHelper.OpcResult.Unknow),
                //new OpcHelper.OpcDataItem ("Channel_1.Device_1.Bool_1",200,"","",OpcHelper.OpcResult.Unknow),
            });
        }

        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>;
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (opcClienthelper.IsConnected)
                {
                    if (MessageBox.Show("正在通讯中，确定要退出么？\r\r退出后所有通讯将关闭！", "OPC测试助手", MessageBoxButton.OKCancel, MessageBoxImage.Question)
                          == MessageBoxResult.OK)
                    {
                        //e.Cancel = true;
                        opcClienthelper.Dispose();
                        //sw.Close();
                        //fs.Close();
                        //fs.Flush();
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }


                swLog.Flush();
                swLog.Close();
                fsLog.Close();

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

        /// <summary>
        /// 增加无效订阅数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>;
        private void btnAddInvalidDataItems_Click(object sender, RoutedEventArgs e)
        {
            opcClienthelper.RegisterOpcDataItemsAsync(new List<OpcHelper.OpcDataItem> {
                new OpcHelper.OpcDataItem ("Channel_1.Device_1.Tag_20",updateRateGroup1,"","", OpcHelper.OpcResult.Unknow),
                new OpcHelper.OpcDataItem ("Channel_1.Device_1.Bool_1",updateRateGroup1,"","",OpcHelper.OpcResult.Unknow),

            });
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>;
        private void btnWriteDataItem_Click(object sender, RoutedEventArgs e)
        {
            string message = null;
            if (!opcClienthelper.IsConnected)
            {
                message = DateTime.Now.ToString(dateString) + "请先连接服务器" + System.Environment.NewLine;
                asyncUpMessage(message);
                return;
            }
            OpcDataItem opcDataItem = null;
            OpcResult opcResult = OpcResult.Unknow;
            if (Equals(null, opcClienthelper.OpcDataItems) || opcClienthelper.OpcDataItems.Count < 1)
            {
                message = DateTime.Now.ToString(dateString) + "没有数据点" + System.Environment.NewLine;
            }
            else
            {
                opcDataItem = opcClienthelper.OpcDataItems.FirstOrDefault().Clone() as OpcDataItem;
                //bool newValue = (DateTime.Now.Millisecond % 2) == 0 ? true : false;
                //bool newValue = !tmpValue;
                //tmpValue = newValue;
                //System.Diagnostics.Debug.Print(tmpValue.ToString());
                opcResult = opcClienthelper.Write(opcDataItem, DateTime.Now.Millisecond);
                message = DateTime.Now.ToString(dateString) + "写入完成 " + opcResult + " " + (opcDataItem == null ? " " : opcDataItem.ToString()) + System.Environment.NewLine;

            }
            asyncUpMessage(message);
        }

        /// <summary>
        /// 读取实时数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadDataItem_Click(object sender, RoutedEventArgs e)
        {
            string message = null;
            if (!opcClienthelper.IsConnected)
            {
                message = DateTime.Now.ToString(dateString) + "请先连接服务器" + System.Environment.NewLine;
                asyncUpMessage(message);
                return;
            }
            OpcDataItem opcDataItem;
            if (Equals(null, opcClienthelper.OpcDataItems) || opcClienthelper.OpcDataItems.Count < 1)
            {
                message = DateTime.Now.ToString(dateString) + "没有数据点" + System.Environment.NewLine;
            }
            else
            {
                //正常读取
                opcDataItem = opcClienthelper.OpcDataItems.FirstOrDefault().Clone() as OpcDataItem;
                opcDataItem.Name = opcDataItem.Name;
                opcDataItem = opcClienthelper.Read(opcDataItem);
                message = DateTime.Now.ToString("HH:mm:ss ffff ") + "读完成 " + (opcDataItem == null ? " " : opcDataItem.ToString()) + System.Environment.NewLine;
            }
            asyncUpMessage(message);
            if (!Equals(null, opcClienthelper.OpcDataItems) && opcClienthelper.OpcDataItems.Count > 0)
            {
                //无效读取
                var opcDataItem2 = opcClienthelper.OpcDataItems.LastOrDefault().Clone() as OpcDataItem;
                opcDataItem2.Name = opcDataItem2.Name + "xxx";
                opcDataItem2 = opcClienthelper.Read(opcDataItem2);
                message = DateTime.Now.ToString(dateString) + "读完成 " + (opcDataItem2 == null ? " " : opcDataItem2.ToString()) + System.Environment.NewLine;
                asyncUpMessage(message);
            }
        }

        /// <summary>
        /// 读取缓存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadCacheDataItems_Click(object sender, RoutedEventArgs e)
        {
            string message;
            if (!opcClienthelper.IsConnected)
            {
                message = DateTime.Now.ToString(dateString) + "请先连接服务器" + System.Environment.NewLine;
                asyncUpMessage(message);
                return;
            }
            if (Equals(null, opcClienthelper.OpcDataItems) || opcClienthelper.OpcDataItems.Count < 1)
            {
                message = DateTime.Now.ToString(dateString) + "没有数据点" + System.Environment.NewLine;
            }
            else
            {
                var opcDataItem = opcClienthelper.OpcDataItems.FirstOrDefault().Clone() as OpcDataItem;
                message = DateTime.Now.ToString(dateString) + "读完成 " + (opcDataItem == null ? " " : opcDataItem.ToString()) + System.Environment.NewLine;
            }
            asyncUpMessage(message);
        }

        private void btnUpdateDataItems_Click(object sender, RoutedEventArgs e)
        {
            string message;
            if (!opcClienthelper.IsConnected)
            {
                message = DateTime.Now.ToString(dateString) + "请先连接服务器" + System.Environment.NewLine;
                asyncUpMessage(message);
                return;
            }
            var strList = txtOpcDataItems.Text.Split('\r', '\n');

            List<OpcDataItem> opcDataItems = new List<OpcDataItem>(strList.Count());
            //txtOpcDataItems .Text.Split (System.Environment.NewLine ):
            foreach (var strOpcDataItem in strList)
            {
                var strOpcDataItemTmp = strOpcDataItem.Split(';');
                if (strOpcDataItemTmp.Count() < 2)
                {
                    continue;
                }
                OpcDataItem opcDataItem =
                    new OpcDataItem(strOpcDataItemTmp[0], int.Parse(strOpcDataItemTmp[1]), strOpcDataItemTmp[2], strOpcDataItemTmp[3], (OpcResult)Enum.Parse(typeof(OpcResult), strOpcDataItemTmp[4]));
                opcDataItems.Add(opcDataItem);
            }
            opcClienthelper.RegisterOpcDataItemsAsync(opcDataItems);
            dataGridDataSource = new ObservableCollection<OpcDataItem>(opcDataItems);
            gvOpcDataItems.ItemsSource = dataGridDataSource;
            this.txtb.Text = "(" + dataGridDataSource.Count(a => a.Quality == OpcResult.S_OK) + "/" + dataGridDataSource.Count() + ")";

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
    }
}
