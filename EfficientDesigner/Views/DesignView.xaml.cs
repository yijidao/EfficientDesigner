using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        //private void TreeView_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed && ToolBoxView.SelectedItem is IControl controlItem)
        //    {
        //        var data = new DataObject();
        //        data.SetData("control", controlItem);
        //        DragDrop.DoDragDrop(sender as DependencyObject, data, DragDropEffects.Copy);
        //    }
        //}

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Back || e.Key == Key.Delete))
            {
                DesignPanel.RemoveChildren();
            }
        }


    }
}
