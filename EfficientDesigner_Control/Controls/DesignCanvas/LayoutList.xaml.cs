﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<Layout> Layouts
        {
            get { return (ObservableCollection<Layout>)GetValue(LayoutsProperty); }
            set { SetValue(LayoutsProperty, value); }
        }

        public static readonly DependencyProperty LayoutsProperty =
            DependencyProperty.Register("Layouts", typeof(ObservableCollection<Layout>), typeof(LayoutList), new PropertyMetadata(new ObservableCollection<Layout>()));

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //dataGrid.ItemsSource = ServiceFactory.GetLayoutService().GetLayouts();

            Layouts = new ObservableCollection<Layout>(ServiceFactory.GetLayoutService().GetLayouts());

            var b = new Binding(nameof(Layouts)) { Mode = BindingMode.OneWay, Source = this };
            BindingOperations.SetBinding(dataGrid, DataGrid.ItemsSourceProperty, b);

            var binding = new Binding(nameof(CurrentLayout)) {Mode = BindingMode.TwoWay, Source = this};
            BindingOperations.SetBinding(dataGrid, DataGrid.SelectedItemProperty, binding);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ServiceFactory.GetLayoutService().RemoveLayout(CurrentLayout);
            Layouts.Remove(CurrentLayout);
        }
    }
}