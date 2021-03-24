using System;
using EfficientDesigner_Service.Models;

namespace TestWebApi.Dto
{
    public class PropertyBindingDto
    {
        private readonly PropertyBinding _propertyBinding;

        public string PropertyName => _propertyBinding.PropertyName;

        public string ElementName => _propertyBinding.ElementName;

        public string Value => _propertyBinding.Value;

        public DateTime CreateTime => _propertyBinding.CreateTime;

        public DateTime UpdateTime => _propertyBinding.UpdateTime;

        public PropertyBindingDto(PropertyBinding propertyBinding)
        {
            _propertyBinding = propertyBinding;
        }
    }
}
