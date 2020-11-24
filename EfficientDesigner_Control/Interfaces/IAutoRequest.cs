using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfficientDesigner_Control.Interfaces
{
    interface IAutoRequest
    {
        /// <summary>
        /// 间隔时间，单位为秒
        /// </summary>
        public int Interval { get; set; }
    }
}
