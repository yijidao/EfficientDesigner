using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfficientDesigner_Client_Common.Models
{
    public class LayoutModel
    {
        public string DisplayName { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public string File { get; set; }

        public PropertyBindingModel[] PropertyBindings { get; set; } = Array.Empty<PropertyBindingModel>();

        public LayoutModel()
        {

        }
    }

    public class PropertyBindingModel
    {
        public string PropertyName { get; set; }

        public string ElementName { get; set; }

        public string Value { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
