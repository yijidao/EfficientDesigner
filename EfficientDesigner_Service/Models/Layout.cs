using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EfficientDesigner_Service.Models
{
    public class Layout
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid LayoutId { get; set; }

        public string DisplayName { get; set; }

        public string File { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public List<PropertyBinding> PropertyBindings { get; set; }
    }
}
