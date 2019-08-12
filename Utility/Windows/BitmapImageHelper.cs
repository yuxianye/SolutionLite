using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Utility.Windows
{
    /// <summary>
    /// 图像助手
    /// </summary>
    public class BitmapImageHelper
    {
        /// <summary>
        /// 获取图像
        /// </summary>
        /// <param name="uri">图像的路径。例如Images\\List_32x32.png或者pack://application:,,,/Solution.Desktop.Resource;component/Images/Add_32x32.png</param>
        /// <returns></returns>
        public static BitmapImage GetBitmapImage(string uri, int decodePixelHeight = 0, int decodePixelWidth = 0)
        {
            if (Equals(uri, null) || uri.Length < 1)
            {
                return null;
            }
            BitmapImage bitmapImage = new BitmapImage();
            try
            {
                bitmapImage.BeginInit();
                if (decodePixelHeight > 0)
                {
                    bitmapImage.DecodePixelHeight = decodePixelHeight;
                }
                if (decodePixelWidth > 0)
                {
                    bitmapImage.DecodePixelWidth = decodePixelWidth;
                }
                if (System.IO.File.Exists(uri))
                {
                    bitmapImage.StreamSource = System.IO.File.OpenRead(uri);
                }
                else
                {
                    bitmapImage.UriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
                }
                bitmapImage.EndInit();
                return bitmapImage;

            }
            catch (Exception)
            {
                //Utility.LogHelper.Error("bitmapImage.UriSource记载资源错误。", ex);
                return null;
            }
        }

        /// <summary>
        /// 取得图像内容
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static Image GetImage(string uri, int decodePixelHeight = 0, int decodePixelWidth = 0)
        {
            try
            {
                var source = GetBitmapImage(uri);
                //return new Image() { Source = GetBitmapImage(uri), Width = 16, Height = 16 };
                if (Equals(source, null))
                {
                    return null;
                }
                else
                {
                    var image = new Image() { Source = source };
                    if (decodePixelHeight > 0)
                    {
                        image.Height = decodePixelHeight;
                    }
                    if (decodePixelWidth > 0)
                    {
                        image.Height = decodePixelWidth;
                    }
                    return image;
                }

            }
            catch (Exception)
            {
                //Utility.LogHelper.Error("GetImage错误。", ex);
                return null;
            }
        }

    }
}
