using System;
using System.Collections.Generic;
using GXVCU.Api.Filter;
using GXVCU.Common;
using GXVCU.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace GXVCU.Api.Controllers.V2
{
    [Route("api/apb")]
    [ApiController]
    public class ApbController : ControllerBase
    {
        private readonly ILogger<ApbController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApbController(ILogger<ApbController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [CustomRoute(ApiVersions.V2, "apbs")]
        public IEnumerable<string> Get()
        {
            return new string[] { "第二版的 apbs" };
        }

        

       
    }
}
