using BackServices.Common;
using BackServices.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BackServices.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 健康检查接口
        /// </summary>
        /// <returns></returns>
        [HttpGet("HealthCheck")]
        public MessageModel<string> HealthCheck()
        {
            return new MessageModel<string>
            {
                Success = true,
                Msg = "健康检查",
                Response= "HealthCheck",
            };
        }

       

    }
}
