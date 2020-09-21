using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EfficientDesigner_Control.Models
{
    class MasterDetailModel : MasterModel
    {

        public MasterDetailModel(FrameworkElement element, MasterModel master, List<MasterDetailModel> details = null) : base(element, details)
        {
            Master = master;
        }

        public MasterModel Master { get; set; }
    }
}
