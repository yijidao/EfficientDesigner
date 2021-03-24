using System;
using System.Linq;
using EfficientDesigner_Service.Models;

namespace TestWebApi.Dto
{
    public class LayoutDto
    {
        private readonly Layout _layout;

        public string DisplayName => _layout.DisplayName;

        public DateTime CreateTime => _layout.CreateTime;

        public DateTime UpdateTime => _layout.UpdateTime;

        public string File => _layout.File;

        public PropertyBindingDto[] PropertyBindings { get; }

        public LayoutDto(Layout layout)
        {
            _layout = layout;

            PropertyBindings = layout?.PropertyBindings.Select(x => new PropertyBindingDto(x)).ToArray() ?? Array.Empty<PropertyBindingDto>();
        }
    }
}
