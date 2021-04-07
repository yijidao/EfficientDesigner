using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using EfficientDesigner_Service;
using EfficientDesigner_Service.Models;
using EfficientDesigner_Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TestWebApi.Dto;

namespace TestWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EfficientDesignApiController : ControllerBase
    {
        private readonly ILogger<EfficientDesignApiController> _logger;
        private readonly ILayoutService _layoutService;

        public Dictionary<string, Dictionary<DateTime, int>> LinePassengerFlowDic { get; set; } = new Dictionary<string, Dictionary<DateTime, int>>();

        //public Dictionary<string, Dictionary<DateTime, int>> InOutPassengerFlowDic { get; set; } = new Dictionary<string, Dictionary<DateTime, int>>();

        public EfficientDesignApiController(ILogger<EfficientDesignApiController> logger, ILayoutService layoutService)
        {
            _logger = logger;
            _layoutService = layoutService;

            LinePassengerFlowDic.Add("一号线", GeneratePassengerFlow());
            LinePassengerFlowDic.Add("二号线", GeneratePassengerFlow());
            LinePassengerFlowDic.Add("三号线", GeneratePassengerFlow());
            LinePassengerFlowDic.Add("四号线", GeneratePassengerFlow());
            LinePassengerFlowDic.Add("五号线", GeneratePassengerFlow());
            LinePassengerFlowDic.Add("六号线", GeneratePassengerFlow());

            //InOutPassengerFlowDic.Add("进站", GeneratePassengerFlow());
            //InOutPassengerFlowDic.Add("出站", GeneratePassengerFlow());
        }

        private Dictionary<DateTime, int> GeneratePassengerFlow()
        {
            var random = new Random();
            var passengerFlow = new Dictionary<DateTime, int>();

            var current = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 6, 0, 0);
            for (int i = 0; i < 12; i++)
            {
                passengerFlow.Add(current, random.Next(1, 15));
                current = current.AddHours(1);
            }

            return passengerFlow;
        }



        [HttpGet]
        public string Get()
        {
            return "google and  baidu";
        }

        [HttpGet("getgoogle")]
        public string Google()
        {
            return "www.google.com";
        }

        [HttpGet("getbaidu")]
        public string BaiDu()
        {
            return "www.baidu.com";
        }

        [HttpGet("GetMap")]
        public string Map()
        {
            return "https://map.baidu.com/search/%E7%8F%A0%E6%B1%9F%E6%96%B0%E5%9F%8E/@12616137.89549133,2630248.205,17.61z?querytype=s&da_src=shareurl&wd=%E7%8F%A0%E6%B1%9F%E6%96%B0%E5%9F%8E&c=257&src=0&pn=0&sug=0&l=11&b=(12541425.533842426,2610838.854180073;12656488.70143085,2664924.5358199445)&from=webmap&biz_forward=%7B%22scaler%22:2,%22styles%22:%22pl%22%7D&device_ratio=2";
        }

        [HttpGet("GetMetro")]
        public string Metro()
        {
            return "http://ydyc.gzmtr.cn:19090/pcindex.html#/";
        }

        [HttpGet("GetPCI")]
        public string PCI()
        {
            return "https://www.pcitech.com/";
        }

        [HttpGet("GetImage")]
        public string Image()
        {
            return
                "https://cdn.jsdelivr.net/gh/apache/incubator-echarts-website@asf-site/examples/data/thumb/scatter-matrix.jpg?_v_=20200710_1";
        }



        [HttpGet("GetPassengerFlow")]
        public string PassengerFlow()
        {
            var data = LinePassengerFlowDic.Select(dic => new LineChartData
            {
                Name = dic.Key,
                DataModels = dic.Value.Where(dic2 => dic2.Key <= DateTime.Now)
                    .Select(dic2 => new DateValueModel(dic2.Key, dic2.Value))
            });

            return JsonConvert.SerializeObject(data);
        }

        [HttpGet("GetInOutPassengerFlow")]
        public string InOutPassengerFlow()
        {
            var inDic = GeneratePassengerFlow();
            var outDic = GeneratePassengerFlow();

            var data = new ColumnChartData[]
            {
                new ColumnChartData
                {
                    Name = "入站",
                    DataModels = inDic.Where(dic => dic.Key <= DateTime.Now).Select(dic => new LableValueModel{Lable = $"{dic.Key.Hour}:00", Value = dic.Value})
                },
                new ColumnChartData
                {
                    Name = "出站",
                    DataModels = outDic.Where(dic => dic.Key <= DateTime.Now).Select(dic => new LableValueModel{Lable = $"{dic.Key.Hour}:00", Value = dic.Value})
                }
            };

            return JsonConvert.SerializeObject(data);

        }

        [HttpGet("GetZhuJiangNewTownInOutPassengerFlow")]
        public string ZhuJiangNewTownInOutPassengerFlow()
        {
            var data = new LineChartData[]
            {
                new LineChartData
                {
                    Name = "进站",
                    DataModels = GeneratePassengerFlow().Where(dic => dic.Key <= DateTime.Now).Select(dic=>new DateValueModel(dic.Key, dic.Value))
                },
                new LineChartData
                {
                    Name = "出站",
                    DataModels = GeneratePassengerFlow().Where(dic => dic.Key <= DateTime.Now).Select(dic=>new DateValueModel(dic.Key, dic.Value))
                },
            };
            return JsonConvert.SerializeObject(data);
        }

        [HttpGet("LayoutList")]
        public async Task<IEnumerable<LayoutDto>> LayoutList()
        {
            var datas = await ServiceFactory.GetLayoutService().GetLayouts();

            var result = datas.Select(x => new LayoutDto(x));

            return result;
        }

        [HttpGet("ServiceInfoList")]
        public async Task<IEnumerable<ServiceInfo>> GetServiceInfos([FromQuery] params string[] name)
        {
            return await _layoutService.GetServiceInfos(name);
        }

        [HttpPost("ServiceInfoList")]
        public IEnumerable<ServiceInfo> UpdateServiceInfoList([FromBody] UpdateServiceInfoRequest request)
        {
            return _layoutService.UpdateServiceInfos(request.ReturnUpdateData, request.Datas);
        }

        [HttpDelete("ServiceInfoList")]
        public IActionResult DeleteServiceInfos([FromQuery] params string[] name)
        {
            if (name.Count(x => !string.IsNullOrWhiteSpace(x)) == 0)
            {
                return BadRequest("缺少参数");
            }
            else
            {
                _layoutService.RemoveServiceInfosFor(name);
                return Ok();
            }
        }
        [HttpGet("ServiceInfoListFor")]
        public async Task<IEnumerable<ServiceInfo>> GetServiceInfosFor(string service, string function)
        {
            var result = Array.Empty<ServiceInfo>();
            if (string.IsNullOrWhiteSpace(service) && string.IsNullOrWhiteSpace(function))
            {
                result = await _layoutService.GetServiceInfos();
            }
            else if (!string.IsNullOrWhiteSpace(service) && !string.IsNullOrWhiteSpace(function))
            {
                result = await _layoutService.GetServiceInfos(x => service.Trim() == x.Service && function.Trim() == x.Function);
            }
            else if (!string.IsNullOrWhiteSpace(function))
            {
                result = await _layoutService.GetServiceInfos(x => function.Trim() == x.Function);
            }
            else if (!string.IsNullOrWhiteSpace(service))
            {
                result = await _layoutService.GetServiceInfos(x => service.Trim() == x.Service);
            }
            return result;
        }

        [HttpGet("Test1")]
        public IEnumerable<string> GetTest1()
        {
            var result = new List<string>();

            for (int i = 1; i <= 10; i++)
            {
                result.Add($"测试列表A-{i}");
            }

            return result;
        }

        [HttpGet("Test2")]
        public IEnumerable<string> GetTest2()
        {
            var result = new List<string>();
            for (int i = 1; i < 7; i++)
            {
                result.Add($"测试列表B-{i}");
            }

            return result;
        }
    }
}
