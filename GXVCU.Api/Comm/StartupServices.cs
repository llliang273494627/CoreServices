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
            try
            {
                var m = new Appsettings(configuration).MainDB.CurrentDb();
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
                        InitKeyType = InitKeyType.Attribute,
                    });
                });
                Console.WriteLine("启动 SqlSugar 服务");
                log.Info("启动 SqlSugar 服务");
            }
            catch (Exception ex)
            {
                log.Error("启动 SqlSugar 服务失败", ex);
            }
            
        }

        /// <summary>
        /// 添加操作服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddCommSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            try
            {
                services.AddScoped(o => { return new Appsettings(configuration); });
                Console.WriteLine("启动配置服务");
                log.Info("启动配置服务");
            }
            catch (Exception ex)
            {
                Console.WriteLine("启动配置服务失败");
                log.Error("启动配置服务",ex);
            }
            
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="services"></param>
        public static void AddJobSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            try
            {
                // 任务调度
                services.AddSingleton<IJobFactory, JobFactory>();
                services.AddSingleton<SchedulerCenterServer>();
                services.AddTransient<Job_Blogs_Quartz>();

                Console.WriteLine("添加定时任务");
                log.Info("添加定时任务");
            }
            catch (Exception ex)
            {
                Console.WriteLine("添加定时任务失败");
                log.Info("添加定时任务失败",ex);
            }
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
                log.Error("添加程序集依赖注入失败", ex);
            }
            
            
        }

    }
}
