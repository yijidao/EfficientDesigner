using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EfficientDesigner_Control.ExtensionMethods
{
    public static class FrameworkElementExtensions
    {
        public static double GetCanvasTop(this FrameworkElement element) =>
            double.IsNaN(Canvas.GetTop(element)) ? 0 : Canvas.GetTop(element);

        public static double GetCanvasLeft(this FrameworkElement element) =>
            double.IsNaN(Canvas.GetLeft(element)) ? 0 : Canvas.GetLeft(element);
    }
}
