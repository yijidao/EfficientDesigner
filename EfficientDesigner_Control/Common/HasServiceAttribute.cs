using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfficientDesigner_Control.Common
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class HasServiceAttribute: Attribute
    {
        public string Service { get; }
        public string Function { get; }

        public string Description { get; set; }

        public HasServiceAttribute(string service, string function, string description)
        {
            Service = service;
            Function = function;
            Description = description;
        }
    }
}
