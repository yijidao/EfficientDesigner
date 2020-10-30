using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfficientDesigner_Service.Models
{
    public class DataSource
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid DataSourceId { get; set; }

        public string Api { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
