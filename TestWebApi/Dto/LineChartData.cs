using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWebApi.Dto
{
    public class LineChartData
    {
        public string Name { get; set; }

        public IEnumerable<DateValueModel> DataModels { get; set; }
    }
}
