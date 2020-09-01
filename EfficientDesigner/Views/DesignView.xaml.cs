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

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Back || e.Key == Key.Delete))
            {
                DesignPanel.RemoveChild();
            }
        }
    }
}
