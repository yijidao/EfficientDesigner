﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace EfficientDesigner_Control.Controls
{
    public abstract class PropertyEditorBase : DependencyObject
    {
        public abstract FrameworkElement CreateElement(PropertyItem propertyItem);

        public virtual void SetBinding(PropertyItem propertyItem, DependencyObject element) =>
            BindingOperations.SetBinding(element, GetDependencyProperty(),
                new Binding($"{(string.IsNullOrWhiteSpace(propertyItem.PropertyName) ? propertyItem.AttachPropertyName : propertyItem.PropertyName)}")
                {
                    Source = propertyItem.Value,
                    Mode = GetBindingMode(propertyItem),
                    UpdateSourceTrigger = GetUpdateSourceTrigger(propertyItem),
                    Converter = GetConverter(propertyItem),
                    StringFormat = GetFormat(propertyItem)
                });

        public abstract DependencyProperty GetDependencyProperty();

        public virtual BindingMode GetBindingMode(PropertyItem propertyItem) => propertyItem.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay;

        public virtual UpdateSourceTrigger GetUpdateSourceTrigger(PropertyItem propertyItem) => UpdateSourceTrigger.LostFocus;

        public virtual IValueConverter GetConverter(PropertyItem propertyItem) => null;

        public virtual string GetFormat(PropertyItem propertyItem) => default(string);

    }
}
