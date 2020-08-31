using EfficientDesigner_Control.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Shapes;

namespace EfficientDesigner_Control.Controls
{
    public class DesignCanvas : Control
    {

        private const string CanvasName = "CanvasPanel";
        private const string VLineName = "PART_VerticalLine";
        private const string HLineName = "PART_HorizontalLine";
        private const string TopTextName = "PART_TopText";
        private const string LeftTextName = "PART_LeftText";

        public DesignCanvas()
        {
            AllowDrop = true;
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DesignCanvas), new FrameworkPropertyMetadata(typeof(DesignCanvas)));

            DesignPanelSelectedHandler = new RoutedEventHandler(OnDesignPanelSelected);
            DesignPanelChildrenMoveHandler = new RoutedEventHandler(DesignPanel_ChilrenMove);
        }

        //private Canvas DesignPanel { get; set; }

        private Canvas _DesignPanel;

        public Canvas DesignPanel
        {
            get { return _DesignPanel; }
            set
            {

                if (_DesignPanel != null)
                {
                    _DesignPanel.Drop -= OnDesignPanelDrop;
                    _DesignPanel.RemoveHandler(ControlAdorner.SelectedEvent, DesignPanelSelectedHandler);
                    _DesignPanel.RemoveHandler(ControlAdorner.MoveEvent, DesignPanelChildrenMoveHandler);
                }

                _DesignPanel = value;

                if (_DesignPanel != null)
                {
                    _DesignPanel.Drop += OnDesignPanelDrop;
                    _DesignPanel.AddHandler(ControlAdorner.SelectedEvent, DesignPanelSelectedHandler);
                    _DesignPanel.AddHandler(ControlAdorner.MoveEvent, DesignPanelChildrenMoveHandler);
                    _DesignPanel.Children.Add(HLine);
                    _DesignPanel.Children.Add(VLine);
                    _DesignPanel.Children.Add(TopText);
                    _DesignPanel.Children.Add(LeftText);
                }
            }
        }


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

                //ControlPropertyGrid.SelectedObject = value;
                //PropertyPanel1.SelectedElement = value?.AdornedElement as FrameworkElement;
            }
        }

        private RoutedEventHandler DesignPanelSelectedHandler { get; set; }

        private RoutedEventHandler DesignPanelChildrenMoveHandler { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            HLine = GetTemplateChild(HLineName) as Line;
            VLine = GetTemplateChild(VLineName) as Line;
            TopText = GetTemplateChild(TopTextName) as TextBlock;
            LeftText = GetTemplateChild(LeftTextName) as TextBlock;

            DesignPanel = GetTemplateChild(CanvasName) as Canvas;
        }

        private void OnDesignPanelDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("control"))
            {
                var control = e.Data.GetData("control") as IControl;
                var element = control?.GetElement();

                if (element != null)
                {

                    var p = e.GetPosition(DesignPanel);
                    Canvas.SetTop(element, p.Y);
                    Canvas.SetLeft(element, p.X);

                    DesignPanel.Children.Add(element);

                    var layout = AdornerLayer.GetAdornerLayer(element);
                    if (layout != null)
                    {
                        layout.Add(new ControlAdorner(element, DesignPanel));
                    }

                    e.Effects = DragDropEffects.Copy;
                }
            }
            e.Handled = true;
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
            DesignPanel.Children.Remove(SelectedAdorner.AdornedElement);
            SelectedAdorner = null;
        }

        public void Save()
        {
            var dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, XamlWriter.Save(DesignPanel));
            }
        }

        public void Load()
        {
            var dialog =  new OpenFileDialog();
            dialog.ShowDialog();
        }
    }
}
