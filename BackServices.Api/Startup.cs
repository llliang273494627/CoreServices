using System;
using System.Threading.Tasks;
using BackServices.Api.Comm;
using BackServices.Common;
using BackServices.Common.DB;
using BackServices.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace BackServices.Api
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

            // ����Swagger����
            services.AddSwaggerSetup();
            // ͨ�������
            services.AddPublicClassService(Configuration);
            // �������ݿ����
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
                                    Console.WriteLine("��SQL��䡿��" + sql);
                                    if (_logger != null)
                                    {
                                        string key = "��SQL��������";
                                        foreach (var param in p)
                                        {
                                            key += $"{param.ParameterName}:{param.Value}\n";
                                        }
                                        _logger.LogInformation(key + "��SQL��䡿��" + sql);
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
                    // �ӿ�
                    //SlaveConnectionConfigs = listConfig_Slave,
                    // �Զ�������
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
            // �����Զ��������
            services.AddTableService();
            // �����������
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

            // ����ȫ�ּ��(���������㣬�����ܷ���)
            app.UseMiddleware<AutoUseMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            _logger = logger;
            // ����Swagger
            app.UseSwaggerMildd();
            // ������������
            app.UseSeedDataMildd(dBSeed);
            // ����QuartzNetJob���ȷ���
            app.UseQuartzJobMildd(logger, dBSeed, schedulerCenter);
            
        }
    }
}