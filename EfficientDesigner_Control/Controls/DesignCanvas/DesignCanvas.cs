using EfficientDesigner_Control.Commands;
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
using System.Xml;
using EfficientDesigner_Control.ExtensionMethods;
using EfficientDesigner_Control.Interfaces;
using EfficientDesigner_Service;
using EfficientDesigner_Service.Models;
using EfficientDesigner_Service.ServiceImplements;
using EfficientDesigner_Service.Services;
using HandyControl.Controls;
using MessageBox = HandyControl.Controls.MessageBox;
using Path = System.IO.Path;
using ScrollViewer = System.Windows.Controls.ScrollViewer;
using Window = System.Windows.Window;

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

        public ICommand PreviewCommand
        {
            get => (ICommand)GetValue(PreviewCommandProperty);
            set => SetValue(PreviewCommandProperty, value);
        }

        public static readonly DependencyProperty PreviewCommandProperty =
            DependencyProperty.Register("PreviewCommand", typeof(ICommand), typeof(DesignCanvas), new PropertyMetadata(null));



        public ICommand PublishCommand
        {
            get => (ICommand)GetValue(PublishCommandProperty);
            set => SetValue(PublishCommandProperty, value);
        }

        public static readonly DependencyProperty PublishCommandProperty =
            DependencyProperty.Register("PublishCommand", typeof(ICommand), typeof(DesignCanvas), new PropertyMetadata(null));

        public ICommand GetLayoutsCommand
        {
            get { return (ICommand)GetValue(GetLayoutsCommandProperty); }
            set { SetValue(GetLayoutsCommandProperty, value); }
        }

        public static readonly DependencyProperty GetLayoutsCommandProperty =
            DependencyProperty.Register("GetLayoutsCommand", typeof(ICommand), typeof(DesignCanvas), new PropertyMetadata(null));



        public DesignCanvas()
        {
            AllowDrop = true;
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DesignCanvas), new FrameworkPropertyMetadata(typeof(DesignCanvas)));
            SaveCommand = new DelegateCommand(Save);
            LoadCommand = new DelegateCommand(Load);
            SaveAsCommand = new DelegateCommand(SaveAs);
            PreviewCommand = new DelegateCommand(Preview);
            PublishCommand = new DelegateCommand(Publish);
            GetLayoutsCommand = new DelegateCommand(GetLayouts);
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
            var top = element.GetCanvasTop();
            var left = element.GetCanvasLeft();

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

            var control = e.Data.GetData("control") as ControlDetail;
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

            SetBorder(child);

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

            // 因为 XamlWriter.Save 无法完整序列化在资源字典中canvas赋值的参数，所以在这里手动再赋值一下
            DesignPanel.Width = 1920;
            DesignPanel.Height = 1080;

            if (!AddedHandler)
            {
                AddedHandler = true;
                AddHandler(ControlAdorner.MoveEvent, new RoutedEventHandler(DesignPanel_ChildrenMove));
                AddHandler(ControlAdorner.DecoratorSizeChangedEvent, new RoutedEventHandler(DesignPanel_ChildSizeChanged));
                AddHandler(ControlAdorner.MouseDownEvent, new RoutedEventHandler(DesignPanel_ChildMouseDown));
            }

            ScrollContainer.PreviewMouseWheel += ScrollContainerOnMouseWheel;
        }


        private const double ZoomMax = 10;
        private const double ZoomMin = 0.1;
        private const double ZoomSpeed = 0.001;

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
                    if (zoom < ZoomMin) zoom = ZoomMin;
                    if (zoom > ZoomMax) zoom = ZoomMax;
                    ctl.DesignPanel.LayoutTransform = new ScaleTransform(zoom, zoom);
                }));

        private void ScrollContainerOnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Zoom += ZoomSpeed * e.Delta;

                if (Zoom < ZoomMin) Zoom = ZoomMin;
                if (Zoom > ZoomMax) Zoom = ZoomMax;

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

            var top = element.GetCanvasTop();
            var left = element.GetCanvasLeft();

            var x = left + vector.X;
            var y = top + vector.Y;

            x = Math.Max(0, Math.Min(x, DesignPanel.Width - element.ActualWidth));
            y = Math.Max(0, Math.Min(y, DesignPanel.Height - element.ActualHeight));


            Canvas.SetTop(element, y > 0 ? Math.Floor(y) : Math.Ceiling(y));
            Canvas.SetLeft(element, x > 0 ? Math.Floor(x) : Math.Ceiling(x));

            MoveLineAndText(element);

            SetBorder(element);

            CheckBorderLine(element);

        }

        public void RemoveChildren()
        {
            if (SelectedDecorators?.Any() == true)
            {
                foreach (var element in SelectedDecorators.Select(x => x.AdornedElement))
                {
                    DesignPanel.Children.Remove(element);
                    RemoveBorder(element as FrameworkElement);
                }

                SelectedDecorators = null;
            }

            if (SelectedDecorator != null)
            {
                DesignPanel.Children.Remove(SelectedDecorator.AdornedElement);
                RemoveBorder(SelectedDecorator.AdornedElement as FrameworkElement);
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

        public void Publish()
        {
            var displayNameDialog = new PublishLayout();
            displayNameDialog.InputText = FileName;

            var dialog = Dialog.Show(displayNameDialog);
            displayNameDialog.Yes += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(displayNameDialog.InputText)) return;

                var reader = SaveChild();
                var layout = new Layout
                {
                    CreateTime = DateTime.Now,
                    DisplayName = displayNameDialog.InputText,
                    File = reader.ReadToEnd(),
                    LayoutId = Guid.NewGuid()
                };
                ServiceFactory.GetLayoutService().UpdateLayout(layout);
                dialog.Close();
            };
            displayNameDialog.No += (s, e) => dialog.Close();
        }


        private void GetLayouts()
        {
            var us = new LayoutList();
            var dialog = Dialog.Show(us);
            us.closeButton.Click += (s, e) =>
            {
                dialog.Close();
            };
            us.LoadLayout += (s, e) =>
            {
                if (!(e.OriginalSource is Layout layout)) return;
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(layout.File));
                LoadChild(stream, layout.DisplayName);
                dialog.Close();
            };
        }


        /// <summary>
        /// 将拖拽到 DesignPanel 中的子控件保存到指定文件中
        /// </summary>
        /// <param name="fileName"></param>
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

        /// <summary>
        /// 将拖拽到 DesignPanel 中的子控件保存到 XmlReader 中
        /// </summary>
        /// <returns></returns>
        private StringReader SaveChild()
        {
            DesignPanel.Children.Remove(HLine);
            DesignPanel.Children.Remove(VLine);
            DesignPanel.Children.Remove(TopText);
            DesignPanel.Children.Remove(LeftText);

            var reader = new StringReader(XamlWriter.Save(DesignPanel));

            DesignPanel.Children.Add(HLine);
            DesignPanel.Children.Add(VLine);
            DesignPanel.Children.Add(TopText);
            DesignPanel.Children.Add(LeftText);

            //return XmlReader.Create(reader);
            return reader;
        }

        private string FileName { get; set; }

        public void Load()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "(*.ed)|*.ed";
            if (dialog.ShowDialog() == true)
            {
                LoadChild(dialog.OpenFile(), dialog.FileName);

                //var canvas = XamlReader.Load(dialog.OpenFile()) as Canvas;
                //if (canvas == null) return;
                //SelectedDecorator = null;
                //DesignPanel.Children.Clear();

                //DesignPanel.Children.Add(HLine);
                //DesignPanel.Children.Add(VLine);
                //DesignPanel.Children.Add(TopText);
                //DesignPanel.Children.Add(LeftText);

                //while (canvas.Children.Count > 0)
                //{
                //    var child = canvas.Children[0];
                //    canvas.Children.Remove(child);
                //    AddChild(child as FrameworkElement);
                //}

                //FileName = dialog.FileName;
            }
        }

        private void LoadChild(Stream stream, string fileName)
        {
            using (stream)
            {
                var canvas = XamlReader.Load(stream) as Canvas;
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

                FileName = fileName;
            }
        }


        private void Preview()
        {
            var reader = SaveChild();

            if (!(XamlReader.Load(XmlReader.Create(reader)) is Canvas canvas)) return;

            var canvas2 = new Canvas { Height = DesignPanel.Height, Width = DesignPanel.Width };

            var window = new Window
            {
                Title = "预览模式",
                Content = new ScrollViewer
                {
                    Content = canvas2,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Visible,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
                },
            };

            while (canvas.Children.Count > 0)
            {
                var child = canvas.Children[0];
                canvas.Children.Remove(child);

                if (child is IHasDisplayMode hasDisplayMode)
                {
                    hasDisplayMode.SetDisplayMode(ControlDisplayMode.Runtime);
                }

                canvas2.Children.Add(child);
            }

            window.ShowDialog();
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

                //var x = Canvas.GetLeft(SelectedBound);
                //var y = Canvas.GetTop(SelectedBound);


                var x = SelectedBound.GetCanvasLeft();
                var y = SelectedBound.GetCanvasTop();

                var decorators = GetDecorators(new Point(x, y), new Point(x + SelectedBound.Width, y + SelectedBound.Height)).ToList();

                SelectedDecorators = decorators;

                SelectedBound.Visibility = Visibility.Collapsed;
                SelectedBound.Width = 0;
                SelectedBound.Height = 0;
                DoSelectMultiple = false;
            }

            RemoveBorderLine();
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
                //if (Canvas.GetTop(element) >= p2.Y) continue;
                //if (Canvas.GetLeft(element) >= p2.X) continue;
                //if (Canvas.GetTop(element) + element.Height <= p1.Y) continue;
                //if (Canvas.GetLeft(element) + element.Width <= p1.X) continue;

                if (element.GetCanvasTop() >= p2.Y) continue;
                if (element.GetCanvasLeft() >= p2.X) continue;
                if (element.GetCanvasTop() + element.Height <= p1.Y) continue;
                if (element.GetCanvasLeft() + element.Width <= p1.X) continue;

                var d = AdornerLayer.GetAdornerLayer(element)?.GetAdorners(element)?.OfType<ControlAdorner>().FirstOrDefault();
                if (d == null) continue;
                yield return d;
            }
        }


        #region 拖拽控件显示对齐线
        /**
         * 1. 四个字典保存控件和对应的上下左右坐标。变量名：TopBorders,BottomBorders,LeftBorders,RightBorders
         * 2. 拖动的时候判断
         *    1) 上下左右是否与上下左右相等，相等就显示红线。红线连接两个控件的最远点，红线只需起到对齐作用，不需要与 border 一样。
         *    2) 上下左右是否和下上右左相等，相等就显示红线。红线在两个控件的交界处，红线必须跟 border 一样。
         * 3. 最多只有八条红线，可以使用八个变量维护八条红线，拖动的实现添加红线，鼠标松开的时候移除红线。
         * 4. 两个方法拖动时和松开鼠标时调用，CheckBorderLine,RemoveBorderLine
         */

        private Dictionary<FrameworkElement, double> TopBorders { get; } = new Dictionary<FrameworkElement, double>();
        private Dictionary<FrameworkElement, double> BottomBorders { get; } = new Dictionary<FrameworkElement, double>();
        private Dictionary<FrameworkElement, double> LeftBorders { get; } = new Dictionary<FrameworkElement, double>();
        private Dictionary<FrameworkElement, double> RightBorders { get; } = new Dictionary<FrameworkElement, double>();

        public Line TopBorderLine { get; set; }
        public Line BottomBorderLine { get; set; }
        public Line RightBorderLine { get; set; }
        public Line LeftBorderLine { get; set; }
        public Line TopCrossBorderLine { get; set; }
        public Line BottomCrossBorderLine { get; set; }
        public Line RightCrossBorderLine { get; set; }
        public Line LeftCrossBorderLine { get; set; }

        /// <summary>
        /// 保存控件在画布中四个边的值
        /// </summary>
        /// <param name="element"></param>
        private void SetBorder(FrameworkElement element)
        {
            SetTopBorder(element);
            SetBottomBorder(element);
            SetLeftBorder(element);
            SetRightBorder(element);
        }

        /// <summary>
        /// 移除控件在画布中四个边的值
        /// </summary>
        /// <param name="element"></param>
        private void RemoveBorder(FrameworkElement element)
        {
            RemoveTopBorder(element);
            RemoveBottomBorder(element);
            RemoveLeftBorder(element);
            RemoveRightBorder(element);
        }


        private void SetTopBorder(FrameworkElement element)
        {
            var top = Canvas.GetTop(element);
            if (double.IsNaN(top)) return;
            if (TopBorders.ContainsKey(element))
            {
                TopBorders[element] = Canvas.GetTop(element);
            }
            else
            {
                TopBorders.Add(element, top);
            }
        }

        private void RemoveTopBorder(FrameworkElement element)
        {
            if (element == null) return;
            TopBorders.Remove(element);
        }


        private void SetBottomBorder(FrameworkElement element)
        {
            var bottom = Canvas.GetTop(element) + element.Height;
            if (double.IsNaN(bottom)) return;
            if (BottomBorders.ContainsKey(element))
            {
                BottomBorders[element] = bottom;
            }
            else
            {
                BottomBorders.Add(element, bottom);
            }
        }

        private void RemoveBottomBorder(FrameworkElement element)
        {
            if (element == null) return;
            BottomBorders.Remove(element);
        }

        private void SetLeftBorder(FrameworkElement element)
        {
            var left = Canvas.GetLeft(element);
            if (double.IsNaN(left)) return;
            if (LeftBorders.ContainsKey(element))
            {
                LeftBorders[element] = left;
            }
            else
            {
                LeftBorders.Add(element, left);
            }
        }

        private void RemoveLeftBorder(FrameworkElement element)
        {
            if (element == null) return;
            LeftBorders.Remove(element);
        }

        private void SetRightBorder(FrameworkElement element)
        {
            var right = Canvas.GetLeft(element) + element.Width;
            if (double.IsNaN(right)) return;
            if (RightBorders.ContainsKey(element))
            {
                RightBorders[element] = right;
            }
            else
            {
                RightBorders.Add(element, right);
            }
        }

        private void RemoveRightBorder(FrameworkElement element)
        {
            if (element == null) return;
            RightBorders.Remove(element);
        }

        /// <summary>
        /// 检查画布中有没有对齐的控件，有就显示一条红线
        /// </summary>
        /// <param name="element"></param>
        private void CheckBorderLine(FrameworkElement element)
        {
            double top = Canvas.GetTop(element), left = Canvas.GetLeft(element);
            if (double.IsNaN(top) || double.IsNaN(left)) return;

            CheckTopBorderLine(element, top, left, left + element.Width);
            CheckBottomBorderLine(element, top + element.Height, left, left + element.Width);
            CheckLeftBorderLine(element, left, top, top + element.Height);
            CheckRightBorderLine(element, left + element.Width, top, top + element.Height);
        }

        /// <summary>
        /// 移除用于对齐控件的红线
        /// </summary>
        private void RemoveBorderLine()
        {
            if (TopBorderLine != null)
            {
                DesignPanel.Children.Remove(TopBorderLine);
                TopBorderLine = null;
            }

            if (BottomBorderLine != null)
            {
                DesignPanel.Children.Remove(BottomBorderLine);
                BottomBorderLine = null;
            }

            if (LeftBorderLine != null)
            {
                DesignPanel.Children.Remove(LeftBorderLine);
                LeftBorderLine = null;
            }

            if (RightBorderLine != null)
            {
                DesignPanel.Children.Remove(RightBorderLine);
                RightBorderLine = null;
            }
        }

        private void CheckTopBorderLine(FrameworkElement element, double top, double left, double right)
        {
            var elements = TopBorders.Where(x => x.Key != element && Math.Abs(x.Value - top) < 1).ToList();

            if (elements.Any())
            {
                var minLeft = elements.Min(x => Canvas.GetLeft(x.Key));
                var maxRight = elements.Max(x => Canvas.GetLeft(x.Key) + x.Key.Width);

                if (left < minLeft)
                {
                    minLeft = left;
                }

                if (right > maxRight)
                {
                    maxRight = right;
                }

                if (TopBorderLine == null)
                {
                    TopBorderLine = new Line
                    {
                        StrokeThickness = 1,
                        Stroke = Brushes.Red,
                        X1 = minLeft,
                        X2 = maxRight,
                        Y1 = top,
                        Y2 = top
                    };
                    DesignPanel.Children.Add(TopBorderLine);
                }
                else
                {
                    TopBorderLine.X1 = minLeft;
                    TopBorderLine.X2 = maxRight;
                    TopBorderLine.Y1 = top;
                    TopBorderLine.Y2 = top;
                    TopBorderLine.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (TopBorderLine != null)
                {
                    TopBorderLine.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void CheckBottomBorderLine(FrameworkElement element, double bottom, double left, double right)
        {
            var elements = BottomBorders.Where(x => x.Key != element && Math.Abs(x.Value - bottom) < 1).ToList();

            if (elements.Any())
            {
                var minLeft = elements.Min(x => Canvas.GetLeft(x.Key));
                var maxRight = elements.Max(x => Canvas.GetLeft(x.Key) + x.Key.Width);

                if (left < minLeft)
                {
                    minLeft = left;
                }

                if (right > maxRight)
                {
                    maxRight = right;
                }

                if (BottomBorderLine == null)
                {
                    BottomBorderLine = new Line
                    {
                        StrokeThickness = 1,
                        Stroke = Brushes.Red,
                        X1 = minLeft,
                        X2 = maxRight,
                        Y1 = bottom,
                        Y2 = bottom
                    };
                    DesignPanel.Children.Add(BottomBorderLine);
                }
                else
                {
                    BottomBorderLine.X1 = minLeft;
                    BottomBorderLine.X2 = maxRight;
                    BottomBorderLine.Y1 = bottom;
                    BottomBorderLine.Y2 = bottom;
                    BottomBorderLine.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (BottomBorderLine != null)
                {
                    BottomBorderLine.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void CheckLeftBorderLine(FrameworkElement element, double left, double top, double bottom)
        {
            var elements = LeftBorders.Where(x => x.Key != element && Math.Abs(x.Value - left) < 1).ToList();

            if (elements.Any())
            {
                var minTop = elements.Min(x => Canvas.GetTop(x.Key));
                var maxBottom = elements.Max(x => Canvas.GetTop(x.Key) + x.Key.Height);

                if (top < minTop)
                {
                    minTop = top;
                }

                if (bottom > maxBottom)
                {
                    maxBottom = bottom;
                }

                if (LeftBorderLine == null)
                {
                    LeftBorderLine = new Line
                    {
                        StrokeThickness = 1,
                        Stroke = Brushes.Red,
                        X1 = left,
                        X2 = left,
                        Y1 = minTop,
                        Y2 = maxBottom
                    };
                    DesignPanel.Children.Add(LeftBorderLine);
                }
                else
                {
                    LeftBorderLine.X1 = left;
                    LeftBorderLine.X2 = left;
                    LeftBorderLine.Y1 = minTop;
                    LeftBorderLine.Y2 = maxBottom;
                    LeftBorderLine.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (LeftBorderLine != null)
                {
                    LeftBorderLine.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void CheckRightBorderLine(FrameworkElement element, double right, double top, double bottom)
        {
            var elements = RightBorders.Where(x => x.Key != element && Math.Abs(x.Value - right) < 1).ToList();

            if (elements.Any())
            {
                var minTop = elements.Min(x => Canvas.GetTop(x.Key));
                var maxBottom = elements.Max(x => Canvas.GetTop(x.Key) + x.Key.Height);

                if (top < minTop)
                {
                    minTop = top;
                }

                if (bottom > maxBottom)
                {
                    maxBottom = bottom;
                }

                if (RightBorderLine == null)
                {
                    RightBorderLine = new Line
                    {
                        StrokeThickness = 1,
                        Stroke = Brushes.Red,
                        X1 = right,
                        X2 = right,
                        Y1 = minTop,
                        Y2 = maxBottom
                    };
                    DesignPanel.Children.Add(RightBorderLine);
                }
                else
                {
                    RightBorderLine.X1 = right;
                    RightBorderLine.X2 = right;
                    RightBorderLine.Y1 = minTop;
                    RightBorderLine.Y2 = maxBottom;
                    RightBorderLine.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (RightBorderLine != null)
                {
                    RightBorderLine.Visibility = Visibility.Collapsed;
                }
            }
        }
        #endregion
    }
}
