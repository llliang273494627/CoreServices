using GXVCU.Services;
using GXVCU.Tasks;
using GXVCU.Tasks.QuartzNet.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
