using System;
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

    }
}
