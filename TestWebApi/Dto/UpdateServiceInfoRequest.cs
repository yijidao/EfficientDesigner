using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EfficientDesigner_Service.Models;

namespace TestWebApi.Dto
{
    public class UpdateServiceInfoRequest
    {
        public bool ReturnUpdateData { get; set; }

        public ServiceInfo[] Datas { get; set; }

        //public IEnumerable<ServiceInfo> AddDatas { get; set; }

        //public IEnumerable<ServiceInfo> UpdateDatas { get; set; }
    }
}
