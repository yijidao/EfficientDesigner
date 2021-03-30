using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfficientDesigner_Client_Common.Models;
using EfficientDesigner_Control.Common;
using Prism.Mvvm;

namespace EfficientDesigner_Control.ViewModels.ServicePanel
{
    public class ServiceItemViewModel : BindableBase
    {
        public string Description { get; set; }

        public string Service { get; set; }

        public string Function { get; set; }

        //private ObservableCollection<ServiceInfoItem> _serviceInfoList = new ObservableCollection<ServiceInfoItem>();
        //public ObservableCollection<ServiceInfoItem> ServiceInfoList
        //{
        //    get => _serviceInfoList;
        //    set => SetProperty(ref _serviceInfoList, value);
        //}
        private ServiceInfoItem[] _serviceInfoList = Array.Empty<ServiceInfoItem>();
        public ServiceInfoItem[] ServiceInfoList
        {
            get => _serviceInfoList;
            set => SetProperty(ref _serviceInfoList, value);
        }

        public ServiceItemViewModel(HasServiceAttribute hasServiceAttribute)
        {
            Description = hasServiceAttribute.Description;
            Service = hasServiceAttribute.Service;
            Function = hasServiceAttribute.Function;
        }
    }
}
