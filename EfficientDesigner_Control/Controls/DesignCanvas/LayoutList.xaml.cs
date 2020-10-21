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
using EfficientDesigner_Service;
using EfficientDesigner_Service.Models;

namespace EfficientDesigner_Control.Controls
{
    /// <summary>
    /// LayoutList.xaml 的交互逻辑
    /// </summary>
    public partial class LayoutList : UserControl
    {
        public LayoutList()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }



        public Layout CurrentLayout
        {
            get { return (Layout)GetValue(CurrentLayoutProperty); }
            set { SetValue(CurrentLayoutProperty, value); }
        }

        public static readonly DependencyProperty CurrentLayoutProperty =
            DependencyProperty.Register("CurrentLayout", typeof(Layout), typeof(LayoutList), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = ServiceFactory.GetLayoutService().GetLayouts();
            var binding = new Binding(nameof(CurrentLayout));
            binding.Mode = BindingMode.TwoWay;
            binding.Source = this;
            BindingOperations.SetBinding(dataGrid, DataGrid.SelectedItemProperty, binding);
            //dataGrid.SelectedItem = 
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
        }
    }
}
