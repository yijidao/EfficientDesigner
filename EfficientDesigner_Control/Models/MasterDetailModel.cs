using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfficientDesigner_Control.Models
{
    class MasterDetailModel
    {
        public string DisplayName { get; }
        public MasterModel Master { get; }
        public List<MasterDetailModel> Detail { get; }

        public MasterDetailModel(string displayName, MasterModel master, List<MasterDetailModel> detail)
        {
            DisplayName = displayName;
            Master = master;
            Detail = detail ?? new List<MasterDetailModel>();
        }
    }
}
