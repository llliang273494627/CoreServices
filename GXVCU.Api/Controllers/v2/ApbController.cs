using System;
using System.Collections.Generic;
using GXVCU.Api.Comm;
using GXVCU.Common;
using GXVCU.Common.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GXVCU.Api.Controllers.v2
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApbController : ControllerBase
    {
        private readonly ILogger<ApbController> _logger;

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

        /// <summary>
        /// 打印测试
        /// </summary>
        /// <param name="entityBitmap"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomRoute(ApiVersions.V2, "Print")]
        public MessageModel<string> Print([FromBody] EntityBitmapLHGQ entityBitmap)
        {
            var data = new MessageModel<string>();
            try
            {
                var print = new HelperPrintDocument();
                print.Print(entityBitmap);
                data.Response = "已经发出打印请求！";
                data.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                data.Success = false;
                data.Response = "打印失败！";
                data.Msg = ex.Message;
            }
            return data;
        }

    }
}
