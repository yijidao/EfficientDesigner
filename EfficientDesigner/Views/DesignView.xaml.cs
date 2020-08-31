using CommonServiceLocator;
using EfficientDesigner.Models;
using EfficientDesigner_Control.Controls;
using EfficientDesigner_Control.Interfaces;
using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EfficientDesigner.Views
{
    /// <summary>
    /// DesignView.xaml 的交互逻辑
    /// </summary>
    public partial class DesignView : UserControl
    {
        public DesignView()
        {
            InitializeComponent();

        }

        private void TreeView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && ToolBoxView.SelectedItem is IControl controlItem)
            {
                var data = new DataObject();
                data.SetData("control", controlItem);
                DragDrop.DoDragDrop(sender as DependencyObject, data, DragDropEffects.Copy);
            }
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
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



        private void DesignPanel_Selected(object sender, RoutedEventArgs e)
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

            VerticalLine.X1 = ps[1].X;
            VerticalLine.X2 = ps[1].X;
            VerticalLine.Y2 = ps[1].Y;

            HorizontalLine.Y1 = ps[1].Y;
            HorizontalLine.X2 = ps[1].X;
            HorizontalLine.Y2 = ps[1].Y;

            Canvas.SetTop(CanvasTopText, ps[0].Y / 2);
            Canvas.SetLeft(CanvasTopText, ps[1].X - 10);

            Canvas.SetTop(CanvasLeftText, ps[1].Y - 10);
            Canvas.SetLeft(CanvasLeftText, ps[0].X / 2);

            CanvasTopText.Text = ps[0].Y.ToString("0.##");
            CanvasLeftText.Text = ps[0].X.ToString("0.##");

            if (ps[0].Y < 30)
            {
                CanvasTopText.Visibility = Visibility.Collapsed;
            }
            else
            {
                CanvasTopText.Visibility = Visibility.Visible;
            }

            if (ps[0].X < 30)
            {
                CanvasLeftText.Visibility = Visibility.Collapsed;
            }
            else
            {
                CanvasLeftText.Visibility = Visibility.Visible;
            }

        }

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
                PropertyPanel1.SelectedElement = value?.AdornedElement as FrameworkElement;
            }
        }



        public Line VerticalLine { get; set; } = new Line { Stroke = Brushes.Gray, StrokeThickness = 1, Y1 = 0 };
        public Line HorizontalLine { get; set; } = new Line { Stroke = Brushes.Gray, StrokeThickness = 1, X1 = 0 };

        public TextBlock CanvasTopText { get; set; } = new TextBlock { Background = Brushes.White };
        public TextBlock CanvasLeftText { get; set; } = new TextBlock { Background = Brushes.White };


        public bool PanelLoaded { get; set; }

        private void DesignPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (!PanelLoaded)
            {
                var layout = AdornerLayer.GetAdornerLayer(DesignPanel);
                layout.AddHandler(ControlAdorner.SelectedEvent, new RoutedEventHandler(DesignPanel_Selected));
                layout.AddHandler(ControlAdorner.MoveEvent, new RoutedEventHandler(DesignPanel_ChilrenMove));

                DesignPanel.Children.Add(VerticalLine);
                DesignPanel.Children.Add(HorizontalLine);
                DesignPanel.Children.Add(CanvasLeftText);
                DesignPanel.Children.Add(CanvasTopText);

                PanelLoaded = true;
            }
            //Keyboard.Focus(DesignPanel);

        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Back || e.Key == Key.Delete) && SelectedAdorner != null )
            {
                DesignPanel.Children.Remove(SelectedAdorner.AdornedElement);
                SelectedAdorner = null;
            }
        }

    }

}
