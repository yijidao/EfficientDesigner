using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EfficientDesigner.Controls
{
    public class PropertyItem : Control
    {


        public string DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register("DisplayName", typeof(string), typeof(PropertyItem), new PropertyMetadata(default(string)));

        public FrameworkElement EditorElement
        {
            get { return (FrameworkElement)GetValue(EditorElementProperty); }
            set { SetValue(EditorElementProperty, value); }
        }

        public static readonly DependencyProperty EditorElementProperty =
            DependencyProperty.Register("EditorElement", typeof(FrameworkElement), typeof(PropertyItem), new PropertyMetadata(default(string)));

        public static double GetDisplayNameWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(DisplayNameWidthProperty);
        }

        public static void SetDisplayNameWidth(DependencyObject obj, double value)
        {
            obj.SetValue(DisplayNameWidthProperty, value);
        }

        public static readonly DependencyProperty DisplayNameWidthProperty =
            DependencyProperty.RegisterAttached("DisplayNameWidth", typeof(double), typeof(PropertyItem), new PropertyMetadata(150d));


    }
}
