using System;
using System.Threading.Tasks;
using EfficientDesigner_Client_Common.Models;

namespace EfficientDesigner_Client_Common.Services
{
    public interface ILayoutService
    {
        Task<LayoutModel[]> GetLayoutList();

        Task<ServiceInfoItem[]> GetServiceList();

        Task<string[]> GetTestList(Guid layoutId, string viewId);

        Task<ServiceInfoItem[]> GetServiceListFor(string service, string function);
    }
}
