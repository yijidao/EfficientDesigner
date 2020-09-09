using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EfficientDesigner_Control.Controls
{
    public class ImagePropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem) => new TextBox
        {

        };

        public override DependencyProperty GetDependencyProperty()
        {
            return TextBox.TextProperty;
        }
    }
}
