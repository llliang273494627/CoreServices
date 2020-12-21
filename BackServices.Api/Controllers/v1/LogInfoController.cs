using System;
using System.Threading.Tasks;
using BackServices.Api.Comm;
using BackServices.Common;
using BackServices.Model.Models;
using BackServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BackServices.Api.Controllers
{
    /// <summary>
    /// 日志信息
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LogInfoController : ControllerBase
    {
        private readonly ILogger<LogInfoController> _logger;
        private readonly LogDetailsServices _logDetails;

        public LogInfoController(ILogger<LogInfoController> logger, LogDetailsServices logDetails)
        {
            _logger = logger;
            _logDetails = logDetails;
        }

        /// <summary>
        /// 添加日志记录
        /// </summary>
        /// <param name="logDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomRoute(ApiVersions.V1, "LogInformation")]
        public async Task<MessageModel<Exception>> LogInformation([FromBody] LogDetails logDetails)
        {
            var loginfo = new MessageModel<Exception>();
            try
            {
                await _logDetails.LogInformation(logDetails);
                loginfo.Success = true;
                loginfo.Msg = "添加成功";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "添加日志记录异常");
                loginfo.Success = false;
                loginfo.Msg = "添加失败";
                loginfo.Response = ex;
            }
            return loginfo;
        }

        /// <summary>
        /// 添加错误日志记录
        /// </summary>
        /// <param name="logDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomRoute(ApiVersions.V1, "LogError")]
        public async Task<MessageModel<Exception>> LogError([FromBody] LogDetails logDetails)
        {
            var loginfo = new MessageModel<Exception>();
            try
            {
                await _logDetails.LogError(logDetails);
                loginfo.Success = true;
                loginfo.Msg = "添加成功";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "添加日志记录异常");
                loginfo.Success = false;
                loginfo.Msg = "添加失败";
                loginfo.Response = ex;
            }
            return loginfo;
        }

    }
}
