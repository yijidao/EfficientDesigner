using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EfficientDesigner_Control.Controls
{
    public class BindingApiItem : Control
    {
        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(
            "DisplayName", typeof(string), typeof(BindingApiItem), new PropertyMetadata(default(string)));

        public string DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof(string[]), typeof(BindingApiItem), new PropertyMetadata(default(string[])));

        public string[] ItemsSource
        {
            get { return (string[])GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem", typeof(string), typeof(BindingApiItem), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                delegate (DependencyObject o, DependencyPropertyChangedEventArgs args)
                {
                    var ctl = ((BindingApiItem)o);
                    ctl.RaiseEvent(new RoutedEventArgs(ValueChangedEvent, ctl));
                }));

        public string SelectedItem
        {
            get { return (string)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(
            "PropertyName", typeof(string), typeof(BindingApiItem), new PropertyMetadata(default(string)));

        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        public Guid LayoutId { get; set; }

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(BindingApiItem));

        public event RoutedEventHandler ValueChanged
        {
            add => AddHandler(ValueChangedEvent, value);
            remove => RemoveHandler(ValueChangedEvent, value);
        }
    }
}
