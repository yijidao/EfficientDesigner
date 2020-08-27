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
using System.Windows.Threading;

namespace EfficientDesigner.Controls
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

        // Using a DependencyProperty as the backing store for SelectedElement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedElementProperty =
            DependencyProperty.Register("SelectedElement", typeof(UIElement), typeof(PropertyPanel), new PropertyMetadata(null, (dp, e) =>
            {
                var ctl = (PropertyPanel)dp;
                ctl.UpdateItems(e.NewValue as UIElement);
            }));



        private void UpdateItems(UIElement selectedElement)
        {
            if (selectedElement == null || ElementItemsControl == null) return;


            var watch = Stopwatch.StartNew();

            var items = GetDependencyProperties(selectedElement.GetType())/*.Take(40)*/.Select(f => GetPropertyItem(f)).OrderBy(pi => pi.DisplayName);

            Debug.WriteLine(watch.ElapsedMilliseconds);
            watch.Restart(); 
            ElementItemsControl.ItemsSource = items;
            Debug.WriteLine(watch.ElapsedMilliseconds);


        }

        private PropertyItem GetPropertyItem(FieldInfo fieldInfo)
        {
            var dp = fieldInfo.GetValue(SelectedElement) as DependencyProperty;
            var textBox = new TextBox();
            var binding = new Binding(nameof(TextBox.Text));
            binding.Source = textBox;
            binding.Mode = BindingMode.OneWayToSource;
            BindingOperations.SetBinding(SelectedElement, dp, binding);
            return new PropertyItem { DisplayName = dp.Name, EditorElement = new TextBox() };
        }

        /// <summary>
        /// 获取自身和父类所有的依赖属性
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private IEnumerable<FieldInfo> GetDependencyProperties(Type type)
        {
            var ps = type.GetFields(BindingFlags.Public | BindingFlags.Static)
                         .Where(f => f.FieldType == typeof(DependencyProperty));
            if (type.BaseType != null)
                ps = ps.Union(GetDependencyProperties(type.BaseType));
            return ps;
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
