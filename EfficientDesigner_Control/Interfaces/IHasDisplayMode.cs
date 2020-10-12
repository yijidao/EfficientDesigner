using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfficientDesigner_Control.Controls;

namespace EfficientDesigner_Control.Interfaces
{
    /// <summary>
    /// 用来表示控件具有设计模式和运行模式两种状态
    /// </summary>
    interface IHasDisplayMode
    {
        public ControlDisplayMode GetDisplayMode();

        public void SetDisplayMode(ControlDisplayMode mode);
    }
}
