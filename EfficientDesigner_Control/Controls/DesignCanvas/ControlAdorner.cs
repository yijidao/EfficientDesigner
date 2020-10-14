using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace EfficientDesigner_Control.Controls
{
    public class ControlAdorner : Adorner
    {
        /*
         * https://stackoverflow.com/questions/59803689/wpf-adorner-for-control-user-resizable
         * 使用Thumb调整大小
         */

        public ControlAdorner(UIElement adornedElement, Panel ownPanel) : base(adornedElement)
        {
            VisualChildren = new VisualCollection(this);
            OwnPanel = ownPanel;
            LeftTop.DragDelta += (sender, e) =>
            {
                var hor = e.HorizontalChange;
                var ver = e.VerticalChange;
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    if (DragStarted) IsHorizontalDrag = Math.Abs(hor) > Math.Abs(ver);
                    if (IsHorizontalDrag) ver = hor; else hor = ver;
                }
                ResizeX(hor);
                ResizeY(ver);
                RaiseEvent(new RoutedEventArgs(DecoratorSizeChangedEvent, this));
                DragStarted = false;
                e.Handled = true;
            };

            RightTop.DragDelta += (sender, e) =>
            {
                var hor = e.HorizontalChange;
                var ver = e.VerticalChange;
                Debug.WriteLine(hor + "," + ver + "," + (Math.Abs(hor) > Math.Abs(ver)) + "," + ChildElement.Height + "," + ChildElement.Width + "," + DragStarted + "," + IsHorizontalDrag);
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    if (DragStarted) IsHorizontalDrag = Math.Abs(hor) > Math.Abs(ver);
                    if (IsHorizontalDrag) ver = -hor; else hor = -ver;
                }
                ResizeWidth(hor);
                ResizeY(ver);
                RaiseEvent(new RoutedEventArgs(DecoratorSizeChangedEvent, this));
                DragStarted = false;
                e.Handled = true;
            };
            LeftBottom.DragDelta += (sender, e) =>
            {
                double hor = e.HorizontalChange;
                double ver = e.VerticalChange;
                Debug.WriteLine(hor + "," + ver + "," + (Math.Abs(hor) > Math.Abs(ver)) + "," + ChildElement.Height + "," + ChildElement.Width + "," + DragStarted + "," + IsHorizontalDrag);
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    if (DragStarted) IsHorizontalDrag = Math.Abs(hor) > Math.Abs(ver);
                    if (IsHorizontalDrag) ver = -hor; else hor = -ver;
                }
                ResizeX(hor);
                ResizeHeight(ver);
                RaiseEvent(new RoutedEventArgs(DecoratorSizeChangedEvent, this));
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
                RaiseEvent(new RoutedEventArgs(DecoratorSizeChangedEvent, this));
                DragStarted = false;
                e.Handled = true;
            };

        }

        private Thumb _leftTop;

        public Thumb LeftTop => _leftTop ??= GetThumb();

        private Thumb _leftBottom;

        public Thumb LeftBottom => _leftBottom ??= GetThumb();

        private Thumb _rightTop;

        public Thumb RightTop => _rightTop ??= GetThumb();

        private Thumb _rightBottom;

        public Thumb RightBottom => _rightBottom ??= GetThumb();

        private Thumb GetThumb()
        {
            var thumb = new Thumb { Width = 10, Height = 10, Background = Brushes.Black };
            thumb.DragStarted += (sender, e) => DragStarted = true;
            VisualChildren.Add(thumb);
            return thumb;
        }


        private double Angle { get; set; }

        private bool DragStarted { get; set; }
        private bool IsHorizontalDrag { get; set; }

        private Point TransformOrigin { get; set; } = new Point(0, 0);

        public static readonly RoutedEvent MoveEvent = EventManager.RegisterRoutedEvent(
    "Move", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ControlAdorner));

        public event RoutedEventHandler Move
        {
            add => AddHandler(MoveEvent, value);
            remove => RemoveHandler(MoveEvent, value);
        }

        public static readonly RoutedEvent DecoratorSizeChangedEvent =
            EventManager.RegisterRoutedEvent("DecoratorSizeChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler),
                typeof(ControlAdorner));

        public event RoutedEventHandler DecoratorSizeChanged
        {
            add => AddHandler(DecoratorSizeChangedEvent, value);
            remove => RemoveHandler(DecoratorSizeChangedEvent, value);
        }

        /// <summary>
        /// 移动时触发的事件，用于在画板中更新垂直线和水平线
        /// </summary>
        public void RaiseMoveEvent()
        {
            var args = new RoutedEventArgs(MoveEvent, this);
            RaiseEvent(args);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            //base.OnRender(drawingContext);
            if (IsSelected)
            {
                LeftBottom.Visibility = Visibility.Visible;
                LeftTop.Visibility = Visibility.Visible;
                RightBottom.Visibility = Visibility.Visible;
                RightTop.Visibility = Visibility.Visible;
                drawingContext.DrawRectangle(Brushes.Transparent, BorderPen, new Rect(-1, -1, AdornedElement.DesiredSize.Width + 2, AdornedElement.DesiredSize.Height + 2));
            }
            else
            {
                LeftBottom.Visibility = Visibility.Collapsed;
                LeftTop.Visibility = Visibility.Collapsed;
                RightBottom.Visibility = Visibility.Collapsed;
                RightTop.Visibility = Visibility.Collapsed;
                //drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(-1, -1, AdornedElement.DesiredSize.Width + 1, AdornedElement.DesiredSize.Height + 1));
            }
        }

        public static Pen BorderPen { get; set; } = new Pen(Brushes.DeepSkyBlue, 1);

        public bool IsSelected
        {
            get;
            set;
        }

        private FrameworkElement ChildElement => AdornedElement as FrameworkElement;

        private void ResizeWidth(double e)
        {
            e = Math.Floor(e);
            double deltaHorizontal = Math.Min(-e, ChildElement.ActualWidth - ChildElement.MinWidth);
            Canvas.SetTop(ChildElement,  Canvas.GetTop(ChildElement) - TransformOrigin.X * deltaHorizontal * Math.Sin(Angle));
            Canvas.SetLeft(ChildElement, Canvas.GetLeft(ChildElement) + (deltaHorizontal * TransformOrigin.X * (1 - Math.Cos(Angle))));

            ChildElement.Width = Math.Max(4, ChildElement.Width - deltaHorizontal);
        }
        private void ResizeX(double e)
        {
            e = Math.Floor(e);
            double deltaHorizontal = Math.Min(e, ChildElement.ActualWidth - ChildElement.MinWidth);
            Canvas.SetTop(ChildElement, Canvas.GetTop(ChildElement) + deltaHorizontal * Math.Sin(Angle) - TransformOrigin.X * deltaHorizontal * Math.Sin(Angle));
            Canvas.SetLeft(ChildElement, Canvas.GetLeft(ChildElement) + deltaHorizontal * Math.Cos(Angle) + (TransformOrigin.X * deltaHorizontal * (1 - Math.Cos(Angle))));
            ChildElement.Width = Math.Max(4, ChildElement.Width - deltaHorizontal);
        }
        private void ResizeHeight(double e)
        {
            e = Math.Floor(e);
            double deltaVertical = Math.Min(-e, ChildElement.ActualHeight - ChildElement.MinHeight);
            Canvas.SetTop(ChildElement, Canvas.GetTop(ChildElement) + (TransformOrigin.Y * deltaVertical * (1 - Math.Cos(-Angle))));
            Canvas.SetLeft(ChildElement, Canvas.GetLeft(ChildElement) - deltaVertical * TransformOrigin.Y * Math.Sin(-Angle));
            ChildElement.Height = Math.Max(4, ChildElement.Height - deltaVertical);
        }
        private void ResizeY(double e)
        {
            e = Math.Floor(e);
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
            double decoratorWidth = this.DesiredSize.Width;
            double decoratorHeight = this.DesiredSize.Height;
            LeftTop.Arrange(new Rect(-decoratorWidth / 2 - 2, -decoratorHeight / 2 - 2, decoratorWidth, decoratorHeight));
            RightTop.Arrange(new Rect(desireWidth - decoratorWidth / 2 + 2, -decoratorHeight / 2 - 2, decoratorWidth, decoratorHeight));
            LeftBottom.Arrange(new Rect(-decoratorWidth / 2 - 2, desireHeight - decoratorHeight / 2 + 2, decoratorWidth, decoratorHeight));
            RightBottom.Arrange(new Rect(desireWidth - decoratorWidth / 2 + 2, desireHeight - decoratorHeight / 2 + 2, decoratorWidth, decoratorHeight));
            return finalSize;
        }

        private VisualCollection VisualChildren { get; set; }

        protected override int VisualChildrenCount => VisualChildren.Count;
        protected override Visual GetVisualChild(int index) => VisualChildren[index];

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!(e.OriginalSource is Thumb) && e.LeftButton == MouseButtonState.Pressed)
            {
                RaiseMoveEvent();
            }
        }

        public Panel OwnPanel { get; set; }

        public Point MousePoint { get; private set; }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            var p = Mouse.GetPosition(AdornedElement);
            MousePoint = new Point(Math.Floor(p.X), Math.Floor(p.Y)); 
        }
    }
    }
