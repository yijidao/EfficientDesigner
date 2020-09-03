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


        public DesignCanvas()
        {
            AllowDrop = true;
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DesignCanvas), new FrameworkPropertyMetadata(typeof(DesignCanvas)));
            SaveCommand = new DelegateCommand(Save);
            LoadCommand = new DelegateCommand(Load);
        }

        public bool AddedHandler { get; set; }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (!AddedHandler)
            {
                AddedHandler = true;
                var layout = AdornerLayer.GetAdornerLayer(this);
                layout.AddHandler(ControlAdorner.SelectedEvent, new RoutedEventHandler(OnDesignPanelSelected));
                layout.AddHandler(ControlAdorner.MoveEvent, new RoutedEventHandler(DesignPanel_ChilrenMove));
            }
        }

        protected override void OnDrop(DragEventArgs e)
        {
            if (e.Data.GetDataPresent("control") && DesignPanel != null)
            {
                var control = e.Data.GetData("control") as IControl;
                var element = control?.GetElement();

                var p = e.GetPosition(DesignPanel);
                Canvas.SetTop(element, p.Y);
                Canvas.SetLeft(element, p.X);

                AddChild(element);
                e.Effects = DragDropEffects.Copy;
            }
            e.Handled = true;
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

        private ControlAdorner _SelectedAdorner;

        private ControlAdorner SelectedAdorner
        {
            get { return _SelectedAdorner; }
            set
            {
                _SelectedAdorner?.InvalidateVisual();
                value?.InvalidateVisual();
                _SelectedAdorner = value;
                SelectedElement = value?.AdornedElement;
            }
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


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            HLine = GetTemplateChild(HLineName) as Line;
            VLine = GetTemplateChild(VLineName) as Line;
            TopText = GetTemplateChild(TopTextName) as TextBlock;
            LeftText = GetTemplateChild(LeftTextName) as TextBlock;
            DesignPanel = GetTemplateChild(CanvasName) as Canvas;
        }


        private void OnDesignPanelSelected(object sender, RoutedEventArgs e)
        {
            if (SelectedAdorner != null)
                SelectedAdorner.IsSelected = false;

            if (e.Source is ControlAdorner newElement)
            {
                newElement.IsSelected = true;
                SelectedAdorner = newElement;
            }
        }

        private void DesignPanel_ChilrenMove(object sender, RoutedEventArgs e)
        {
            var ps = (Point[])(e.OriginalSource);

            if (double.IsNaN(ps[0].X) || double.IsNaN(ps[0].Y) || double.IsNaN(ps[1].X) || double.IsNaN(ps[1].Y)) return;

            VLine.X1 = ps[1].X;
            VLine.X2 = ps[1].X;
            VLine.Y2 = ps[1].Y;

            HLine.Y1 = ps[1].Y;
            HLine.X2 = ps[1].X;
            HLine.Y2 = ps[1].Y;

            Canvas.SetTop(TopText, ps[0].Y / 2);
            Canvas.SetLeft(TopText, ps[1].X - 10);

            Canvas.SetTop(LeftText, ps[1].Y - 10);
            Canvas.SetLeft(LeftText, ps[0].X / 2);

            TopText.Text = ps[0].Y.ToString("0.##");
            LeftText.Text = ps[0].X.ToString("0.##");

            if (ps[0].Y < 30)
            {
                TopText.Visibility = Visibility.Collapsed;
            }
            else
            {
                TopText.Visibility = Visibility.Visible;
            }

            if (ps[0].X < 30)
            {
                LeftText.Visibility = Visibility.Collapsed;
            }
            else
            {
                LeftText.Visibility = Visibility.Visible;
            }
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
    }
}
