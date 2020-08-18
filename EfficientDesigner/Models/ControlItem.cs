using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfficientDesigner.Models
{
    public class ControlItem
    {
        private string _Title;

        public string Title
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Title))
                {
                    return ItemType.Name;
                }
                return _Title;
            }
            set
            {
                _Title = value;
            }
        }


        public Type ItemType { get; set; }
    }
}
