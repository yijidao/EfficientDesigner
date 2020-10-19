using System;
using System.Collections.Generic;
using System.Text;
using EfficientDesigner_Service.Models;

namespace EfficientDesigner_Service.Services
{
    interface ILayoutService
    {
        Layout[] GetLayouts();

        void UpdateLayout(Layout layout);
    }
}
