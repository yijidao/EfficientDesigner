using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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

            //var watch = Stopwatch.StartNew();

            var propertyDescriptors = TypeDescriptor.GetProperties(element.GetType()).OfType<PropertyDescriptor>().Where(x => x.IsBrowsable);

            var items = GetPropertyItems(propertyDescriptors).ToList();
            items.Add(GetZIndex(element));
            items.Add(GetCanvasLeft(element));
            items.Add(GetCanvasTop(element));
            items = items.OrderBy(x => x.DisplayName).ToList();
            //Debug.WriteLine(watch.ElapsedMilliseconds);
            //watch.Restart();
            ElementItemsControl.ItemsSource = items;
            //Debug.WriteLine(watch.ElapsedMilliseconds);
        }

        public PropertyResolver Resolver { get; } = new PropertyResolver();

        public IEnumerable<PropertyItem> GetPropertyItems(IEnumerable<PropertyDescriptor> propertyDescriptors)
        {
            foreach (var descriptor in propertyDescriptors)
            {
                // 只读和目前不支持的属性不显示
                if (Resolver.ResolveIsReadOnly(descriptor)) continue;
                if ((!PropertyResolver.TypeCodeDic.TryGetValue(descriptor.PropertyType, out _)) && !Resolver.IsContentProperty(descriptor)) continue;

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
                //Debug.WriteLine($"{item.PropertyName}   {item.PropertyType}");
                yield return item;
            }
        }

        private PropertyItem GetZIndex(UIElement element)
        {
            var item = new PropertyItem
            {
                Category = nameof(Panel),
                DisplayName = "Panel.ZIndex",
                Description = "Panel.ZIndex",
                IsReadOnly = false,
                DefaultValue = Panel.GetZIndex(element),
                Editor = new NumberPropertyEditor(int.MinValue, int.MaxValue),
                Value = SelectedElement,
                AttachPropertyName = "(Panel.ZIndex)",
                PropertyType = typeof(int)
            };
            item.InitEditorElement();
            return item;
        }

        private PropertyItem GetCanvasTop(UIElement element)
        {
            var item = new PropertyItem
            {
                Category = nameof(Canvas),
                DisplayName = "Canvas.Top",
                Description = "Canvas.Top",
                IsReadOnly = false,
                DefaultValue = Canvas.GetTop(element),
                Editor = new NumberPropertyEditor(double.MinValue, double.MaxValue),
                Value = SelectedElement,
                AttachPropertyName = "(Canvas.Top)",
                PropertyType = typeof(double),
            };
            item.InitEditorElement();
            return item;
        }

        private PropertyItem GetCanvasLeft(UIElement element)
        {
            var item =  new PropertyItem
            {
                Category = nameof(Canvas),
                DisplayName = "Canvas.Left",
                Description = "Canvas.Left",
                IsReadOnly = false,
                DefaultValue = Canvas.GetLeft(element),
                Editor = new NumberPropertyEditor(double.MinValue, double.MaxValue),
                Value = SelectedElement,
                AttachPropertyName = "(Canvas.Left)",
                PropertyType = typeof(double),
            };
            item.InitEditorElement();
            return item;
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
