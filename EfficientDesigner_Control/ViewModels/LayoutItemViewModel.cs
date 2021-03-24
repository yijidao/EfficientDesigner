using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfficientDesigner_Service.Models;
using Prism.Mvvm;

namespace EfficientDesigner_Control.ViewModels
{
    public class LayoutItemViewModel : BindableBase
    {
        public Layout Model { get; }

        public string Name => Model.DisplayName;

        public DateTime CreateTime => Model.CreateTime;

        public LayoutItemViewModel(Layout model)
        {
            Model = model;
        }
    }
}
