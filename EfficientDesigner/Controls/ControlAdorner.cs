using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

        public ControlAdorner(UIElement adornedElement, Panel ownPanel) : base(adornedElement)
        {
            VisualChilderns = new VisualCollection(this);
            OwnPanel = ownPanel;
            LeftTop.DragDelta += (sender, e) =>
            {
                double hor = e.HorizontalChange;
                double vert = e.VerticalChange;
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    if (DragStarted) IsHorizontalDrag = Math.Abs(hor) > Math.Abs(vert);
                    if (IsHorizontalDrag) vert = hor; else hor = vert;
                }
                ResizeX(hor);
                ResizeY(vert);
                DragStarted = false;
                e.Handled = true;
            };

            RightTop.DragDelta += (sender, e) =>
            {
                double hor = e.HorizontalChange;
                double vert = e.VerticalChange;
                System.Diagnostics.Debug.WriteLine(hor + "," + vert + "," + (Math.Abs(hor) > Math.Abs(vert)) + "," + ChildElement.Height + "," + ChildElement.Width + "," + DragStarted + "," + IsHorizontalDrag);
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    if (DragStarted) IsHorizontalDrag = Math.Abs(hor) > Math.Abs(vert);
                    if (IsHorizontalDrag) vert = -hor; else hor = -vert;
                }
                ResizeWidth(hor);
                ResizeY(vert);
                DragStarted = false;
                e.Handled = true;
            };
            LeftBottom.DragDelta += (sender, e) =>
            {
                double hor = e.HorizontalChange;
                double vert = e.VerticalChange;
                System.Diagnostics.Debug.WriteLine(hor + "," + vert + "," + (Math.Abs(hor) > Math.Abs(vert)) + "," + ChildElement.Height + "," + ChildElement.Width + "," + DragStarted + "," + IsHorizontalDrag);
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    if (DragStarted) IsHorizontalDrag = Math.Abs(hor) > Math.Abs(vert);
                    if (IsHorizontalDrag) vert = -hor; else hor = -vert;
                }
                ResizeX(hor);
                ResizeHeight(vert);
                DragStarted = false;
                e.Handled = true;
            };
            RightBottom.DragDelta += (sender, e) =>
            {
                double hor = e.HorizontalChange;
                double vert = e.VerticalChange;
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    if (DragStarted) IsHorizontalDrag = Math.Abs(hor) > Math.Abs(vert);
                    if (IsHorizontalDrag) vert = hor; else hor = vert;
                }
                ResizeWidth(hor);
                ResizeHeight(vert);
                DragStarted = false;
                e.Handled = true;
            };

        }

        private Thumb _LeftTop;

        public Thumb LeftTop
        {
            get
            {
                if (_LeftTop == null)
                {
                    _LeftTop = GetThumb();
                }
                return _LeftTop;
            }
        }

        private Thumb _LeftBottom;

        public Thumb LeftBottom
        {
            get
            {
                if (_LeftBottom == null)
                {
                    _LeftBottom = GetThumb();
                }
                return _LeftBottom;
            }
        }

        private Thumb _RightTop;

        public Thumb RightTop
        {
            get
            {
                if (_RightTop == null)
                {
                    _RightTop = GetThumb();
                }
                return _RightTop;
            }
        }

        private Thumb _RightBottom;

        public Thumb RightBottom
        {
            get
            {
                if (_RightBottom == null)
                {
                    _RightBottom = GetThumb();
                }
                return _RightBottom;
            }
        }

        private Thumb GetThumb()
        {
            var thumb = new Thumb { Width = 10, Height = 10, Background = Brushes.Black };
            thumb.DragStarted += (object sender, DragStartedEventArgs e) => DragStarted = true;
            VisualChilderns.Add(thumb);
            return thumb;
        }


        private double Angle { get; set; }

        private bool DragStarted { get; set; }
        private bool IsHorizontalDrag { get; set; }

        private Point TransformOrigin { get; set; } = new Point(0, 0);

        public static readonly RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent(
    "Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ControlAdorner));

        // Provide CLR accessors for the event
        public event RoutedEventHandler Selected
        {
            add { AddHandler(SelectedEvent, value); }
            remove { RemoveHandler(SelectedEvent, value); }
        }

        public void RaiseSelectedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(SelectedEvent, this);
            RaiseEvent(newEventArgs);
        }

        public static readonly RoutedEvent MoveEvent = EventManager.RegisterRoutedEvent(
    "Move", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ControlAdorner));

        // Provide CLR accessors for the event
        public event RoutedEventHandler Move
        {
            add { AddHandler(MoveEvent, value); }
            remove { RemoveHandler(MoveEvent, value); }
        }

        /// <summary>
        /// 移动时触发的事件，用于在画板中更新垂直线和水平线
        /// </summary>
        /// <param name="point1">canvas.left和canvans.top</param>
        /// <param name="point2">(canvas.left + width/2) 和 (canvas.top + height/2)</param>
        public void RaiseMoveEvent(Point point1, Point point2)
        {
            var args = new RoutedEventArgs(MoveEvent, new Point[] { point1, point2 });
            RaiseEvent(args);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            //base.OnRender(drawingContext);
            if (IsSelected)
            {
                MousePoint = Mouse.GetPosition(AdornedElement);

                LeftBottom.Visibility = Visibility.Visible;
                LeftTop.Visibility = Visibility.Visible;
                RightBottom.Visibility = Visibility.Visible;
                RightTop.Visibility = Visibility.Visible;
                drawingContext.DrawRectangle(Brushes.Transparent, BorderPen, new Rect(-1, -1, AdornedElement.DesiredSize.Width + 1, AdornedElement.DesiredSize.Height + 1));
            }
            else
            {
                LeftBottom.Visibility = Visibility.Collapsed;
                LeftTop.Visibility = Visibility.Collapsed;
                RightBottom.Visibility = Visibility.Collapsed;
                RightTop.Visibility = Visibility.Collapsed;
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

        private FrameworkElement ChildElement => AdornedElement as FrameworkElement;

        private void ResizeWidth(double e)
        {
            double deltaHorizontal = Math.Min(-e, ChildElement.ActualWidth - ChildElement.MinWidth);
            Canvas.SetTop(ChildElement, Canvas.GetTop(ChildElement) - TransformOrigin.X * deltaHorizontal * Math.Sin(Angle));
            Canvas.SetLeft(ChildElement, Canvas.GetLeft(ChildElement) + (deltaHorizontal * TransformOrigin.X * (1 - Math.Cos(Angle))));

            ChildElement.Width = Math.Max(4, ChildElement.Width - deltaHorizontal);
        }
        private void ResizeX(double e)
        {
            double deltaHorizontal = Math.Min(e, ChildElement.ActualWidth - ChildElement.MinWidth);
            Canvas.SetTop(ChildElement, Canvas.GetTop(ChildElement) + deltaHorizontal * Math.Sin(Angle) - TransformOrigin.X * deltaHorizontal * Math.Sin(Angle));
            Canvas.SetLeft(ChildElement, Canvas.GetLeft(ChildElement) + deltaHorizontal * Math.Cos(Angle) + (TransformOrigin.X * deltaHorizontal * (1 - Math.Cos(Angle))));
            ChildElement.Width = Math.Max(4, ChildElement.Width - deltaHorizontal);
        }
        private void ResizeHeight(double e)
        {
            double deltaVertical = Math.Min(-e, ChildElement.ActualHeight - ChildElement.MinHeight);
            Canvas.SetTop(ChildElement, Canvas.GetTop(ChildElement) + (TransformOrigin.Y * deltaVertical * (1 - Math.Cos(-Angle))));
            Canvas.SetLeft(ChildElement, Canvas.GetLeft(ChildElement) - deltaVertical * TransformOrigin.Y * Math.Sin(-Angle));
            ChildElement.Height = Math.Max(4, ChildElement.Height - deltaVertical);
        }
        private void ResizeY(double e)
        {
            double deltaVertical = Math.Min(e, ChildElement.ActualHeight - ChildElement.MinHeight);
            Canvas.SetTop(ChildElement, Canvas.GetTop(ChildElement) + deltaVertical * Math.Cos(-Angle) + (TransformOrigin.Y * deltaVertical * (1 - Math.Cos(-Angle))));
            Canvas.SetLeft(ChildElement, Canvas.GetLeft(ChildElement) + deltaVertical * Math.Sin(-Angle) - (TransformOrigin.Y * deltaVertical * Math.Sin(-Angle)));
            ChildElement.Height = Math.Max(4, ChildElement.Height - deltaVertical);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            base.ArrangeOverride(finalSize);
            double desireWidth = AdornedElement.DesiredSize.Width;
            double desireHeight = AdornedElement.DesiredSize.Height;
            double adornerWidth = this.DesiredSize.Width;
            double adornerHeight = this.DesiredSize.Height;
            LeftTop.Arrange(new Rect(-adornerWidth / 2 - 2, -adornerHeight / 2 - 2, adornerWidth, adornerHeight));
            RightTop.Arrange(new Rect(desireWidth - adornerWidth / 2 + 2, -adornerHeight / 2 - 2, adornerWidth, adornerHeight));
            LeftBottom.Arrange(new Rect(-adornerWidth / 2 - 2, desireHeight - adornerHeight / 2 + 2, adornerWidth, adornerHeight));
            RightBottom.Arrange(new Rect(desireWidth - adornerWidth / 2 + 2, desireHeight - adornerHeight / 2 + 2, adornerWidth, adornerHeight));
            return finalSize;
        }

        private VisualCollection VisualChilderns { get; set; }

        protected override int VisualChildrenCount => VisualChilderns.Count;
        protected override Visual GetVisualChild(int index) => VisualChilderns[index];

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!(e.OriginalSource is Thumb) && e.LeftButton == MouseButtonState.Pressed)
            {
                var p = e.GetPosition(AdornedElement);
                var vector = p - MousePoint;

                var x = Canvas.GetLeft(AdornedElement) + vector.X;
                var y = Canvas.GetTop(AdornedElement) + vector.Y;

                x = Math.Max(0, Math.Min(x, OwnPanel.ActualWidth - this.ActualWidth));
                y = Math.Max(0, Math.Min(y, OwnPanel.ActualHeight - this.ActualHeight));

                Canvas.SetTop(AdornedElement, y);
                Canvas.SetLeft(AdornedElement, x);

                RaiseMoveEvent(new Point(x, y), new Point(x + ChildElement.Width/ 2, y + ChildElement.Height / 2));
            }
        }

        public Panel OwnPanel { get; set; }

        private Point MousePoint { get; set; }


    }
}
