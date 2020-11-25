using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GXVCU.Api.Comm;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GXVCU.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    var setting = new Appsettings(Directory.GetCurrentDirectory());
                    if (string.IsNullOrEmpty(setting.UseUrls) == false)
                        webBuilder.UseUrls(setting.UseUrls);

                    // 添加log4net日志
                    webBuilder.ConfigureLogging(c =>
                    {
                        // 过滤类别
                        c.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.None);
                        c.AddFilter("Microsoft.AspNetCore.HttpsPolicy.HttpsRedirectionMiddleware", LogLevel.None);
                        c.AddFilter("Quartz.Impl.StdSchedulerFactory", LogLevel.None);
                        c.AddFilter("Quartz.Core.SchedulerSignalerImpl", LogLevel.None);
                        c.AddFilter("Quartz.Core.QuartzScheduler", LogLevel.None);
                        // 添加控制台输出
                        c.AddConsole();
                        c.AddLog4Net();
                    });
                });
    }
}
