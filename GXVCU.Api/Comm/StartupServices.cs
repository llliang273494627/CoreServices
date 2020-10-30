using GXVCU.Tasks;
using GXVCU.Tasks.QuartzNet.Jobs;
using log4net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Simpl;
using Quartz.Spi;
using SqlSugar;
using System;
using System.Linq;
using System.Reflection;

namespace GXVCU.Api.Comm
{
    public static class StartupServices
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StartupServices));

        /// <summary>
        /// SqlSugar 启动服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddSqlsugarSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var m =new Appsettings(configuration).MainDB.CurrentDb();
            // 把多个连接对象注入服务，这里必须采用Scope，因为有事务操作
            services.AddScoped<ISqlSugarClient>(o =>
            {
                return new SqlSugarClient(new ConnectionConfig()
                {
                    ConfigId = m.ConnId.ObjToString().ToLower(),
                    ConnectionString = m.Connection,
                    DbType = (SqlSugar.DbType)m.DbType,
                    IsAutoCloseConnection = true,
                    IsShardSameThread = true,
                    InitKeyType=InitKeyType.Attribute,
                });
            });
        }

        /// <summary>
        /// 添加操作服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddCommSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped(o => { return new Appsettings(configuration); });
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="services"></param>
        public static void AddJobSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // 任务调度
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<SchedulerCenterServer>();
            services.AddTransient<Job_Blogs_Quartz>();

        }

        /// <summary>
        /// 添加 services 程序集服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddServicesSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            try
            {
                // 任务调度
                var assembly = Assembly.Load(new AssemblyName("GXVCU.Services"));
                assembly.GetTypes().ToList().ForEach( c => {
                    services.TryAddTransient(c);
                } );
                
            }
            catch (Exception ex)
            {
                log.Error("添加程序集失败", ex);
            }
            
            
        }

    }
}
