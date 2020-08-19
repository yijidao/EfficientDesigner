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

                    //var layer = AdornerLayer.GetAdornerLayer(element);
                    //var layer = new AdornerLayer();
                    //layer.Add(new ControlAdorner(element));

                    DesignPanel.Children.Add(element);
                    e.Effects = DragDropEffects.Copy;
                }
            }
            e.Handled = true;
        }

        public bool IsDrag { get; set; }

        public FrameworkElement SelectedElement { get; set; }

        public Point ClickPosition { get; set; }

        private void DesignPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var p = e.GetPosition(DesignPanel);
            var hitResult = VisualTreeHelper.HitTest(DesignPanel, p);
            if (hitResult.VisualHit != null)
            {
                if (DesignPanel.Children.Contains(hitResult.VisualHit as FrameworkElement))
                {
                    SelectedElement = hitResult.VisualHit as FrameworkElement;
                    ClickPosition = e.GetPosition(SelectedElement);
                    IsDrag = true;
                }
            }
            else
            {
                SelectedElement = null;
            }
        }

        private void DesignPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDrag && SelectedElement != null)
            {
                var p = e.GetPosition(DesignPanel);
                var transform = SelectedElement.RenderTransform as TranslateTransform;
                if (transform == null)
                {
                    transform = new TranslateTransform();
                    SelectedElement.RenderTransform = transform;
                }
                transform.X = p.X - ClickPosition.X;
                transform.Y = p.Y - ClickPosition.Y;
            }
        }

        private void DesignPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsDrag = false;
        }
    }

    public class ControlAdorner : Adorner
    {
        public ControlAdorner(UIElement adornedElement) : base(adornedElement)
        {
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            //base.OnRender(drawingContext);
            var size = this.AdornedElement.DesiredSize;
            var rect = new Rect(0, 0, size.Width, size.Height);
            drawingContext.DrawRectangle(Brushes.Transparent, BorderPen, rect);
            drawingContext.DrawRectangle(Brushes.White, BlockPen, new Rect(-2, -2, 2, 2));
            drawingContext.DrawRectangle(Brushes.White, BlockPen, new Rect(size.Width, size.Height, 2, 2));
            drawingContext.DrawRectangle(Brushes.White, BlockPen, new Rect(-2, size.Height, 2, 2));
            drawingContext.DrawRectangle(Brushes.White, BlockPen, new Rect(size.Width, -2, 2, 2));
        }

        public Pen BorderPen { get; set; } = new Pen(Brushes.Black, 0.5);

        public Pen BlockPen { get; set; } = new Pen(Brushes.SkyBlue, 1);
    }

}
