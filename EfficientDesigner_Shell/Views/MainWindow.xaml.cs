﻿using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            var canvas = await DesignCanvas.LoadLayout(layout);
            var grid = new ScrollViewer
            {
                Background = Application.Current.Resources["RegionBrush"] as Brush,
                Content = canvas,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Visible,
                VerticalScrollBarVisibility = ScrollBarVisibility.Visible
            };

            var tabItem = new HandyControl.Controls.TabItem
            {
                Header = layout.DisplayName,
                Content = grid,
                ShowCloseButton = true,
                IsSelected = true
            };
            tabItem.KeyUp += (sender, e) =>
            {
                if (e.Key != Key.F5) return;
                DesignCanvas.RefershDataSoure(canvas, layout);
            };

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
