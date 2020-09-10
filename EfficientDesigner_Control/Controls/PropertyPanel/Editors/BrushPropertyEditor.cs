
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
    public class BrushPropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem) => new TextBox();

        public override DependencyProperty GetDependencyProperty() => TextBox.TextProperty;

        public override IValueConverter GetConverter(PropertyItem propertyItem) => Converters.String2Brush.Instance;
    }
}
