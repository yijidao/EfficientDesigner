using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfficientDesigner_Common.Dtos;

namespace EfficientDesigner_Common.Services
{
    public interface ILayoutService
    {
        Task<LayoutDto[]> GetLayoutList();
    }
}
