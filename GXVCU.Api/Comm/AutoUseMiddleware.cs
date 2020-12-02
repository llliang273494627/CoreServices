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
                            var request = context.Request;
                            var url = $"{ request.Host }{ request.Path}，{ request.Method}";
                            var queryStr = request.QueryString;
                            var content = $"QueryData：{url}\r\n QueryData：{queryStr}";
                            break;
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

    }
}
