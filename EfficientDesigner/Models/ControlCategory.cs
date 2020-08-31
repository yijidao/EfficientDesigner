using EfficientDesigner_Control.Interfaces;
using EfficientDesigner_Control.Controls;
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

        public IControl[] Items { get; set; }

        public static ControlCategory GetWPFCategories()
        {
            var result = new ControlCategory { Title = "WPF控件" };
            result.Items = new IControl[] { new ButtonControl(), new RectangleControl(), new TextBoxControl() };
            return result;
        }



    }
}
