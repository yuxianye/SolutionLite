using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Desktop.PublicControl
{
    /// <summary>
    /// Label走马灯自定义控件
    /// </summary>
    public class ScrollingTextBlock : TextBlock
    {

        public static readonly DependencyProperty ScrollTextProperty =
            DependencyProperty.RegisterAttached("ScrollText", typeof(string), typeof(ScrollingTextBlock), new FrameworkPropertyMetadata(string.Empty, OnScrollTextPropertyChanged));
        public static string GetScrollText(DependencyObject dp)
        {
            return (string)dp.GetValue(ScrollTextProperty);
        }
        public static void SetScrollText(DependencyObject dp, string value)
        {
            dp.SetValue(ScrollTextProperty, value);
        }


        /// <summary>
        /// 定时器
        /// </summary>
        System.Windows.Threading.DispatcherTimer MarqueeTimer = new System.Windows.Threading.DispatcherTimer();

        /// <summary>
        /// 输出文本
        /// </summary>
        private string outText = string.Empty;
        /// <summary>
        /// 过度文本存储
        /// </summary>
        private string tempString = string.Empty;

        /// <summary>
        /// 时间
        /// </summary>
        private DateTime _SignTime;

        /// <summary>
        /// 是否第一次
        /// </summary>
        private bool _IfFirst = true;

        /// <summary>
        /// 滚动一循环字幕停留的秒数,单位为毫秒,默认值停留3秒
        /// </summary>
        public int StopSecond { get; set; } = 3000;

        int runSpeed = 1;
        /// <summary>
        /// 滚动的速度(秒)
        /// </summary>
        [Description("文字滚动的速度")]　//显示在属性设计视图中的描述
        public int RunSpeed
        {
            get { return runSpeed; }
            set
            {
                runSpeed = value;
                MarqueeTimer.Interval = new TimeSpan(0, 0, RunSpeed);
            }
        }

        private string textSource;

        /// <summary>
        /// 滚动文字源
        /// </summary>
        [Description("文字滚动的Text")]
        public string TextSource
        {
            get { return textSource; }
            set
            {
                textSource = value;
                tempString = textSource + "   ";
                outText = tempString;
                if (string.IsNullOrWhiteSpace(value))
                {
                    Visibility = Visibility.Collapsed;
                }
                else
                {
                    Visibility = Visibility.Visible;

                }
            }
        }


        private static void OnScrollTextPropertyChanged(DependencyObject sender,
           DependencyPropertyChangedEventArgs e)
        {
            ScrollingTextBlock dp = sender as ScrollingTextBlock;
            if (e.NewValue != e.OldValue)
            {
                dp.TextSource = e.NewValue as string;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ScrollingTextBlock()
        {
            Visibility = Visibility.Collapsed;

            MarqueeTimer.Interval = new TimeSpan(0, 0, RunSpeed);//文字移动的速度
            MarqueeTimer.IsEnabled = true;      //开启定时触发事件
            MarqueeTimer.Tick += MarqueeTimer_Tick; ;//绑定定时事件
            this.Loaded += new RoutedEventHandler(ScrollingTextBlock_Loaded);//绑定控件Loaded事件
        }

        private void ScrollingTextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            tempString = textSource + "   ";
            outText = tempString;
            _SignTime = DateTime.Now;
            fontsize = this.FontSize;
            FontFamily = new FontFamily("Microsoft YaHei");
            this.FontFamily.ToString();

        }
        /// <summary>
        /// 字体大小
        /// </summary>
        private double fontsize;

        void MarqueeTimer_Tick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(outText)) return;

            if (outText.Substring(1) + outText[0] == tempString)
            {
                if (_IfFirst)
                {
                    _SignTime = DateTime.Now;
                }

                if ((DateTime.Now - _SignTime).TotalMilliseconds > StopSecond)
                {
                    _IfFirst = true; ;
                }
                else
                {
                    _IfFirst = false;
                    return;
                }
            }

            outText = outText.Substring(1) + outText[0];

            if (string.IsNullOrWhiteSpace(TextSource))
            {
                return;
            }
            var width = MeasureTextWidth(TextSource, fontsize, FontFamily.Source);

            if (this.ActualWidth > width)
            {
                outText = TextSource;
            }
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Text = outText;
            }));


        }

        private double MeasureTextWidth(string text, double fontSize, string fontFamily)
        {
            FormattedText formattedText = new FormattedText(
                text,
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                new Typeface(fontFamily.ToString()),
                fontSize,
              Brushes.Black
            );
            //System.Windows.DpiScale dpiScale = new DpiScale();


            //var v = dpiScale.PixelsPerDip;

            //PixelsPerDip
            // FormattedText formattedText2 = new PixelsPerDip(
            //    text,
            //    System.Globalization.CultureInfo.InvariantCulture,
            //    FlowDirection.LeftToRight,
            //    new Typeface(fontFamily.ToString()),
            //    fontSize,
            //  System.Windows.Media.Brushes.Black
            //);

            return formattedText.WidthIncludingTrailingWhitespace;
        }

    }
}
