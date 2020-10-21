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

namespace EfficientDesigner_Control.Controls
{
    /// <summary>
    /// PublishLayout.xaml 的交互逻辑
    /// </summary>
    public partial class PublishLayout : UserControl
    {
        public PublishLayout()
        {
            InitializeComponent();
        }

        public string InputText
        {
            get { return (string)GetValue(InputTextProperty); }
            set { SetValue(InputTextProperty, value); }
        }

        public static readonly DependencyProperty InputTextProperty =
            DependencyProperty.Register("InputText", typeof(string), typeof(PublishLayout), new PropertyMetadata(""));


        public static readonly RoutedEvent YesEvent = EventManager.RegisterRoutedEvent("Yes", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(PublishLayout));

        public event RoutedEventHandler Yes
        {
            add => AddHandler(YesEvent, value);
            remove => RemoveHandler(YesEvent, value);
        }

        public static readonly RoutedEvent NoEvent = EventManager.RegisterRoutedEvent("No", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(PublishLayout));

        public event RoutedEventHandler No
        {
            add => AddHandler(NoEvent, value);
            remove => RemoveHandler(NoEvent, value);
        }


        private void YesButton_OnClick(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(YesEvent, this));

        private void NoButton_OnClick(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(NoEvent, this));
    }
}
