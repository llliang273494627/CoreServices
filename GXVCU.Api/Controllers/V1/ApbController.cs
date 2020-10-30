using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GXVCU.Api.Comm;
using GXVCU.Api.Filter;
using GXVCU.Common;
using GXVCU.Common.DB;
using GXVCU.Common.FromBodys;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace GXVCU.Api.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApbController : ControllerBase
    {
        private readonly SqlSugarClient _sqlSugarClient;
        private readonly IWebHostEnvironment Env;
        private readonly ILogger<ApbController> _logger;
        private readonly Appsettings _appsettings;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApbController(ISqlSugarClient sqlSugarClient, IWebHostEnvironment env,ILogger<ApbController> logger,
            Appsettings appsettings )
        {
            _sqlSugarClient = sqlSugarClient as SqlSugarClient;
            Env = env;
            _logger = logger;
            _appsettings = appsettings;
        }

        [HttpGet]
        [CustomRoute(ApiVersions.V1, "apbs")]
        public IEnumerable<string> Get()
        {
            return new string[] { "第一版的 apbs" };
        }

        /// <summary>
        /// 测试数据库连接
        /// </summary>
        /// <param name="mutiDB"></param>
        [HttpPost]
        [CustomRoute(ApiVersions.V1, "connection")]
        public MessageModel<object> Connection()
        {
            MessageModel<object> msg = new MessageModel<object>();
            try
            {
                var mutiDB = _appsettings.MainDB.CurrentDb();
                var db = new SqlSugarClient(new ConnectionConfig()
                {
                    ConfigId = mutiDB.ConnId.ObjToString().ToLower(),
                    ConnectionString = mutiDB.Connection,
                    DbType = (SqlSugar.DbType)mutiDB.DbType,
                    IsAutoCloseConnection = true,
                    IsShardSameThread = true,
                });
                msg.Response = db.DbMaintenance.GetTableInfoList();
                msg.Success = true;
            }
            catch (Exception ex)
            {
                msg.Success = false;
                msg.Msg = ex.Message;
                _logger.LogError(ex.StackTrace);
            }
            return msg;
        }

        /// <summary>
        /// 获取所有表实体
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustomRoute(ApiVersions.V1, "createtablesentity")]
        public MessageModel<object> CreateTablesEntity([FromBody] MainDBEntity dBEntity)
        {
            MessageModel<object> data = new MessageModel<object>();
            data.Success = false;
            if (Env.IsDevelopment())
            {
                try
                {
                    if (string.IsNullOrEmpty(dBEntity.DirectoryPath) || dBEntity.DirectoryPath.ToLower() == "string")
                        dBEntity.DirectoryPath = @"D:\my-file\GXVCU.Model";
                    if (string.IsNullOrEmpty(dBEntity.NameSpace) || dBEntity.NameSpace.ToLower() == "string")
                        dBEntity.NameSpace = "GXVCU.Model.Models";
                    foreach (var item in _sqlSugarClient.DbMaintenance.GetTableInfoList())
                    {
                        _sqlSugarClient.MappingTables.Add(item.Name, item.Name);
                        foreach (var col in _sqlSugarClient.DbMaintenance.GetColumnInfosByTableName(item.Name))
                        {
                            _sqlSugarClient.MappingColumns.Add(col.DbColumnName, col.DbColumnName, item.Name);
                        }
                    }
                    _sqlSugarClient.DbFirst.IsCreateAttribute().CreateClassFile(dBEntity.DirectoryPath, dBEntity.NameSpace);
                    data.Success = true;
                    data.Msg = dBEntity.DirectoryPath;
                }
                catch (Exception ex)
                {
                    data.Success = false;
                    data.Msg = ex.Message;
                    _logger.LogError(ex.StackTrace);
                }
            }
            return data;
        }

        /// <summary>
        /// 创建种子数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustomRoute(ApiVersions.V1, "createdatabase")]
        public MessageModel<string> CreateDataBase([FromBody] MainDBTables dBTables)
        {
            MessageModel<string> data = new MessageModel<string>();
            data.Success = false;
            if (Env.IsDevelopment())
            {
                try
                {
                    if (string.IsNullOrEmpty(dBTables.UsingPath) || dBTables.UsingPath.ToLower() == "string")
                        dBTables.UsingPath = "GXVCU.Model";
                    if (string.IsNullOrEmpty(dBTables.NameSpace) || dBTables.NameSpace.ToLower() == "string")
                        dBTables.NameSpace = "GXVCU.Model.Models";
                    // 创建数据库
                    bool isOK = _sqlSugarClient.DbMaintenance.CreateDatabase();
                    string msg = "数据库创建：" + isOK.ToString();
                    Console.WriteLine(msg);
                    var modelTypes = from t in Assembly.Load(dBTables.UsingPath).GetTypes()
                                     where t.IsClass && t.Namespace == dBTables.NameSpace
                                     select t;
                    modelTypes.ToList().ForEach(t =>
                    {
                        if (!_sqlSugarClient.DbMaintenance.IsAnyTable(t.Name))
                        {
                            Console.WriteLine(t.Name);
                            _sqlSugarClient.CodeFirst.InitTables(t);
                        }
                    });
                    data.Success = true;
                }
                catch (Exception ex)
                {
                    data.Success = false;
                    data.Response = ex.Message;
                    _logger.LogError(ex.StackTrace);
                }
            }
            return data;
        }

    }
}
