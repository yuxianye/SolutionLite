using System;
using System.Windows;

namespace PortsHelper.Test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            this.cb_PortName.ItemsSource = PortsHelper.SerialPortHelper.GetPortNames();
            this.cb_PortName.SelectedIndex = 0;
            this.cb_PortName.SelectedIndex = 2;

            this.cb_BaudRate.ItemsSource = Enum.GetValues(typeof(PortsHelper.SerialPortBaudRates));
            this.cb_BaudRate.SelectedIndex = 8;

            this.cb_Parity.ItemsSource = Enum.GetValues(typeof(System.IO.Ports.Parity));
            this.cb_Parity.SelectedIndex = 0;

            this.cb_DataBits.ItemsSource = Enum.GetValues(typeof(PortsHelper.SerialPortDatabits));
            this.cb_DataBits.SelectedIndex = 3;

            this.cb_StopBits.ItemsSource = Enum.GetValues(typeof(System.IO.Ports.StopBits));
            this.cb_StopBits.SelectedIndex = 0;
            //
            this.cb_StopBits.SelectedIndex = 1;
        }
        PortsHelper.SerialPortHelper serialPortHelper = new SerialPortHelper();

        private void btn_Connect_Click(object sender, RoutedEventArgs e)
        {
            bool result = serialPortHelper.OpenPort();
            rtxt_Message.AppendText($"串口打开结果：{ result}{System.Environment.NewLine}");
            if (result)
            {
                btn_Connect.IsEnabled = false;
                btn_Close.IsEnabled = true;
                //
                /////////////////////
                serialPortHelper.OnDataReceived -= SerialPortHelper_OnDataReceived;
                serialPortHelper.OnError -= SerialPortHelper_OnError;

                serialPortHelper.OnDataReceived += SerialPortHelper_OnDataReceived;
                serialPortHelper.OnError += SerialPortHelper_OnError;
                /////////////////////
            }
        }


        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            bool result = serialPortHelper.ClosePort();
            rtxt_Message.AppendText($"串口关闭结果：{ result}{System.Environment.NewLine}");
            if (result)
            {
                btn_Connect.IsEnabled = true;
                btn_Close.IsEnabled = false;
            }
        }

        private void btn_Send_Click(object sender, RoutedEventArgs e)
        {
            bool result = serialPortHelper.WriteData(txt_Data.Text.Trim());
            rtxt_Message.AppendText($"发送结果：{ result}{System.Environment.NewLine}");
            if (result)
            {

            }
        }

        private void SerialPortHelper_OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            try
            { 
                //string readString = System.Text.Encoding.Default.GetString(e.Data, 0, e.Data.Length);
                //upUiMessage($"收到串口信息：" + readString);
                //if (readString.Equals("A"))
                //{
                //    //MessageBox.Show("aa");
                //}
                //
                //标准连续输出
                Byte bStx = e.Data[0];
                if (bStx == 2 )
                { 
                    //
                    //48 -> "110000"
                    string sSB_A = Convert.ToString(e.Data[1], 2);
                    //
                    string sSB_B = Convert.ToString(e.Data[2], 2);
                    //
                    //65 -> "1000001"
                    string sSB_C = Convert.ToString(e.Data[3], 2);
                    //
                    string sWeight1 = System.Text.Encoding.Default.GetString(e.Data, 4, 6);
                    string sWeight2 = System.Text.Encoding.Default.GetString(e.Data, 10, 6);
                    //
                    string sSB_A_0 = sSB_A.Length - 1 > 0 ? sSB_A.Substring(sSB_A.Length - 1 , 1 ) : "";
                    string sSB_A_1 = sSB_A.Length - 2 > 0 ? sSB_A.Substring(sSB_A.Length - 2 , 1)  : "";
                    string sSB_A_2 = sSB_A.Length - 3 > 0 ? sSB_A.Substring(sSB_A.Length - 3 , 1)  : "";
                    //
                    string sSB_B_0 = sSB_B.Length - 1 > 0 ? sSB_B.Substring(sSB_B.Length - 1, 1) : ""; 
                    string sSB_B_1 = sSB_B.Length - 2 > 0 ? sSB_B.Substring(sSB_B.Length - 2, 1) : "";
                    string sSB_B_2 = sSB_B.Length - 3 > 0 ? sSB_B.Substring(sSB_B.Length - 3, 1) : "";
                    string sSB_B_3 = sSB_B.Length - 4 > 0 ? sSB_B.Substring(sSB_B.Length - 4, 1) : "";
                    string sSB_B_4 = sSB_B.Length - 5 > 0 ? sSB_B.Substring(sSB_B.Length - 5, 1) : "";
                    string sSB_B_5 = sSB_B.Length - 6 > 0 ? sSB_B.Substring(sSB_B.Length - 6, 1) : "";
                    string sSB_B_6 = sSB_B.Length - 7 > 0 ? sSB_B.Substring(sSB_B.Length - 7, 1) : "";
                    //
                    string sSB_C_0 = sSB_C.Length - 1 > 0 ? sSB_C.Substring(sSB_C.Length - 1, 1) : "";
                    string sSB_C_1 = sSB_C.Length - 2 > 0 ? sSB_C.Substring(sSB_C.Length - 2, 1) : "";
                    string sSB_C_2 = sSB_C.Length - 3 > 0 ? sSB_C.Substring(sSB_C.Length - 3, 1) : "";
                    string sSB_C_3 = sSB_C.Length - 4 > 0 ? sSB_C.Substring(sSB_C.Length - 4, 1) : "";
                    string sSB_C_4 = sSB_C.Length - 5 > 0 ? sSB_C.Substring(sSB_C.Length - 5, 1) : "";
                    string sSB_C_5 = sSB_C.Length - 6 > 0 ? sSB_C.Substring(sSB_C.Length - 6, 1) : "";
                    string sSB_C_6 = sSB_C.Length - 7 > 0 ? sSB_C.Substring(sSB_C.Length - 7, 1) : "";
                    //
                    //状态字节A位
                    if (sSB_A_2.Equals("0") && sSB_A_1.Equals("0") && sSB_A_0.Equals("0") )
                    {
                        sWeight1 = sWeight1 + "00";
                        sWeight2 = sWeight2 + "00";
                    }
                    else if (sSB_A_2.Equals("0") && sSB_A_1.Equals("0") && sSB_A_0.Equals("1"))
                    {
                        sWeight1 = sWeight1 + "0";
                        sWeight2 = sWeight2 + "0";
                    }
                    else if (sSB_A_2.Equals("0") && sSB_A_1.Equals("1") && sSB_A_0.Equals("0"))
                    {
                        //sWeight1 = sWeight1;
                        //sWeight2 = sWeight2;
                    }
                    else if (sSB_A_2.Equals("0") && sSB_A_1.Equals("1") && sSB_A_0.Equals("1"))
                    {
                        sWeight1 = sWeight1.Insert(5, ".");
                        sWeight2 = sWeight2.Insert(5, ".");
                    }
                    else if (sSB_A_2.Equals("1") && sSB_A_1.Equals("0") && sSB_A_0.Equals("0"))
                    {
                        sWeight1 = sWeight1.Insert(4, ".");
                        sWeight2 = sWeight2.Insert(4, ".");
                    }
                    else if (sSB_A_2.Equals("1") && sSB_A_1.Equals("0") && sSB_A_0.Equals("1"))
                    {
                        sWeight1 = sWeight1.Insert(3, ".");
                        sWeight2 = sWeight2.Insert(3, ".");
                    }
                    else if (sSB_A_2.Equals("1") && sSB_A_1.Equals("1") && sSB_A_0.Equals("0"))
                    {
                        sWeight1 = sWeight1.Insert(2, ".");
                        sWeight2 = sWeight2.Insert(2, ".");
                    }
                    else if (sSB_A_2.Equals("1") && sSB_A_1.Equals("1") && sSB_A_0.Equals("1"))
                    {
                        sWeight1 = sWeight1.Insert(1, ".");
                        sWeight2 = sWeight2.Insert(1, ".");
                    }
                    //
                    //
                    //状态字节B位
                    if ( sSB_B_0.Equals("0"))
                    {
                        sWeight1 = sWeight1 + "(毛重)";
                    }
                    else if (sSB_B_0.Equals("1"))
                    {
                        sWeight1 = sWeight1 + "(净重)";
                    }
                    //正负
                    if ( sSB_B_1.Equals("0"))
                    {
                        sWeight1 = "+" + sWeight1;
                        sWeight2 = "+" + sWeight2;
                    }
                    else if ( sSB_B_1.Equals("1"))
                    {
                        sWeight1 = "-" + sWeight1;
                        sWeight2 = "-" + sWeight2;
                    }
                    //动态 稳定
                    if (sSB_B_3.Equals("1"))
                    {
                        sWeight1 = sWeight1 + "(动态)";
                        sWeight2 = sWeight2 + "(动态)";
                    }
                    else if (sSB_B_3.Equals("0"))
                    {
                        sWeight1 = sWeight1 + "(稳定)";
                        sWeight2 = sWeight2 + "(稳定)";
                    }
                    //动态 稳定
                    if (sSB_B_4.Equals("1"))
                    {
                        sWeight1 = sWeight1 + "(KG)";
                        sWeight2 = sWeight2 + "(KG)";
                    }
                    //
                    //
                    //状态字节C位

                    //
                    upUiMessage($"指示重量信息：" + sWeight1);
                    upUiMessage($"皮重信息：" + sWeight2);
                    //
                    LogHelper.Logger.Info("获取电子秤信息成功！");
                }
            }
            catch (Exception ex)
            {
                string msg = $"获取电子秤信息错误！{ex.Message}";
                LogHelper.Logger.Error(msg, ex);
            }
        }

        private void SerialPortHelper_OnError(object sender, SerialPortErrorEventArgs e)
        {

        }

        private void cb_PortName_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string sPortName = cb_PortName.Items[cb_PortName.SelectedIndex].ToString();
            //
            serialPortHelper.PortName  = sPortName;
        }

        private void upUiMessage(string message)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                //tb_Message.Text = message;
                rtxt_Message.AppendText(message);
                rtxt_Message.AppendText(System.Environment.NewLine);
                rtxt_Message.ScrollToEnd();
                //MessageBox.Show(message, this.Title, MessageBoxButton.OK, MessageBoxImage.Information);

            }));
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            //btn_Connect_Click(btn_Connect,null);
        }
    }
}
