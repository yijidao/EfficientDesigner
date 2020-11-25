 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EfficientDesigner_Control.Controls
{
    public class ColumnChartData
    {
        public string Name { get; set; }

        public IEnumerable<LableValueModel> DataModels { get; set; }
    }
}
