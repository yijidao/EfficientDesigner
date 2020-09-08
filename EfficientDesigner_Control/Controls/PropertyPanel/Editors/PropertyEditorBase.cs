using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace EfficientDesigner_Control.Controls.Editors
{
    public abstract class PropertyEditorBase : DependencyObject

    {
        public virtual void SetBinding(PropertyItem propertyItem, DependencyObject dp)
        { }

        public abstract FrameworkElement CreateElement(PropertyItem propertyItem);

    }
}
