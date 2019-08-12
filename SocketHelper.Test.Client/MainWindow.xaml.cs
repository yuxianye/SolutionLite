using System;
using System.Collections.Generic;
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

namespace SocketHelper.Test.Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();




        }



        SocketHelper.ClientHelper clientHelper = new SocketHelper.ClientHelper();

        private void btn_Connect_Click(object sender, RoutedEventArgs e)
        {
            clientHelper.OnPackagReceived += ClientHelper_OnPackagReceived;
            var v = clientHelper.ConnectAsync(new System.Net.IPEndPoint(System.Net.IPAddress.Parse(txt_IP.Text.Trim()), int.Parse(txt_Port.Text))).Result;
            LogHelper.Logger.Info($"连接结果：{v}");
            LogHelper.Logger.Info($"连接状态：{clientHelper.IsConnected}");

            string msg = "T_PrimaryData       1288 PES     SlabScal3         2018-08-29 16:04:23    000000                                                                                                                                2018-08-29 16:04:21UP2018-08-29 16:04:21SCP01     2018-08-29 16:04:21006SlabID0829160421000100011                   POID08291604215551                      Alloy1    00000101000002010000021100000221000003010000040100000501000601000701000801SlabID0829160421000200022                   POID08291604215551                      Alloy2    00000102000002020000021200000222000003020000040200000502000602000702000802SlabID0829160421000300033                   POID08291604215551                      Alloy3    00000103000002030000021300000223000003030000040300000503000603000703000803SlabID0829160421000400044                   POID08291604215551                      Alloy4    00000104000002040000021400000224000003040000040400000504000604000704000804SlabID0829160421000500055                   POID08291604215551                      Alloy5    00000105000002050000021500000225000003050000040500000505000605000705000805SlabID0829160421000600066                   POID08291604215551                      Alloy6    00000106000002060000021600000226000003060000040600000506000606000706000806";

            //msg = "T_PrimaryData       1120 PES     SlabScal2         2018-08-29 16:03:32    000000                                                                                                                                2018-08-29 16:03:30UP2018-08-29 16:03:30SCP01     2018-08-29 16:03:30005SlabID0829160330000100011                   POID08291603306859                      Alloy1    00000101000002010000021100000221000003010000040100000501000601000701000801SlabID0829160330000200022                   POID08291603306859                      Alloy2    00000102000002020000021200000222000003020000040200000502000602000702000802SlabID0829160330000300033                   POID08291603306859                      Alloy3    00000103000002030000021300000223000003030000040300000503000603000703000803SlabID0829160330000400044                   POID08291603306859                      Alloy4    00000104000002040000021400000224000003040000040400000504000604000704000804SlabID0829160330000500055                   POID08291603306859                      Alloy5    00000105000002050000021500000225000003050000040500000505000605000705000805";
            v = clientHelper.Send(System.Text.Encoding.ASCII.GetBytes(msg));
            LogHelper.Logger.Info($"发送结果：{v}");
        }
        private void ClientHelper_OnPackagReceived(object sender, SocketHelper.ClientPackageInfo e)
        {
            LogHelper.Logger.Info($"收到数据包：{e}");

            //throw new NotImplementedException();
        }



    }
}
