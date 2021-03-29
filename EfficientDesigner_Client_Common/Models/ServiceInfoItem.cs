using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfficientDesigner_Client_Common.Models
{
    public class ServiceInfoItem
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public bool Enable { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

    }
}
