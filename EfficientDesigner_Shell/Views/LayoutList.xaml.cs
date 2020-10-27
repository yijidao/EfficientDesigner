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
using EfficientDesigner_Service.Models;
using EfficientDesigner_Service.Services;
using EfficientDesigner_Shell.Events;
using Prism.Events;

namespace EfficientDesigner_Shell.Views
{
    /// <summary>
    /// LayoutList.xaml 的交互逻辑
    /// </summary>
    public partial class LayoutList : UserControl
    {
        private readonly IEventAggregator _eventAggregator;

        public LayoutList(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _eventAggregator = eventAggregator;
        }

        private void Layout_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is Button button && button.DataContext is Layout layout)) return;

            _eventAggregator.GetEvent<OpenLayoutView>().Publish(layout);
        }
    }
}
