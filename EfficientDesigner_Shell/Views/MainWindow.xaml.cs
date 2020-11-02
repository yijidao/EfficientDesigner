using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using EfficientDesigner_Control.Controls;
using EfficientDesigner_Service.Models;
using EfficientDesigner_Shell.Events;
using Prism;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;

namespace EfficientDesigner_Shell.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : HandyControl.Controls.Window
    {
        public MainWindow(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            eventAggregator.GetEvent<OpenLayoutView>().Subscribe(OpenTabItem);
        }

        private async void OpenTabItem(Layout layout)
        {
            var tabItem = new HandyControl.Controls.TabItem
            {
                Header = layout.DisplayName,
                Content = await DesignCanvas.LoadLayout(layout),
                ShowCloseButton = true,
                IsSelected = true
            };
            if (tabItem.Content is Canvas ctl)
            {
                ctl.Background = Application.Current.Resources["RegionBrush"] as Brush;
            }
            tabControl.Items.Add(tabItem);
        }

        private void TabControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            tabControl.Items.Add(new HandyControl.Controls.TabItem
            {
                Header = "首页",
                Content = ContainerLocator.Current.Resolve<LayoutList>(),
                IsSelected = true
            });

        }
    }
}
