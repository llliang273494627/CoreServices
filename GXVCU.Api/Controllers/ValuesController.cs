using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using GXVCU.Api.Comm;
using GXVCU.Common;
using GXVCU.Common.FromBodys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SqlSugar;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GXVCU.Api.Controllers
{
    [Route("api/values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 测试写入日志文件
        /// </summary>
        /// <param name="logEntity"></param>
        [HttpPost("logger")]
        public void Logger([FromBody] LogEntity logEntity)
        {
            _logger.LogInformation("LogInformation :" + logEntity.Messager);
            _logger.LogError("LogError msg:" + logEntity.Messager);
        }

    }
}
