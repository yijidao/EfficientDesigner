using EfficientDesigner.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EfficientDesigner.Models
{
    public class RectangleControl : IControl
    {
        public string Title => "Rectangle";

        public FrameworkElement GetElement()
        {
            return new Rectangle
            {
                Width = 100,
                Height = 100,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Fill = Brushes.Transparent
            };
        }
    }
}
