using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EfficientDesigner_Control.Controls
{
    public class FontStylePropertyEditor : PropertyEditorBase
    {
        private static FontStyle[] _fontStyleCollection = new FontStyle[]
        {
            FontStyles.Normal,
            FontStyles.Oblique,
            FontStyles.Italic
        };

        public override FrameworkElement CreateElement(PropertyItem propertyItem) =>
            new ComboBox
            {
                ItemsSource = _fontStyleCollection
            };

        public override DependencyProperty GetDependencyProperty() => ComboBox.SelectedItemProperty;
    }
}
