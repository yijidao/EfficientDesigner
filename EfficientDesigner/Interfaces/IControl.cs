using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EfficientDesigner.Interfaces
{
    public interface IControl
    {
        string Title { get; }

        FrameworkElement GetElement();

        
    }
}
