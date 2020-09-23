using EfficientDesigner_Control.Commands;
using EfficientDesigner_Control.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EfficientDesigner_Control.Controls
{
    public class DesignCanvas : Control
    {
        private const string CanvasName = "PART_Canvas";
        private const string VLineName = "PART_VerticalLine";
        private const string HLineName = "PART_HorizontalLine";
        private const string TopTextName = "PART_TopText";
        private const string LeftTextName = "PART_LeftText";
        private const string SelectedBoundName = "PART_SelectedBound";
        private const string ScrollViewerName = "PART_ScrollViewer";

        public ICommand SaveCommand
        {
            get => (ICommand)GetValue(SaveCommandProperty);
            set => SetValue(SaveCommandProperty, value);
        }

        /// <summary>
        /// 保存画布内容中的子元素
        /// </summary>
        public static readonly DependencyProperty SaveCommandProperty =
            DependencyProperty.Register("SaveCommand", typeof(ICommand), typeof(DesignCanvas), new PropertyMetadata(null));

        public ICommand LoadCommand
        {
            get => (ICommand)GetValue(LoadCommandProperty);
            set => SetValue(LoadCommandProperty, value);
        }

        /// <summary>
        /// 加载子元素到画布中
        /// </summary>
        public static readonly DependencyProperty LoadCommandProperty =
            DependencyProperty.Register("LoadCommand", typeof(ICommand), typeof(DesignCanvas), new PropertyMetadata(null));

        public ICommand SaveAsCommand
        {
            get => (ICommand)GetValue(SaveAsCommandProperty);
            set => SetValue(SaveAsCommandProperty, value);
        }

        public static readonly DependencyProperty SaveAsCommandProperty =
            DependencyProperty.Register("SaveAsCommand", typeof(ICommand), typeof(DesignCanvas), new PropertyMetadata(null));

        public DesignCanvas()
        {
            AllowDrop = true;
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DesignCanvas), new FrameworkPropertyMetadata(typeof(DesignCanvas)));
            SaveCommand = new DelegateCommand(Save);
            LoadCommand = new DelegateCommand(Load);
            SaveAsCommand = new DelegateCommand(SaveAs);
        }

        public bool AddedHandler { get; set; }

        //protected override void OnRender(DrawingContext drawingContext)
        //{
        //    base.OnRender(drawingContext);

        //    if (AddedHandler) return;
        //    AddedHandler = true;
        //    var layout = AdornerLayer.GetAdornerLayer(this);
        //    layout?.AddHandler(ControlAdorner.MoveEvent, new RoutedEventHandler(DesignPanel_ChildrenMove));
        //    layout?.AddHandler(ControlAdorner.DecoratorSizeChangedEvent, new RoutedEventHandler(DesignPanel_ChildSizeChanged));
        //    layout?.AddHandler(ControlAdorner.MouseDownEvent, new RoutedEventHandler(DesignPanel_ChildMouseDown));
        //}

        private void DesignPanel_ChildMouseDown(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is ControlAdorner decorator)) return;
            SelectedDecorators = null;
            SelectedDecorator = decorator;
        }

        private void DesignPanel_ChildSizeChanged(object sender, RoutedEventArgs e) => MoveLineAndText((e.OriginalSource as ControlAdorner)?.AdornedElement as FrameworkElement);

        /// <summary>
        /// 设置垂直线、水平线、垂直文本、水平文本的位置
        /// </summary>
        /// <param name="element"></param>
        private void MoveLineAndText(FrameworkElement element)
        {
            if (element == null)
            {
                TopText.Visibility = LeftText.Visibility = HLine.Visibility = VLine.Visibility = Visibility.Collapsed;
                return;
            }
            var top = Canvas.GetTop(element);
            var left = Canvas.GetLeft(element);

            HLine.Visibility = VLine.Visibility = Visibility.Visible;

            HLine.Y1 = HLine.Y2 = top + element.Height / 2;
            HLine.X2 = left + element.Width / 2;

            VLine.X1 = VLine.X2 = left + element.Width / 2;
            VLine.Y2 = top + element.Height / 2;

            Canvas.SetTop(TopText, top / 2);
            Canvas.SetLeft(TopText, VLine.X2 - 10);

            Canvas.SetTop(LeftText, HLine.Y2 - 10);
            Canvas.SetLeft(LeftText, left / 2);

            TopText.Text = top.ToString("0.##");
            LeftText.Text = left.ToString("0.##");

            TopText.Visibility = top < 30 ? Visibility.Collapsed : Visibility.Visible;
            LeftText.Visibility = left < 30 ? Visibility.Collapsed : Visibility.Visible;
        }

        protected override void OnDrop(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("control") || DesignPanel == null) return;

            var control = e.Data.GetData("control") as IControl;
            var element = control?.GetElement();
            if (element == null)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }

            var p = e.GetPosition(DesignPanel);
            Canvas.SetTop(element, p.Y);
            Canvas.SetLeft(element, p.X);

            AddChild(element);
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        /// <summary>
        /// 增加子元素到画布中
        /// </summary>
        /// <param name="child"></param>
        private void AddChild(FrameworkElement child)
        {
            if (child == null) return;

            DesignPanel.Children.Add(child);
            child.PreviewMouseDown += Child_PreviewMouseDown;

            var layout = AdornerLayer.GetAdornerLayer(child);
            layout?.Add(new ControlAdorner(child, DesignPanel));
        }

        private void Child_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            SelectedDecorators = null;

            if (e.Source is FrameworkElement element)
            {
                var layer = AdornerLayer.GetAdornerLayer(element);
                var decorator = layer?.GetAdorners(element)?.OfType<ControlAdorner>().FirstOrDefault();

                if (decorator == null) return;
                SelectedDecorator = decorator;
            }
        }

        public Canvas DesignPanel { get; set; }
        private Line HLine { get; set; }
        private Line VLine { get; set; }
        private TextBlock TopText { get; set; }
        private TextBlock LeftText { get; set; }

        private ControlAdorner _selectedDecorator;
        private IEnumerable<ControlAdorner> _selectedDecorators;

        private ControlAdorner SelectedDecorator
        {
            get => _selectedDecorator;
            set
            {
                if (_selectedDecorator != null)
                {
                    _selectedDecorator.IsSelected = false;
                    _selectedDecorator.InvalidateVisual();
                }
                if (value != null)
                {
                    value.IsSelected = true;
                    value.InvalidateVisual();
                }

                _selectedDecorator = value;
                SelectedElement = value?.AdornedElement;
            }
        }

        private IEnumerable<ControlAdorner> SelectedDecorators
        {
            get => _selectedDecorators;
            set
            {
                // 重新渲染之前选中的子控件
                if (_selectedDecorators != null)
                {
                    foreach (var decorator in _selectedDecorators?.Where(x => x != null))
                    {
                        decorator.IsSelected = false;
                        decorator.InvalidateVisual();
                    }
                }

                // 渲染新选中的子控件
                if (value != null)
                {
                    foreach (var decorator in value.Where(x => x != null))
                    {
                        decorator.IsSelected = true;
                        decorator.InvalidateVisual();
                    }
                }

                _selectedDecorators = value;
                SelectedElements = value?.Select(x => x.AdornedElement);
            }
        }

        public static readonly DependencyProperty SelectedElementsProperty = DependencyProperty.Register(
            "SelectedElements", typeof(IEnumerable<UIElement>), typeof(DesignCanvas), new PropertyMetadata(default(IEnumerable<UIElement>)));

        public IEnumerable<UIElement> SelectedElements
        {
            get => (IEnumerable<UIElement>)GetValue(SelectedElementsProperty);
            set => SetValue(SelectedElementsProperty, value);
        }

        public UIElement SelectedElement
        {
            get => (UIElement)GetValue(SelectedElementProperty);
            set => SetValue(SelectedElementProperty, value);
        }

        /// <summary>
        /// 选中的元素，提供给属性面板使用。
        /// </summary>
        public static readonly DependencyProperty SelectedElementProperty =
            DependencyProperty.Register("SelectedElement", typeof(UIElement), typeof(DesignCanvas), new PropertyMetadata(
                (o, args) =>
                {
                    var ctl = (DesignCanvas)o;
                    ctl.MoveLineAndText(args.NewValue as FrameworkElement);
                }));

        public Rectangle SelectedBound { get; private set; }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            HLine = GetTemplateChild(HLineName) as Line ?? throw new ArgumentException();
            VLine = GetTemplateChild(VLineName) as Line ?? throw new ArgumentException();
            TopText = GetTemplateChild(TopTextName) as TextBlock ?? throw new ArgumentException();
            LeftText = GetTemplateChild(LeftTextName) as TextBlock ?? throw new ArgumentException();
            DesignPanel = GetTemplateChild(CanvasName) as Canvas ?? throw new ArgumentException();
            SelectedBound = GetTemplateChild(SelectedBoundName) as Rectangle ?? throw new ArgumentException();
            ScrollContainer = GetTemplateChild(ScrollViewerName) as ScrollViewer ?? throw new ArgumentException();

            if (!AddedHandler)
            {
                AddedHandler = true;
                AddHandler(ControlAdorner.MoveEvent, new RoutedEventHandler(DesignPanel_ChildrenMove));
                AddHandler(ControlAdorner.DecoratorSizeChangedEvent, new RoutedEventHandler(DesignPanel_ChildSizeChanged));
                AddHandler(ControlAdorner.MouseDownEvent, new RoutedEventHandler(DesignPanel_ChildMouseDown));
            }

            ScrollContainer.PreviewMouseWheel += ScrollContainerOnMouseWheel;
        }


        private readonly double _zoomMax = 10;
        private readonly double _zoomMin = 0.1;
        private double _zoomSpeed = 0.001;

        public double Zoom
        {
            get => (double)GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }

        public static readonly DependencyProperty ZoomProperty =
            DependencyProperty.Register("Zoom", typeof(double), typeof(DesignCanvas), new PropertyMetadata(1d,
                (o, args) =>
                {
                    if (!(args.NewValue is double zoom)) return;
                    var ctl = (DesignCanvas)o;
                    if (zoom < ctl._zoomMin) zoom = ctl._zoomMin;
                    if (zoom > ctl._zoomMax) zoom = ctl._zoomMax;
                    ctl.DesignPanel.LayoutTransform = new ScaleTransform(zoom, zoom);
                }));

        private void ScrollContainerOnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Zoom += _zoomSpeed * e.Delta;

                if (Zoom < _zoomMin) Zoom = _zoomMin;
                if (Zoom > _zoomMax) Zoom = _zoomMax;

                DesignPanel.LayoutTransform = new ScaleTransform(Zoom, Zoom);

                //DesignPanel.LayoutTransform = Zoom > 1 ? new ScaleTransform(Zoom, Zoom, p.X, p.Y) : new ScaleTransform(Zoom, Zoom);
                //DesignPanel.RenderTransform = Zoom > 1 ? new ScaleTransform(Zoom, Zoom, p.X, p.Y) : new ScaleTransform(Zoom, Zoom);
            }
        }

        public ScrollViewer ScrollContainer { get; set; }

        private void DesignPanel_ChildrenMove(object sender, RoutedEventArgs e)
        {
            if (DoSelectMultiple) return;
            var mouseOnPanel = Mouse.GetPosition(DesignPanel);
            if (mouseOnPanel.X < 0 || mouseOnPanel.X > DesignPanel.Width || mouseOnPanel.Y < 0 ||
                mouseOnPanel.Y > DesignPanel.Height) return;

            if (!(e.OriginalSource is ControlAdorner controlDecorator)) return;
            if (!(controlDecorator.AdornedElement is FrameworkElement element)) return;

            // 实现控件在画布中拖动的功能
            var mouseOnControl = Mouse.GetPosition(element);
            var vector = mouseOnControl - controlDecorator.MousePoint;


            var top = Canvas.GetTop(element);
            var left = Canvas.GetLeft(element);
            var x = left + vector.X;
            var y = top + vector.Y;

            x = Math.Max(0, Math.Min(x, DesignPanel.Width - element.ActualWidth));
            y = Math.Max(0, Math.Min(y, DesignPanel.Height - element.ActualHeight));

            Canvas.SetTop(element, y);
            Canvas.SetLeft(element, x);

            MoveLineAndText(element);
        }

        public void RemoveChildren()
        {
            if (SelectedDecorators?.Any() == true)
            {
                foreach (var element in SelectedDecorators.Select(x => x.AdornedElement))
                {
                    DesignPanel.Children.Remove(element);
                }

                SelectedDecorators = null;
            }

            if (SelectedDecorator != null)
            {
                DesignPanel.Children.Remove(SelectedDecorator.AdornedElement);
                SelectedDecorator = null;
            }
        }

        public void Save()
        {
            if (string.IsNullOrWhiteSpace(FileName))
            {
                var dialog = new SaveFileDialog();
                dialog.Filter = "(*.ed)|*.ed";
                if (dialog.ShowDialog() == true)
                {
                    SaveChild(dialog.FileName);
                    FileName = dialog.FileName;
                }
            }
            else
            {
                SaveChild(FileName);
            }
        }

        public void SaveAs()
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "(*.ed)|*.ed";
            if (dialog.ShowDialog() == true)
            {
                SaveChild(dialog.FileName);
                FileName = dialog.FileName;
            }
        }

        private void SaveChild(string fileName)
        {
            DesignPanel.Children.Remove(HLine);
            DesignPanel.Children.Remove(VLine);
            DesignPanel.Children.Remove(TopText);
            DesignPanel.Children.Remove(LeftText);

            File.WriteAllText(fileName, XamlWriter.Save(DesignPanel));

            DesignPanel.Children.Add(HLine);
            DesignPanel.Children.Add(VLine);
            DesignPanel.Children.Add(TopText);
            DesignPanel.Children.Add(LeftText);
        }


        private string FileName { get; set; }

        public void Load()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "(*.ed)|*.ed";
            if (dialog.ShowDialog() == true)
            {
                var canvas = XamlReader.Load(dialog.OpenFile()) as Canvas;
                if (canvas == null) return;
                SelectedDecorator = null;
                DesignPanel.Children.Clear();

                DesignPanel.Children.Add(HLine);
                DesignPanel.Children.Add(VLine);
                DesignPanel.Children.Add(TopText);
                DesignPanel.Children.Add(LeftText);

                while (canvas.Children.Count > 0)
                {
                    var child = canvas.Children[0];
                    canvas.Children.Remove(child);
                    AddChild(child as FrameworkElement);
                }

                //foreach (UIElement child in canvas.Children)
                //{
                //    AddChild(child);
                //}
                FileName = dialog.FileName;
            }
        }

        private Point ClickPoint { get; set; }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            ClickPoint = e.GetPosition(DesignPanel);
            Canvas.SetTop(SelectedBound, e.GetPosition(this).Y);
            Canvas.SetLeft(SelectedBound, e.GetPosition(this).X);
        }

        private bool DoSelectMultiple { get; set; }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            //Debug.WriteLine($"{e.Source.GetType().Namespace} {e.OriginalSource.GetType().Name}");
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.LeftButton == MouseButtonState.Pressed)
            {
                DoSelectMultiple = true;
                SelectedBound.Visibility = Visibility.Visible;

                var p1 = e.GetPosition(DesignPanel);

                SelectedBound.Width = Math.Abs(ClickPoint.X - p1.X);
                SelectedBound.Height = Math.Abs(ClickPoint.Y - p1.Y);

                if (p1.X < ClickPoint.X)
                {
                    Canvas.SetLeft(SelectedBound, ClickPoint.X - Math.Abs(ClickPoint.X - p1.X));
                }

                if (p1.Y < ClickPoint.Y)
                {
                    Canvas.SetTop(SelectedBound, ClickPoint.Y - Math.Abs(p1.Y - ClickPoint.Y));
                }
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (DoSelectMultiple)
            {

                SelectedDecorator = null;


                var x = Canvas.GetLeft(SelectedBound);
                var y = Canvas.GetTop(SelectedBound);

                var decorators = GetDecorators(new Point(x, y), new Point(x + SelectedBound.Width, y + SelectedBound.Height)).ToList();

                SelectedDecorators = decorators;

                SelectedBound.Visibility = Visibility.Collapsed;
                SelectedBound.Width = 0;
                SelectedBound.Height = 0;
                DoSelectMultiple = false;
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e) => SelectedBound.Visibility = Visibility.Collapsed;

        /// <summary>
        /// 返回矩形内的所有ControlAdorner
        /// </summary>
        /// <param name="p1">矩形的起始点</param>
        /// <param name="p2">矩形的结束点</param>
        /// <returns></returns>
        private IEnumerable<ControlAdorner> GetDecorators(Point p1, Point p2)
        {
            foreach (FrameworkElement element in DesignPanel.Children)
            {
                if (Canvas.GetTop(element) >= p2.Y) continue;
                if (Canvas.GetLeft(element) >= p2.X) continue;
                if (Canvas.GetTop(element) + element.Height <= p1.Y) continue;
                if (Canvas.GetLeft(element) + element.Width <= p1.X) continue;

                var d = AdornerLayer.GetAdornerLayer(element)?.GetAdorners(element)?.OfType<ControlAdorner>().FirstOrDefault();
                if (d == null) continue;
                yield return d;
            }
        }


    }
}
