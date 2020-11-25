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

                    // ���log4net��־
                    webBuilder.ConfigureLogging(c =>
                    {
                        // �������
                        c.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.None);
                        c.AddFilter("Microsoft.AspNetCore.HttpsPolicy.HttpsRedirectionMiddleware", LogLevel.None);
                        c.AddFilter("Quartz.Impl.StdSchedulerFactory", LogLevel.None);
                        c.AddFilter("Quartz.Core.SchedulerSignalerImpl", LogLevel.None);
                        c.AddFilter("Quartz.Core.QuartzScheduler", LogLevel.None);
                        // ��ӿ���̨���
                        c.AddConsole();
                        c.AddLog4Net();
                    });
                });
    }
}
