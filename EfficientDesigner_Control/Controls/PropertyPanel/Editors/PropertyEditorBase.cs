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
        public abstract FrameworkElement CreateElement(PropertyItem propertyItem);

        public virtual void SetBinding(PropertyItem propertyItem, DependencyObject element) =>
            BindingOperations.SetBinding(element, GetDependencyProperty(),
                new Binding($"{propertyItem.Name}")
                {
                    Source = propertyItem.Value,
                    Mode = GetBindingMode(propertyItem),
                    UpdateSourceTrigger = GetUpdateSourceTrigger(propertyItem),
                    Converter = GetConverter(propertyItem),
                    StringFormat = GetFormat(propertyItem)
                });

        public abstract DependencyProperty GetDependencyProperty();

        public virtual BindingMode GetBindingMode(PropertyItem propertyItem) => propertyItem.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay;

        public virtual UpdateSourceTrigger GetUpdateSourceTrigger(PropertyItem propertyItem) => UpdateSourceTrigger.PropertyChanged;

        public virtual IValueConverter GetConverter(PropertyItem propertyItem) => null;

        public virtual string GetFormat(PropertyItem propertyItem) => default(string);

    }
}
