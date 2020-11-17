﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TestWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EfficientDesignApiController : ControllerBase
    {
        private readonly ILogger<EfficientDesignApiController> _logger;

        public EfficientDesignApiController(ILogger<EfficientDesignApiController> logger)
        {
            _logger = logger;
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
    }
}
