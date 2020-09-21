using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfficientDesigner_Control.Models
{
    class MasterModel
    {
        public MasterModel(string displayName, List<MasterDetailModel> details = null)
        {
            DisplayName = displayName;
            Details = details ?? new List<MasterDetailModel>();
        }

        public string DisplayName { get; }
        public List<MasterDetailModel> Details { get; }
    }
}
