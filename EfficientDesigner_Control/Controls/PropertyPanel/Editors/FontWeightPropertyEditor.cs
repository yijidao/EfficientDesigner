using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EfficientDesigner_Control.Controls
{
    public class FontWeightPropertyEditor : PropertyEditorBase
    {
        private static readonly FontWeight[] _fontWeightCollection = new FontWeight[] {
            FontWeights.Thin,
            FontWeights.ExtraLight,
            FontWeights.UltraLight,
            FontWeights.Light,
            FontWeights.Normal,
            FontWeights.Regular,
            FontWeights.Medium,
            FontWeights.DemiBold,
            FontWeights.SemiBold,
            FontWeights.Bold,
            FontWeights.ExtraBold,
            FontWeights.UltraBold,
            FontWeights.Black,
            FontWeights.Heavy,
            FontWeights.ExtraBlack,
            FontWeights.UltraBlack,
};

        public override FrameworkElement CreateElement(PropertyItem propertyItem) => new ComboBox
        {
            ItemsSource = _fontWeightCollection
        };

        public override DependencyProperty GetDependencyProperty() => ComboBox.SelectedItemProperty;
    }
}
