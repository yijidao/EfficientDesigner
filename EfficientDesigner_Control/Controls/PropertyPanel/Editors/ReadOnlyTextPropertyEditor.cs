using EfficientDesigner_Control.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace EfficientDesigner_Control.Controls
{
    public class ReadOnlyTextPropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem) => new TextBox
        {
            IsReadOnly = true
        };

        public override DependencyProperty GetDependencyProperty() => TextBox.TextProperty;

        public override IValueConverter GetConverter(PropertyItem propertyItem) => Object2String.Instance;
    }
}
