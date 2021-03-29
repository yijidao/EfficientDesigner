﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace EfficientDesigner_Control.ViewModels.ServicePanel
{
    public class ServicePanelViewModel : BindableBase
    {
        private ObservableCollection<ServiceInfoItemViewModel> _serviceList;
        public ObservableCollection<ServiceInfoItemViewModel> ServiceList
        {
            get => _serviceList;
            set => SetProperty(ref _serviceList, value);
        }

    }
}
