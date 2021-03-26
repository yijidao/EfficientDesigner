using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EfficientDesigner_Service.Models;

namespace EfficientDesigner_Service.Services
{
    public interface ILayoutService
    {
        Task<Layout[]> GetLayouts();

        Layout UpdateLayout(Layout layout);

        void RemoveLayout(Layout layout);

        Task<DataSource[]> GetDataSource();

        DataSource UpdateDataSource(DataSource dataSource);

        ServiceInfo[] UpdateServiceInfos(bool returnResult = false, params ServiceInfo[] serviceInfos);

        Task<ServiceInfo[]> GetServiceInfos(params string[] names);

        void RemoveServiceInfosFor(params string[] names);
    }
}
