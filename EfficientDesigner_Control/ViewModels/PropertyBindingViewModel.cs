using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfficientDesigner_Service.Models;

namespace EfficientDesigner_Control.ViewModels
{
    public class PropertyBindingViewModel : BindableBase
    {
        private PropertyBinding _model;

        public PropertyBinding Model
        {
            get { return _model; }
            set
            {
                _model = value;
                PropertyBindingId = _model.PropertyBindingId;
                PropertyName = _model.PropertyName;
                ElementName = _model.ElementName;
                Value = _model.Value;
            }
        }

        private Guid _propertyBindingId;
        public Guid PropertyBindingId
        {
            get { return _propertyBindingId; }
            set
            {
                if (SetProperty(ref _propertyBindingId, value))
                {
                    Model.PropertyBindingId = _propertyBindingId;
                }
            }
        }

        private string _propertyName;
        public string PropertyName
        {
            get { return _propertyName; }
            set
            {
                if (SetProperty(ref _propertyName, value))
                {
                    Model.PropertyName = _propertyName;
                }
            }
        }

        private string _elementName;
        public string ElementName
        {
            get { return _elementName; }
            set
            {
                if (SetProperty(ref _elementName, value))
                {
                    Model.ElementName = _elementName;
                }
            }
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                if (SetProperty(ref _value, value))
                {
                    Model.Value = _value;
                }
            }
        }

    }
}
