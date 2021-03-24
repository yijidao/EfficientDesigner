using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfficientDesigner_Control.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ToTestAttribute : Attribute
    {
        public string Name { get; }

        public ToTestAttribute(string name)
        {
            Name = name;
        }

        public ToTestAttribute()
        {
            Name = string.Empty;
        }
    }
}
