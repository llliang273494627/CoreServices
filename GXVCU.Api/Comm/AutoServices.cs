using GXVCU.Common.DB;
using GXVCU.Services;
using GXVCU.Tasks;
using GXVCU.Tasks.QuartzNet.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Quartz.Spi;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace GXVCU.Api.Comm
{
    public static class AutoServices
    {
        /// <summary>
        /// 添加Swagger服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwaggerSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // 添加Swagger服务
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("V1", new OpenApiInfo
                {
                    Version = "V1",
                    Description = "Dbs：数据库接口，TasksQz：定时任务接口，LogInfo：日志接口",
                    Title = "第一版本接口文档",
                    //License = new OpenApiLicense { Name = "V1" + " 官方文档", Url = new Uri("http://apk.neters.club/.doc/") },
                    Contact = new OpenApiContact { Name = "V1 前端", Url = new Uri("http://192.168.0.100:8088/") },
                });
                c.SwaggerDoc("V2", new OpenApiInfo());
                c.OrderActionsBy(o => o.RelativePath);
                // 开启加权小锁
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
            });
        }

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
            services.AddTransient<LogDetailsServices>();
            
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
