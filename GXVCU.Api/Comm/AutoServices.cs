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

            // 不同http清求，实例不同，同名谓词不同，也不行。
            // 例如httpget跟httppost,作用域是一定范围内，
            // 例如从同一个post请求的create方法，只能统计一次，每次请求都是新的实例
            services.AddScoped<DBSeed>();
            // 临时服务，每次请求时会创建一个新的服务
            services.AddTransient<TasksQzServices>();
            
        }

        /// <summary>
        /// 添加任务服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddJobSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // 首次创建(单例模式)
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<Job_Blogs_Quartz>();
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
