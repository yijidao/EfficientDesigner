using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfficientDesigner_Control.Controls
{
    public class DateValueModel
    {
        public DateTime Date { get; }
        public double Value { get; }

        public DateValueModel(DateTime date, double value)
        {
            Date = date;
            Value = value;
        }
    }
}
