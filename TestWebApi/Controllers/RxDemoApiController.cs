using System;
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

        public List<PostItem> PostItems { get; set; } = new List<PostItem>();

        public RxDemoApiController(ILogger<RxDemoApiController> logger)
        {
            PostItems.Add(new PostItem("宋归", "好想做个普通人", 0, DateTime.UtcNow, "金国太子，宋国船夫！"));
            PostItems.Add(new PostItem("青羽", "教你如何长命百岁", 6, DateTime.UtcNow, "主要靠宋归保佑"));
            PostItems.Add(new PostItem("李莫白", "间谍指南", 3, DateTime.UtcNow, "颜值高，为国当渣男。"));

        }

        [HttpPost("Login")]
        public ActionResult<Dictionary<string,string>> Login([FromBody] LoginRequest request)
        {
            if (request.Username.Trim() != "admin" || request.Password.Trim() != "admin")
            {
                return NotFound("账号或密码错误");
            }
            var dic = new Dictionary<string, string> {{ "userId", "001" }};

            return dic;
        }
        
        [HttpGet("Post")]
        public IEnumerable<PostItem> GetPosts([FromQuery]string[] title)
        {
            if (title?.Length > 0)
            {
                return PostItems.Where(x => title.Contains(x.Title));
            }
            return PostItems;
        }

        [HttpGet("Post/{title}")]
        public ActionResult<PostItem> GetPost(string title)
        {
            var post = PostItems.FirstOrDefault(x => x.Title.Contains(title));
            if (post == null) return NotFound();
            else
            {
                return post;
            }
        }
    }
}
