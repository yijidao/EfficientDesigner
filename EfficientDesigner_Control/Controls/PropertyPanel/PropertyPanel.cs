using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup.Localizer;
using System.Windows.Threading;

namespace EfficientDesigner_Control.Controls
{
    public class PropertyPanel : Control
    {
        private const string ElementItemsControlName = "PART_ItemsControl";

        public PropertyPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyPanel), new FrameworkPropertyMetadata(typeof(PropertyPanel)));
        }

        public UIElement SelectedElement
        {
            get { return (UIElement)GetValue(SelectedElementProperty); }
            set { SetValue(SelectedElementProperty, value); }
        }

        public static readonly DependencyProperty SelectedElementProperty =
            DependencyProperty.Register("SelectedElement", typeof(UIElement), typeof(PropertyPanel), new PropertyMetadata(null, (dp, e) =>
            {
                var ctl = (PropertyPanel)dp;
                ctl.UpdateItems(e.NewValue as UIElement);
            }));



        private void UpdateItems(UIElement element)
        {
            if (element == null || ElementItemsControl == null) return;

            var watch = Stopwatch.StartNew();

            var propertyDescriptors = TypeDescriptor.GetProperties(element.GetType()).OfType<PropertyDescriptor>().Where(x => x.IsBrowsable);

            var items = GetPropertyItems(propertyDescriptors);

            Debug.WriteLine(watch.ElapsedMilliseconds);
            watch.Restart();
            ElementItemsControl.ItemsSource = items;
            Debug.WriteLine(watch.ElapsedMilliseconds);
        }

        public PropertyResolver Resolver { get; } = new PropertyResolver();

        public IEnumerable<PropertyItem> GetPropertyItems(IEnumerable<PropertyDescriptor> propertyDescriptors)
        {
            foreach (var descriptor in propertyDescriptors)
            {
                var item = new PropertyItem
                {
                    Category = Resolver.ResolveCategory(descriptor),
                    DisplayName = Resolver.ResolveDisplayName(descriptor),
                    Description = Resolver.ResolveDescription(descriptor),
                    IsReadOnly = Resolver.ResolveIsReadOnly(descriptor),
                    DefaultValue = Resolver.ResolveDefaultValue(descriptor),
                    Editor = Resolver.ResolveEditor(descriptor),
                    Value = SelectedElement,
                    PropertyName = descriptor.Name,
                    PropertyType = descriptor.PropertyType,
                };
                item.InitEditorElement();
                yield return item;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ElementItemsControl = GetTemplateChild(ElementItemsControlName) as ItemsControl;
            UpdateItems(SelectedElement);
        }

        private ItemsControl ElementItemsControl { get; set; }
    }
}
