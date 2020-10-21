using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfficientDesigner_Service.ServiceImplements;
using EfficientDesigner_Service.Services;

namespace EfficientDesigner_Service
{
    public static class ServiceFactory
    {
        private static ILayoutService LayoutService { get;  } = new LayoutService();

        public static ILayoutService GetLayoutService() => LayoutService;

    }
}
