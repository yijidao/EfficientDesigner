using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EfficientDesigner_Client_Common.Services;
using Prism.Mvvm;

namespace EfficientDesigner_Control.ViewModels.ServicePanel
{
    public class ServicePanelViewModel : BindableBase
    {
        private readonly ILayoutService _layoutService;
        private ObservableCollection<ServiceItemViewModel> _serviceList;
        public ObservableCollection<ServiceItemViewModel> ServiceList
        {
            get => _serviceList;
            set => SetProperty(ref _serviceList, value);
        }

        public ServicePanelViewModel(ILayoutService layoutService)
        {
            _layoutService = layoutService;
        }

        protected  override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(ServiceList))
            {
                foreach (var item in ServiceList)
                {
                    GetServiceInfo(item);
                }
            }
        }

        private async Task GetServiceInfo(ServiceItemViewModel serviceItem)
        {
            var result = await _layoutService.GetServiceListFor(serviceItem.Service, serviceItem.Function);
            serviceItem.ServiceInfoList = result;
        }
    }
}
