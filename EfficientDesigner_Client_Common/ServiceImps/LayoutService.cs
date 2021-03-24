using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EfficientDesigner_Common.Dtos;
using EfficientDesigner_Common.Services;
using Newtonsoft.Json;

namespace EfficientDesigner_Common.ServiceImps
{
    public class LayoutService : ILayoutService
    {
        HttpClient Client { get; } = new HttpClient();

        private const string LayoutListUrl = "https://localhost:5001/efficientdesignapi/layoutlist";

        public async Task<LayoutDto[]> GetLayoutList()
        {
            var result = await Client.GetAsync(LayoutListUrl);
            var content =await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($"状态码：{result.StatusCode}，内容：{content}");
            }

            return JsonConvert.DeserializeObject<LayoutDto[]>(content);
        }
    }
}
