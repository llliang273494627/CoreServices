using System;
using System.Threading.Tasks;
using GXVCU.Api.Comm;
using GXVCU.Common.DB;
using GXVCU.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SqlSugar;
using Swashbuckle.AspNetCore.Filters;

namespace GXVCU.Api
{
    public class Startup
    {
        private static ILogger<Startup> _logger;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // 添加Swagger服务
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("V1", new OpenApiInfo());
                c.SwaggerDoc("V2", new OpenApiInfo());
                c.OrderActionsBy(o => o.RelativePath);
                // 开启加权小锁
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
            });
            // 通用类服务
            services.AddPublicClassService(Configuration);
            // 添加数据库服务
            services.AddScoped(o => {
                var maindb = new Appsettings(Configuration).MainDB;
                var mutiDB = maindb.CurrentDb;
                var db = new SqlSugarClient(new ConnectionConfig()
                {
                    ConfigId = mutiDB.ConnId.ObjToString().ToLower(),
                    ConnectionString = mutiDB.Connection,
                    DbType = (DbType)mutiDB.DbType,
                    IsAutoCloseConnection = true,
                    IsShardSameThread = false,
                    AopEvents = new AopEvents
                    {
                        OnLogExecuting = (sql, p) =>
                        {
                            if (maindb.SqlAOPEnabled)
                            {
                                Parallel.For(0, 1, e =>
                                {
                                    Console.WriteLine("【SQL语句】：" + sql);
                                    if (_logger != null)
                                    {
                                        string key = "【SQL参数】：";
                                        foreach (var param in p)
                                        {
                                            key += $"{param.ParameterName}:{param.Value}\n";
                                        }
                                        _logger.LogInformation(key + "【SQL语句】：" + sql);
                                    }
                                });
                            }
                        }
                    },
                    MoreSettings = new ConnMoreSettings()
                    {
                        //IsWithNoLockQuery = true,
                        IsAutoRemoveDataCache = true
                    },
                    // 从库
                    //SlaveConnectionConfigs = listConfig_Slave,
                    // 自定义特性
                    ConfigureExternalServices = new ConfigureExternalServices()
                    {
                        EntityService = (property, column) =>
                        {
                            if (column.IsPrimarykey && property.PropertyType == typeof(int))
                            {
                                column.IsIdentity = true;
                            }
                        }
                    },
                    InitKeyType = InitKeyType.Attribute,
                });
                return db;
            });
            // 添加自定义表服务
            services.AddTableService();
            // 添加任务服务
            services.AddJobSetup();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger,
            DBSeed dBSeed, SchedulerCenterServer schedulerCenter)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            _logger = logger;
            // 添加Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("swagger/V1/swagger.json", "My V1 Api");
                c.SwaggerEndpoint("swagger/V2/swagger.json", "My V2 Api");
                c.RoutePrefix = "";
            });
            // 添加种子数据
            app.UseSeedDataMildd(dBSeed);
            // 开启QuartzNetJob调度服务
            app.UseQuartzJobMildd(logger, dBSeed, schedulerCenter);
        }
    }
}
