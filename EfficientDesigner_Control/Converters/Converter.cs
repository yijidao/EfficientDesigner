using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace EfficientDesigner_Control.Converters
{
    public class CollapsedWhenFalse : IValueConverter
    {
        public static readonly CollapsedWhenFalse Instance = new CollapsedWhenFalse();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class Object2String : IValueConverter
    {
        public static readonly Object2String Instance = new Object2String();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value?.ToString();

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class String2Brush : IValueConverter
    {

        public static readonly String2Brush Instance = new String2Brush();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ColorConverter.ConvertFromString(value?.ToString()) is Color c)
            {
                return new SolidColorBrush(c);
            }
            else
            {
                return value;
            }


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }
    }
}
