﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using EfficientDesigner_Client_Common.Models;

namespace EfficientDesigner_Control.ViewModels
{
    public class ServiceInfoItemViewModel : BindableBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _address;
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        private bool _enable;
        public bool Enable
        {
            get => _enable;
            set => SetProperty(ref _enable, value);
        }

        public ServiceInfoItemViewModel(ServiceInfoItem model)
        {
            Name = model.Name;
            Address = model.Address;
            Enable = model.Enable;
        }
    }
}
