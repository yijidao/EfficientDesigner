using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EfficientDesigner_Control.Common;
using EfficientDesigner_Control.ViewModels.ServicePanel;
using Prism.Mvvm;

namespace EfficientDesigner_Control.Views.ServicePanel
{
    /// <summary>
    /// ServicePanelView.xaml 的交互逻辑
    /// </summary>
    [ToTest("服务面板")]
    public partial class ServicePanelView : UserControl
    {
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(UserControl), typeof(ServicePanelView), new PropertyMetadata(null, TargetChangedCallback));

        public ServicePanelViewModel Vm => this.DataContext as ServicePanelViewModel;

        //static Func<Type, Type> _defaultViewTypeToViewModelTypeResolver =
        //    viewType =>
        //    {
        //        var viewName = viewType.FullName;
        //        viewName = viewName.Replace(".Views.", ".ViewModels.");
        //        var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
        //        var suffix = viewName.EndsWith("View") ? "Model" : "ViewModel";
        //        var viewModelName = String.Format(CultureInfo.InvariantCulture, "{0}{1}, {2}", viewName, suffix, viewAssemblyName);
        //        return Type.GetType(viewModelName);
        //    };

        public UserControl Target
        {
            get => (UserControl)GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }

        public ServicePanelView()
        {
            InitializeComponent();

            Target = new TestView();
        }

        private static void TargetChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = d as ServicePanelView;
            if (ctl?.Vm == null) return;
            
            if (e.NewValue == null)
            {
                ctl.Vm.ServiceList?.Clear();
                return;
            }

            var vmType = ServicePanelView.GetViewModelType(e.NewValue.GetType());
            var attributes = (HasServiceAttribute[])Attribute.GetCustomAttributes(vmType, typeof(HasServiceAttribute));
            ctl.Vm.ServiceList = new ObservableCollection<ServiceItemViewModel>(attributes.Select(x => new ServiceItemViewModel(x)));
        }

        public static Type GetViewModelType(Type viewType)
        {
            var viewName = viewType.FullName;
            viewName = viewName.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var suffix = viewName.EndsWith("View") ? "Model" : "ViewModel";
            var viewModelName = String.Format(CultureInfo.InvariantCulture, "{0}{1}, {2}", viewName, suffix, viewAssemblyName);
            return Type.GetType(viewModelName);

        }

    }
}
