using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace EfficientDesigner_Control.Controls
{
    public class FontFamilyPropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem) =>
            new ComboBox
            {
                ItemsSource = Fonts.SystemFontFamilies,
            };

        public override DependencyProperty GetDependencyProperty() => ComboBox.SelectedItemProperty;
    }
}
