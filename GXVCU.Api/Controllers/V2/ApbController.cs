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
        private readonly ISqlSugarClient _sqlSugarClient;
        private readonly ILogger<ApbController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApbController(ISqlSugarClient sqlSugarClient,ILogger<ApbController> logger)
        {
            _sqlSugarClient = sqlSugarClient;
            _logger = logger;
        }

        [HttpGet]
        [CustomRoute(ApiVersions.V2, "apbs")]
        public IEnumerable<string> Get()
        {
            return new string[] { "第二版的 apbs" };
        }

        [HttpPost]
        [CustomRoute(ApiVersions.V2, "GetMTOC")]
        public MessageModel<object> GetMTOC()
        {
            MessageModel<object> rep = new MessageModel<object>();
            rep.Success = false;
            try
            {
                rep.Response = _sqlSugarClient.Queryable<MTOC>().ToList();
                rep.Success = true;
            }
            catch (Exception ex)
            {
                rep.Msg = ex.Message; 
                _logger.LogError(ex, "数据库查询失败");
            }
            return rep;
        }

       
    }
}
