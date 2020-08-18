using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace EfficientDesigner.Models
{
    public class ControlCategory
    {
        public string Title { get; set; }

        public ControlItem[] Items { get; set; }

        public static ControlCategory GetWPFCategories()
        {
            var result = new ControlCategory { Title = "WPF控件" };
            result.Items = new Type[] { typeof(TextBlock), typeof(TextBox), typeof(Button), typeof(Ellipse), typeof(Rectangle) }
                           .Select(x => new ControlItem { ItemType = x })
                           .ToArray();
            return result;
        }



    }
}
