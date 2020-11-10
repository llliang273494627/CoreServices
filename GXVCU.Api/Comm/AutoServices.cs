using GXVCU.Common.DB;
using GXVCU.Common.Helper;
using GXVCU.Services;
using GXVCU.Tasks;
using GXVCU.Tasks.QuartzNet.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using System;

namespace GXVCU.Api.Comm
{
    public static class AutoServices
    {
        /// <summary>
        /// 添加表操作服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddTableService(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<DBSeed>();
            services.AddTransient<TasksQzServices>();
            
        }

        /// <summary>
        /// 添加任务服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddJobSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //services.AddHostedService<Job1TimedService>();
            //services.AddHostedService<Job2TimedService>();

            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddTransient<Job_Blogs_Quartz>();//Job使用瞬时依赖注入
            services.AddSingleton<SchedulerCenterServer>();
        }

        /// <summary>
        /// 通用类服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddPublicClassService(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // 添加配置参数
            services.AddScoped(o => { return new Appsettings(configuration); });
        }
    }
}
