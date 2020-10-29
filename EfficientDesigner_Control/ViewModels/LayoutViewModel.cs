using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EfficientDesigner_Service.Models;

namespace EfficientDesigner_Control.ViewModels
{
    public class LayoutViewModel : BindableBase
    {
        private Layout _model;
        public Layout Model
        {
            get => _model;
            set
            {
                _model = value;
                LayoutId = _model.LayoutId;
                DisplayName = _model.DisplayName;
                File = _model.File;
                PropertyBindings = new ObservableCollection<PropertyBinding>(_model.PropertyBindings);
            }
        }

        private Guid _layoutId;
        public Guid LayoutId
        {
            get { return _layoutId; }
            set
            {
                if (SetProperty(ref _layoutId, value))
                {
                    Model.LayoutId = _layoutId;
                };
            }
        }

        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                if (SetProperty(ref _displayName, value))
                {
                    Model.DisplayName = _displayName;
                }
            }
        }

        private string _file;
        public string File
        {
            get { return _file; }
            set
            {
                if (SetProperty(ref _file, value))
                {
                    Model.File = Model.File;
                }
            }
        }

        private ObservableCollection<PropertyBindingViewModel> _propertyBindings;


        public ObservableCollection<PropertyBindingViewModel> PropertyBindings
        {
            get { return _propertyBindings; }
            set
            {
                if (SetProperty(ref _propertyBindings, value))
                {
                    Model.PropertyBindings = _propertyBindings.ToList();
                }
            }
        }
    }
}
