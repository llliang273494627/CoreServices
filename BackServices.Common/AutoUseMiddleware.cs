using BackServices.Common.Helper;
using BackServices.Common.SettingEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BackServices.Common
{
    /// <summary>
    /// 全局监控
    /// </summary>
    public class AutoUseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AutoUseMiddleware> _logger;
        private readonly HelperAppsettings _appsettings;

        public AutoUseMiddleware(RequestDelegate next, ILogger<AutoUseMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _appsettings = new HelperAppsettings(Directory.GetCurrentDirectory());
        }

        public static string Host { get; set; }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                Host= $"http://{ context.Request.Host}/";
                var middleware = _appsettings.GetNodeEntity<EntityMiddleware>("Middleware");
                if (middleware != null && middleware.Enabled)
                {
                    string[] apis = middleware.IgnoreApis.Split(',');
                    foreach (string itme in apis)
                    {
                        if (context.Request.Path.Value.ToLower().Contains(itme))
                        {
                            context.Request.EnableBuffering();
                            Stream originalBody = context.Response.Body;

                            var request = context.Request;
                            var url = $"{ request.Host }{ request.Path}，{ request.Method}";
                            var queryStr = request.QueryString;
                            var bodyStr = await new StreamReader(request.Body).ReadToEndAsync();

                            StringBuilder builder = new StringBuilder();
                            builder.Append($"Url：http://{url}");
                            if (!string.IsNullOrEmpty(queryStr.Value))
                                builder.Append($"\r\nParameters：{queryStr}");
                            if (!string.IsNullOrEmpty(bodyStr))
                                builder.Append($"\r\nBody：{bodyStr}");

                            request.Body.Position = 0;
                            using (var ms = new MemoryStream())
                            {
                                context.Response.Body = ms;
                                await _next(context);
                                ms.Position = 0;
                                await ms.CopyToAsync(originalBody);
                            }
                            _logger.LogWarning(builder.ToString());
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

    }
}
