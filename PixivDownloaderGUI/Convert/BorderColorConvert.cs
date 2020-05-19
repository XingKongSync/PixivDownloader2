using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PixivDownloaderGUI.Convert
{
    class BorderColorConvert : IValueConverter
    {
        private static readonly SolidColorBrush _pinkBrush2 = new SolidColorBrush(Color.FromArgb(0x30, 0xFF, 0x00, 0x00));
        private static readonly SolidColorBrush _pinkBrush3 = new SolidColorBrush(Color.FromArgb(0x20, 0xFF, 0x00, 0x00));
        private static readonly SolidColorBrush _pinkBrush4 = new SolidColorBrush(Color.FromArgb(0x10, 0xFF, 0x00, 0x00));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float fval)
            {
                if (fval >= 0.86)
                {
                    return _pinkBrush2;
                }
                if (fval >= 0.73)
                {
                    return _pinkBrush3;
                }
                if (fval >= 0.6)
                {
                    return _pinkBrush4;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
