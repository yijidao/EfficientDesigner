using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfficientDesigner_Control.Common
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    class HasServiceAttribute: Attribute
    {
        public string InterfaceName { get; }
        public string MethodName { get; }

        public HasServiceAttribute(string interfaceName, string methodName)
        {
            InterfaceName = interfaceName;
            MethodName = methodName;
        }
    }
}
