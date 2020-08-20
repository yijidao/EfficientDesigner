using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace EfficientDesigner.Controls
{
    public class ControlAdorner : Adorner
    {
        /*
         * https://stackoverflow.com/questions/59803689/wpf-adorner-for-control-user-resizable
         * 使用Thumb调整大小
         */


        public ControlAdorner(UIElement adornedElement) : base(adornedElement)
        {
        }

        public static readonly RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent(
    "Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ControlAdorner));

        // Provide CLR accessors for the event
        public event RoutedEventHandler Selected
        {
            add { AddHandler(SelectedEvent, value); }
            remove { RemoveHandler(SelectedEvent, value); }
        }

        void RaiseSelectedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(SelectedEvent, this);
            RaiseEvent(newEventArgs);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            //base.OnRender(drawingContext);
            if (IsSelected)
            {
                drawingContext.DrawRectangle(Brushes.Transparent, BorderPen, new Rect(-1, -1, AdornedElement.DesiredSize.Width + 1, AdornedElement.DesiredSize.Height + 1));
            }
            else
            {
                drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(-1, -1, AdornedElement.DesiredSize.Width + 1, AdornedElement.DesiredSize.Height + 1));
            }

        }

        public static Pen BorderPen { get; set; } = new Pen(Brushes.DeepSkyBlue, 1);


        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            RaiseSelectedEvent();
            e.Handled = true;
        }

        public bool IsSelected { get; set; }

    }
}
