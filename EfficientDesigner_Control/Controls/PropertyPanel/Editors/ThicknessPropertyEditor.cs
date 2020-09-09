using EfficientDesigner_Control.Controls.Editors;
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
    public class ThicknessPropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem) => new ThicknessGrid();

        public override DependencyProperty GetDependencyProperty() => ThicknessGrid.ValueProperty;
    }
}
