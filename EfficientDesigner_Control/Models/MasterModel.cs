using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EfficientDesigner_Control.Models
{
    class MasterModel
    {
        public FrameworkElement Element { get; }
        public List<MasterDetailModel> Details { get; }

        public string DisplayName { get; }

        public MasterModel(FrameworkElement element, List<MasterDetailModel> details = null)
        {
            Element = element;
            DisplayName = string.IsNullOrWhiteSpace(element.Name) ? "[Null]" : $"[{element.Name}]" + $"[{element.GetType().Name}]";
            Details = details ?? new List<MasterDetailModel>();
        }

    }
}
