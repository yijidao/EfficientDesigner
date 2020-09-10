using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EfficientDesigner_Control.Controls
{
    public class FontStretchPropertyEditor : PropertyEditorBase
    {
        private static readonly FontStretch[] _fontStretchCollection = new FontStretch[] 
        {
            FontStretches.Condensed,
            FontStretches.Expanded,
            FontStretches.ExtraCondensed,
            FontStretches.ExtraExpanded,
            FontStretches.Medium,
            FontStretches.Normal,
            FontStretches.SemiCondensed,
            FontStretches.SemiExpanded,
            FontStretches.UltraCondensed,
            FontStretches.UltraExpanded,
        };


        public override FrameworkElement CreateElement(PropertyItem propertyItem) =>
            new ComboBox
            {
                ItemsSource = _fontStretchCollection
            };

        public override DependencyProperty GetDependencyProperty() => ComboBox.SelectedItemProperty;
    }
}
