using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static EfficientDesigner_Control.Controls.PropertyResolver;

namespace EfficientDesigner_Control.Controls.Editors
{
    public class NumberPropertyEditor : PropertyEditorBase
    {
        public NumberPropertyEditor()
        {

        }

        public NumberPropertyEditor(double min, double max)
        {
            Min = min;
            Max = max;
        }

        public double Min { get; }
        public double Max { get; }

        public override FrameworkElement CreateElement(PropertyItem propertyItem) => new TextBox
        {
            IsEnabled = !propertyItem.IsReadOnly
        };

        public override DependencyProperty GetDependencyProperty() => TextBox.TextProperty;

        public override string GetFormat(PropertyItem propertyItem)
        {
            if (propertyItem.PropertyType == typeof(double) || propertyItem.PropertyType == typeof(float))
                return "N";
            else return "N0";
        }

        public override IValueConverter GetConverter(PropertyItem propertyItem) => new NumberConverter(Min, Max);
    }

    public class NumberConverter : IValueConverter
    {

        public NumberConverter(double min, double max)
        {
            Min = min;
            Max = max;
        }

        public double Min { get; }
        public double Max { get; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (double)value;
            if (v < Min) return Min;
            else if (v > Max) return Max;
            else return v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
    }
}
