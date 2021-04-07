﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TestWebApi.Dto;

namespace TestWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RxDemoApiController : ControllerBase
    {
        public RxDemoApiController(ILogger<RxDemoApiController> logger)
        {

        }

        [HttpGet("Login")]
        public ActionResult<Dictionary<string,string>> Login([FromBody] LoginRequest request)
        {
            if (request.Username.Trim() != "admin" || request.Password.Trim() != "admin")
            {
                return NotFound("账号或密码错误");
            }
            var dic = new Dictionary<string, string> { { "userId", "001" } };

            return dic;
        }
    }
}
