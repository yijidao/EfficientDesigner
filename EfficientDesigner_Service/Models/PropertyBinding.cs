using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfficientDesigner_Service.Models
{
    public class PropertyBinding
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PropertyBindingId { get; set; }

        public string PropertyName { get; set; }

        public string ElementName { get; set; }

        public string Value { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public Layout Layout { get; set; }

        public Guid LayoutId { get; set; }
    }
}
