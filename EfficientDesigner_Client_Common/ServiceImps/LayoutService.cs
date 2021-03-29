using System;
using System.Net.Http;
using System.Threading.Tasks;
using EfficientDesigner_Client_Common.Models;
using EfficientDesigner_Client_Common.Services;
using Newtonsoft.Json;

namespace EfficientDesigner_Client_Common.ServiceImps
{
    public class LayoutService : ILayoutService
    {
        HttpClient Client { get; } = new HttpClient();

        private const string LayoutListUrl = "https://localhost:5001/efficientdesignapi/layoutlist";

        private const string ServiceListUrl = "https://localhost:5001/efficientdesignapi/serviceinfolist";

        public async Task<LayoutModel[]> GetLayoutList()
        {
            var result = await Client.GetAsync(LayoutListUrl);
            var content = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($"状态码：{result.StatusCode}，内容：{content}");
            }

            return JsonConvert.DeserializeObject<LayoutModel[]>(content);
        }

        public async Task<ServiceInfoItem[]> GetServiceList()
        {
            var result = await Client.GetAsync(ServiceListUrl);
            var content = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($"状态码：{result.StatusCode}，内容：{content}");
            }

            return JsonConvert.DeserializeObject<ServiceInfoItem[]>(content);
        }

        public async Task<string[]> GetTestList(Guid layoutId, string viewId)
        {

            var url = ""; // 通过布局ID、组件ID、方法名、找对应的url

            var result = await Client.GetAsync(url);
            var content = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($"状态码：{result.StatusCode}，内容：{content}");
            }

            return JsonConvert.DeserializeObject<string[]>(content);
        }
    }
}
