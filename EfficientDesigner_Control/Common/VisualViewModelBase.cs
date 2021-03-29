using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace EfficientDesigner_Control.Common
{ 
    public abstract class VisualViewModelBase : BindableBase
    {
        public string ViewId { get; set; }

        public Guid LayoutId { get; set; }
    }
}
