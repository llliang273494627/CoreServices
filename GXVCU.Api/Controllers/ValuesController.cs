using GXVCU.Common;
using Microsoft.AspNetCore.Mvc;

namespace GXVCU.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
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
