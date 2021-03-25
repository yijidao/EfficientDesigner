using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfficientDesigner_Client_Common.Models;
using EfficientDesigner_Service.Models;
using Prism.Mvvm;

namespace EfficientDesigner_Control.ViewModels
{
    public class LayoutItemViewModel : BindableBase
    {
        public LayoutModel Model { get; }

        public string Name => Model.DisplayName;

        public DateTime CreateTime => Model.CreateTime;

        public LayoutItemViewModel(LayoutModel model)
        {
            Model = model;
        }
    }
}
