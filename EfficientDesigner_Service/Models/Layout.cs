using System;
using System.Collections.Generic;
using System.Text;

namespace EfficientDesigner_Service.Models
{
    public class Layout
    {
        public Guid LayoutId { get; set; }

        public string DisplayName { get; set; }

        public string File { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
