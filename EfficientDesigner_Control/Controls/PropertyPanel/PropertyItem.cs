using EfficientDesigner_Control.Controls.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EfficientDesigner_Control.Controls
{
    public class PropertyItem : Control
    {


        public string DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register("DisplayName", typeof(string), typeof(PropertyItem), new PropertyMetadata(default(string)));

        public FrameworkElement EditorElement
        {
            get { return (FrameworkElement)GetValue(EditorElementProperty); }
            set { SetValue(EditorElementProperty, value); }
        }

        public static readonly DependencyProperty EditorElementProperty =
            DependencyProperty.Register("EditorElement", typeof(FrameworkElement), typeof(PropertyItem), new PropertyMetadata(default(string)));

        public static double GetDisplayNameWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(DisplayNameWidthProperty);
        }

        public static void SetDisplayNameWidth(DependencyObject obj, double value)
        {
            obj.SetValue(DisplayNameWidthProperty, value);
        }

        public static readonly DependencyProperty DisplayNameWidthProperty =
            DependencyProperty.RegisterAttached("DisplayNameWidth", typeof(double), typeof(PropertyItem), new PropertyMetadata(150d));

        public string Category
        {
            get { return (string)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register("Category", typeof(string), typeof(PropertyItem), new PropertyMetadata(default));


        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(PropertyItem), new PropertyMetadata(default));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(PropertyItem), new PropertyMetadata(false));



        public object DefaultValue
        {
            get { return (object)GetValue(DefaultValueProperty); }
            set { SetValue(DefaultValueProperty, value); }
        }

        public static readonly DependencyProperty DefaultValueProperty =
            DependencyProperty.Register("DefaultValue", typeof(object), typeof(PropertyItem), new PropertyMetadata(null));



        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(PropertyItem), new PropertyMetadata(null));



        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register("PropertyName", typeof(string), typeof(PropertyItem), new PropertyMetadata(default));



        public Type PropertyType
        {
            get { return (Type)GetValue(PropertyTypeProperty); }
            set { SetValue(PropertyTypeProperty, value); }
        }

        public static readonly DependencyProperty PropertyTypeProperty =
            DependencyProperty.Register("PropertyType", typeof(Type), typeof(PropertyItem), new PropertyMetadata(null));


        public void InitEditorElement()
        {
            if (Editor == null) return;
            EditorElement = Editor.CreateElement(this);
            Editor.SetBinding(this, EditorElement);
        }

        public PropertyEditorBase Editor { get; set; }
    }
}
