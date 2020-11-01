using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EfficientDesigner_Control.ViewModels;
using EfficientDesigner_Service;
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
        private const string RefreshButtonName = "PART_RefreshButton";

        static BindingApiPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BindingApiPanel), new FrameworkPropertyMetadata(typeof(BindingApiPanel)));
        }

        public static readonly DependencyProperty SelectedElementProperty = DependencyProperty.Register(
            "SelectedElement", typeof(UIElement), typeof(BindingApiPanel), new PropertyMetadata(default(UIElement), SelectedElementChangedCallback));

        private static void SelectedElementChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (BindingApiPanel)d;
            if (e.NewValue == null)
            {
                ctl.ElementItemsControl.ItemsSource = null;
                return;
            }


            var bindingProperties = e.NewValue.GetType().GetProperties()
                .Where(p => Attribute.GetCustomAttribute(p, typeof(BindingApiAttribute)) != null);

            var element = (FrameworkElement)e.NewValue;

            var bindingApiItems = bindingProperties.Select(x =>
            {
                var item = new BindingApiItem
                {
                    PropertyName = x.Name,
                    DisplayName = x.Name,
                    ItemsSource = ctl.Apis,
                    LayoutId = ctl.LayoutModel.LayoutId
                };

                if (ctl.BindingPropertyDic.ContainsKey($"{element.Name}_{x.Name}"))
                {
                    item.SelectedItem =
                        ctl.Apis?.FirstOrDefault(api => api == ctl.BindingPropertyDic[$"{element.Name}_{x.Name}"]);
                }

                return item;
            });

            ctl.ElementItemsControl.ItemsSource = bindingApiItems;
        }

        /// <summary>
        /// 还没赋值，从数据库查询即可
        /// 刷新的时候也要更新这个数组
        /// </summary>
        public string[] Apis
        {
            get
            {
                if (_apis == null)
                {
                    var t = GetDataSources();
                    t.Wait();
                    _apis = t.Result.Select(x => x.Api).ToArray();
                }
                return _apis;
            }
            set => _apis = value;
        }

        private async Task<DataSource[]> GetDataSources() => await ServiceFactory.GetLayoutService().GetDataSource();

        public UIElement SelectedElement
        {
            get { return (UIElement)GetValue(SelectedElementProperty); }
            set { SetValue(SelectedElementProperty, value); }
        }


        public Layout LayoutModel
        {
            get { return (Layout)GetValue(LayoutModelProperty); }
            set { SetValue(LayoutModelProperty, value); }
        }

        public static readonly DependencyProperty LayoutModelProperty =
            DependencyProperty.Register("LayoutModel", typeof(Layout), typeof(BindingApiPanel), new PropertyMetadata(default(Layout), LayoutModelChangedCallback));

        private static void LayoutModelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (BindingApiPanel)d;

            ctl.BindingPropertyDic =
                (e.NewValue as Layout)?.PropertyBindings?.ToDictionary(k => $"{k.ElementName}_{k.PropertyName}",
                    v => v.Value) ?? new Dictionary<string, string>();
        }

        /// <summary>
        /// 存放键为 "ElementName_PropertyName" 值为具体Api的字典。
        /// </summary>
        public Dictionary<string, string> BindingPropertyDic { get; set; }

        private ItemsControl ElementItemsControl { get; set; }

        private Button RefreshButton { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ElementItemsControl = GetTemplateChild(ItemsControlName) as ItemsControl ?? throw new ArgumentException("无法转换成目标类型:ItemsControl");
            RefreshButton = GetTemplateChild(RefreshButtonName) as Button ?? throw new ArgumentException("无法转换成目标类型：Button");
            RefreshButton.Click += RefreshButtonOnClick;

            if (!_addedHandler)
            {
                _addedHandler = true;
                AddHandler(BindingApiItem.ValueChangedEvent, new RoutedEventHandler(BindingApiItem_ValueChanged));
            }
        }

        private async void RefreshButtonOnClick(object sender, RoutedEventArgs e)
        {
            var dataSources = await GetDataSources();
            Apis = dataSources.Select(x => x.Api).ToArray();

            if (!(ElementItemsControl.ItemsSource is IEnumerable<BindingApiItem> bindingApiItems)) return;

            foreach (var bindingApiItem in bindingApiItems)
            {
                bindingApiItem.ItemsSource = Apis;
            }
        }

        private void BindingApiItem_ValueChanged(object sender, RoutedEventArgs e)
        {
            var item = (BindingApiItem)e.OriginalSource;
            var name = ((FrameworkElement)SelectedElement).Name;
            var k = $"{name}_{item.PropertyName}";
            if (BindingPropertyDic.ContainsKey(k))
            {
                BindingPropertyDic[k] = item.SelectedItem;
            }
            else
            {
                BindingPropertyDic.Add(k, item.SelectedItem);
            }

            var p = LayoutModel.PropertyBindings?.FirstOrDefault(x =>
                x.ElementName == name && x.PropertyName == item.PropertyName);
            if (p != null)
            {
                p.Value = item.SelectedItem;
            }
            else
            {
                (LayoutModel.PropertyBindings ??= new List<PropertyBinding>()).Add(new PropertyBinding
                {
                    ElementName = name,
                    PropertyName = item.PropertyName,
                    Value = item.SelectedItem,
                    Layout = LayoutModel,
                });
            }
        }

        private bool _addedHandler;
        private string[] _apis;
    }
}
