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
    public class EnumPropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem) => new ComboBox
        {
            IsEnabled = !propertyItem.IsReadOnly,
            ItemsSource = Enum.GetValues(propertyItem.PropertyType)
        };

        public override DependencyProperty GetDependencyProperty() => ComboBox.SelectedItemProperty;
    }
}
