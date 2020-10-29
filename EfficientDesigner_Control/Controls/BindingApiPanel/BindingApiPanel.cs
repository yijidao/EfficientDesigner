using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EfficientDesigner_Control.ViewModels;
using EfficientDesigner_Service.Models;

namespace EfficientDesigner_Control.Controls
{
    /// <summary>
    /// 1. 新增控件 BindingAPIPanel，用于交互，用于显示属性和选择接口
    ///    a. 接口栏使用下拉选择
    /// 2. 增加特性 CanBindingAPI，用于控件识别哪些属性可以绑定到API
    /// 3. 增加数据库增加接口
    /// 4. 发布的时候将绑定关系保存到数据库
    /// 5. DesignCanvas 添加控件时，应该给控件的Name赋值为guid，然后使用界面的guid 控件 guid 形成关联关系，以Json格式保存属性和值的关系(url : www.baidu.com)。
    /// 6. item  可以使用事件跟panel交互，而不是使用绑定。
    /// </summary>
    public class BindingApiPanel : Control
    {
        private const string ItemsControlName = "PART_ItemsControl";

        static BindingApiPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BindingApiPanel), new FrameworkPropertyMetadata(typeof(BindingApiPanel)));
        }

        public static readonly DependencyProperty SelectedElementProperty = DependencyProperty.Register(
            "SelectedElement", typeof(UIElement), typeof(BindingApiPanel), new PropertyMetadata(default(UIElement), SelectedElementChangedCallback));

        private static void SelectedElementChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (!(e.NewValue is Control element) || !(d is BindingApiPanel ctl)) return;

            var bindingProperties = e.GetType().GetProperties()
                .Where(p => Attribute.GetCustomAttribute(p, typeof(BindingApiAttribute)) != null);

            var bindingApiItems = bindingProperties.Select(x =>
            {
                var item = new BindingApiItem
                {
                    PropertyName = x.Name,
                    DisplayName = x.Name,
                    ItemsSource = ctl.ApiSource,
                    LayoutId = ctl.LayoutModel.LayoutId
                };
                if (ctl.BindingPropertyDic.ContainsKey($"{element.Name}_{x.Name}"))
                {
                    item.SelectedItem = ctl.ApiSource?.FirstOrDefault(s => s == ctl.BindingPropertyDic[x.Name]);
                }
                return item;
            });



        }

        public ObservableCollection<BindingApiItem> BindingApiItems { get; set; }

        public string[] ApiSource { get; set; }


        public UIElement SelectedElement
        {
            get { return (UIElement)GetValue(SelectedElementProperty); }
            set { SetValue(SelectedElementProperty, value); }
        }




        public LayoutViewModel LayoutModel
        {
            get { return (LayoutViewModel)GetValue(LayoutModelProperty); }
            set { SetValue(LayoutModelProperty, value); }
        }

        public static readonly DependencyProperty LayoutModelProperty =
            DependencyProperty.Register("LayoutModel", typeof(LayoutViewModel), typeof(BindingApiPanel), new PropertyMetadata(default(LayoutViewModel), LayoutModelChangedCallback));

        private static void LayoutModelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is BindingApiPanel ctl)) return;
            ctl.BindingPropertyDic =
                (e.NewValue as Layout)?.PropertyBindings.ToDictionary(k => $"{k.ElementName}_{k.PropertyName}",
                    v => v.Value);
        }



        public Dictionary<string, string> BindingPropertyDic { get; set; }

        private ItemsControl ElementItemsControl { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ElementItemsControl =
                GetTemplateChild(ItemsControlName) as ItemsControl ?? throw new ArgumentException("无法转换成目标类型:ItemsControl");

            if (!AddHandler)
            {
                AddHandler = true;
                AddHandler(BindingApiItem.ValueChangedEvent, new RoutedEventHandler(BindingApiItem_ValueChanged));
            }
        }

        private void BindingApiItem_ValueChanged(object sender, RoutedEventArgs e)
        {

        }

        private bool AddHandler;

        public static readonly DependencyProperty CurrentLayoutProperty = DependencyProperty.Register(
            "CurrentLayout", typeof(object), typeof(BindingApiPanel), new PropertyMetadata(default(object)));

        public object CurrentLayout
        {
            get { return (object)GetValue(CurrentLayoutProperty); }
            set { SetValue(CurrentLayoutProperty, value); }
        }

        /// <summary>
        /// 用于储存控件和绑定了Api的属性
        /// </summary>
        public Dictionary<Guid, List<BindingApiItem>> ControlsDic { get; set; }



    }
}
