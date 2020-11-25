 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApi.Dto
{
    public class ColumnChartData
    {
        public string Name { get; set; }

        public IEnumerable<LableValueModel> DataModels { get; set; }
    }
}
