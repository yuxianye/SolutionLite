using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Desktop.PublicControl
{
    public class IconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = Utility.Windows.BitmapImageHelper.GetImage(value?.ToString(), 16, 16);

            if (Equals(source, null))
            {
                return Utility.Windows.BitmapImageHelper.GetImage(Utility.ConstValue.DefaultIconPath, 16, 16);
            }
            return source;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
