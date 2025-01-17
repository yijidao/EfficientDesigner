﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EfficientDesigner_Control.Controls
{
    public class SwitchPropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem) => new CheckBox
        {
            IsEnabled = !propertyItem.IsReadOnly,
            HorizontalAlignment = HorizontalAlignment.Left
        };

        public override DependencyProperty GetDependencyProperty() => CheckBox.IsCheckedProperty;
    }
}
