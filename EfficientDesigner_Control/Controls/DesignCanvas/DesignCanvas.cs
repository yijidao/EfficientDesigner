﻿using EfficientDesigner_Control.Commands;
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


        public ICommand SaveCommand
        {
            get { return (ICommand)GetValue(SaveCommandProperty); }
            set { SetValue(SaveCommandProperty, value); }
        }

        /// <summary>
        /// 保存画布内容中的子元素
        /// </summary>
        public static readonly DependencyProperty SaveCommandProperty =
            DependencyProperty.Register("SaveCommand", typeof(ICommand), typeof(DesignCanvas), new PropertyMetadata(null));


        public ICommand LoadCommand
        {
            get { return (ICommand)GetValue(LoadCommandProperty); }
            set { SetValue(LoadCommandProperty, value); }
        }

        /// <summary>
        /// 加载子元素到画布中
        /// </summary>
        public static readonly DependencyProperty LoadCommandProperty =
            DependencyProperty.Register("LoadCommand", typeof(ICommand), typeof(DesignCanvas), new PropertyMetadata(null));

        public ICommand SaveAsCommand
        {
            get { return (ICommand)GetValue(SaveAsCommandProperty); }
            set { SetValue(SaveAsCommandProperty, value); }
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

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (!AddedHandler)
            {
                AddedHandler = true;
                var layout = AdornerLayer.GetAdornerLayer(this);
                if (layout == null) return;
                layout.AddHandler(ControlAdorner.SelectedEvent, new RoutedEventHandler(OnDesignPanelSelected));
                layout.AddHandler(ControlAdorner.MoveEvent, new RoutedEventHandler(DesignPanel_ChildrenMove));
            }
        }

        protected override void OnDrop(DragEventArgs e)
        {
            if (e.Data.GetDataPresent("control") && DesignPanel != null)
            {
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
        }

        /// <summary>
        /// 增加子元素到画布中
        /// </summary>
        /// <param name="child"></param>
        private void AddChild(UIElement child)
        {
            if (child == null) return;

            DesignPanel.Children.Add(child);

            var layout = AdornerLayer.GetAdornerLayer(child);
            if (layout != null)
            {
                layout.Add(new ControlAdorner(child, DesignPanel));
            }
        }

        public Canvas DesignPanel { get; set; }
        private Line HLine { get; set; }
        private Line VLine { get; set; }
        private TextBlock TopText { get; set; }
        private TextBlock LeftText { get; set; }

        private ControlAdorner _selectedAdorner;
        private IEnumerable<ControlAdorner> _selectedAdorners;

        private ControlAdorner SelectedAdorner
        {
            get => _selectedAdorner;
            set
            {
                _selectedAdorner?.InvalidateVisual();
                value?.InvalidateVisual();
                _selectedAdorner = value;
                SelectedElement = value?.AdornedElement;
            }
        }

        private IEnumerable<ControlAdorner> SelectedAdorners
        {
            get => _selectedAdorners;
            set
            {
                if (_selectedAdorners != null)
                {
                    foreach (var adorner in _selectedAdorners)
                    {
                        adorner?.InvalidateVisual();
                    }
                }

                if (value != null)
                {
                    foreach (var adorner in value)
                    {
                        adorner?.InvalidateVisual();
                    }
                }

                _selectedAdorners = value;
                SelectedElements = value?.Select(x => x.AdornedElement);
            }
        }

        public static readonly DependencyProperty SelectedElementsProperty = DependencyProperty.Register(
            "SelectedElements", typeof(IEnumerable<UIElement>), typeof(DesignCanvas), new PropertyMetadata(default(IEnumerable<UIElement>)));

        public IEnumerable<UIElement> SelectedElements
        {
            get { return (IEnumerable<UIElement>)GetValue(SelectedElementsProperty); }
            set { SetValue(SelectedElementsProperty, value); }
        }

        public UIElement SelectedElement
        {
            get { return (UIElement)GetValue(SelectedElementProperty); }
            set { SetValue(SelectedElementProperty, value); }
        }

        /// <summary>
        /// 选中的元素，提供给属性面板使用。
        /// </summary>
        public static readonly DependencyProperty SelectedElementProperty =
            DependencyProperty.Register("SelectedElement", typeof(UIElement), typeof(DesignCanvas), new PropertyMetadata(null));

        public Rectangle SelectedBound { get; private set; }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            HLine = GetTemplateChild(HLineName) as Line;
            VLine = GetTemplateChild(VLineName) as Line;
            TopText = GetTemplateChild(TopTextName) as TextBlock;
            LeftText = GetTemplateChild(LeftTextName) as TextBlock;
            DesignPanel = GetTemplateChild(CanvasName) as Canvas;
            SelectedBound = GetTemplateChild(SelectedBoundName) as Rectangle;
        }


        private void OnDesignPanelSelected(object sender, RoutedEventArgs e)
        {
            if (SelectedAdorners != null)
            {
                foreach (var adorner in SelectedAdorners)
                {
                    adorner.IsSelected = false;
                }
                SelectedAdorners = null;
            }

            if (SelectedAdorner != null)
                SelectedAdorner.IsSelected = false;

            if (e.Source is ControlAdorner newElement)
            {
                newElement.IsSelected = true;
                SelectedAdorner = newElement;
            }
        }

        private void DesignPanel_ChildrenMove(object sender, RoutedEventArgs e)
        {
            var element = e.OriginalSource as FrameworkElement;
            if (element == null) return;

            var top = Canvas.GetTop(element);
            var left = Canvas.GetLeft(element);

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

        public void RemoveChild()
        {
            if (SelectedAdorner == null) return;
            DesignPanel.Children.Remove(SelectedAdorner.AdornedElement);
            SelectedAdorner = null;
        }

        public void Save()
        {
            if (String.IsNullOrWhiteSpace(FileName))
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
                SelectedAdorner = null;
                DesignPanel.Children.Clear();

                DesignPanel.Children.Add(HLine);
                DesignPanel.Children.Add(VLine);
                DesignPanel.Children.Add(TopText);
                DesignPanel.Children.Add(LeftText);

                while (canvas.Children.Count > 0)
                {
                    var child = canvas.Children[0];
                    canvas.Children.Remove(child);
                    AddChild(child);
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
                if (SelectedAdorner != null)
                {
                    SelectedAdorner.IsSelected = false;
                    SelectedAdorner = null;
                }

                var x = Canvas.GetLeft(SelectedBound);
                var y = Canvas.GetTop(SelectedBound);

                if (SelectedAdorners != null)
                {
                    foreach (var adorner in SelectedAdorners)
                    {
                        adorner.IsSelected = false;
                    }
                }

                var adorners = GetAdorners(new Point(x, y), new Point(x + SelectedBound.Width, y + SelectedBound.Height));

                if (adorners != null)
                {
                    foreach (var adorner in adorners)
                    {
                        adorner.IsSelected = true;
                    }
                }

                SelectedAdorners = adorners;

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
        private IEnumerable<ControlAdorner> GetAdorners(Point p1, Point p2)
        {
            foreach (FrameworkElement element in DesignPanel.Children)
            {
                if (Canvas.GetTop(element) >= p2.Y) continue;
                if (Canvas.GetLeft(element) >= p2.X) continue;
                if (Canvas.GetTop(element) + element.Height <= p1.Y) continue;
                if (Canvas.GetLeft(element) + element.Width <= p1.X) continue;

                var d = AdornerLayer.GetAdornerLayer(this)?.GetAdorners(element)?.OfType<ControlAdorner>().FirstOrDefault();
                if (d == null) continue;
                yield return d;
            }
        }
    }
}
