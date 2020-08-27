using EfficientDesigner.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EfficientDesigner.Models
{
    public class ButtonControl : IControl
    {
        public string Title => "Button";

        //public PropertyInfo[] GetProperties(Button button)
        //{
        //    //typeof(Button).GetProperty(nameof(Button.Width));
           
        //}

        public FrameworkElement GetElement()
        {

            return new Button
            {
                Content = Title,
                Width = 120,
                Height = 40
            };
        }
    }
}
