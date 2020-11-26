using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GXVCU.Api.Comm
{
    /// <summary>
    /// 全局监控
    /// </summary>
    public class AutoUseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AutoUseMiddleware> _logger;
        private readonly Appsettings _appsettings;

        public AutoUseMiddleware(RequestDelegate next, ILogger<AutoUseMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _appsettings = new Appsettings(Directory.GetCurrentDirectory());
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (_appsettings.Middleware.Enabled)
                {
                    string[] apis = _appsettings.Middleware.IgnoreApis.Split(',');
                    foreach (string itme in apis)
                    {
                        if (context.Request.Path.Value.ToLower().Contains(itme))
                        {
                            // 存储请求数据
                            await RequestDataLog(context);
                            return;
                        }
                    }
                }
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "接口请求异常！");
            }
        }

        /// <summary>
        /// 请求信息写入日志
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task RequestDataLog(HttpContext context)
        {
            var request = context.Request;
            var sr = new StreamReader(request.Body);
            var StrBody = await sr.ReadToEndAsync();
            var content = 
                $"QueryData：{request .Host}{request .Path}，{request .Method}\r\n" +
                $" BodyData：{StrBody}\r\n" +
                $"QueryData：{request.QueryString}";
            if (!string.IsNullOrEmpty(content))
            {
                _logger.LogWarning(content);
            }
        }

      
    }
}
