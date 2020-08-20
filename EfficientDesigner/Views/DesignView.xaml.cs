using EfficientDesigner.Controls;
using EfficientDesigner.Interfaces;
using EfficientDesigner.Models;
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
                        layout.Add(new ControlAdorner(element));
                    }

                    e.Effects = DragDropEffects.Copy;
                }
            }
            e.Handled = true;
        }

        public bool IsDrag { get; set; }

        //public FrameworkElement SelectedElement { get; set; }

        public Point ClickPosition { get; set; }

        private void DesignPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //var p = e.GetPosition(DesignPanel);
            //var hitResult = VisualTreeHelper.HitTest(DesignPanel, p);
            //if (hitResult.VisualHit != null)
            //{
            //    if (DesignPanel.Children.Contains(hitResult.VisualHit as FrameworkElement))
            //    {
            //        SelectedElement = hitResult.VisualHit as FrameworkElement;
            //        ClickPosition = e.GetPosition(SelectedElement);
            //        IsDrag = true;
            //    }
            //}
            //else
            //{
            //    SelectedElement = null;
            //}
        }

        private void DesignPanel_MouseMove(object sender, MouseEventArgs e)
        {
            //if (IsDrag && SelectedElement != null)
            //{
            //    var p = e.GetPosition(DesignPanel);
            //    var transform = SelectedElement.RenderTransform as TranslateTransform;
            //    if (transform == null)
            //    {
            //        transform = new TranslateTransform();
            //        SelectedElement.RenderTransform = transform;
            //    }
            //    transform.X = p.X - ClickPosition.X;
            //    transform.Y = p.Y - ClickPosition.Y;
            //}
        }

        private void DesignPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsDrag = false;
        }

        private void DesignPanel_Selected(object sender, RoutedEventArgs e)
        {
            if (SelectedElement is ControlAdorner oldElement)
            {
                oldElement.IsSelected = false;
            }

            if (e.Source is ControlAdorner  newElement)
            {
                newElement.IsSelected = true;
                SelectedElement = newElement;
            }
        }

        private FrameworkElement _SelectedElement;

        public FrameworkElement SelectedElement
        {
            get { return _SelectedElement; }
            set
            {
                _SelectedElement?.InvalidateVisual();
                value?.InvalidateVisual();
                _SelectedElement = value;
                //ControlPropertyGrid.SelectedObject = value;
            }
        }

        private void DesignPanel_Loaded(object sender, RoutedEventArgs e)
        {
            var layout = AdornerLayer.GetAdornerLayer(DesignPanel);
            layout.AddHandler(ControlAdorner.SelectedEvent, new RoutedEventHandler(DesignPanel_Selected));
        }
    }

}
