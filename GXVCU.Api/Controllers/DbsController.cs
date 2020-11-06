using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GXVCU.Common;
using GXVCU.Common.FromBodys;
using GXVCU.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SqlSugar;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GXVCU.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DbsController : ControllerBase
    {
        private readonly ILogger<DbsController> _logger;
        private readonly SqlSugarClient _sqlSugarClient;

        public DbsController(ILogger<DbsController> logger,SqlSugarClient sqlSugarClient)
        {
            _logger = logger;
            _sqlSugarClient = sqlSugarClient;
        }

        /// <summary>
        /// 查看数据库连接字符串
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetConnectionString")]
        public MessageModel<string> GetConnectionString()
        {
            var data = new MessageModel<string>();
            try
            {
                data.Response = _sqlSugarClient.Ado.Connection.ConnectionString;
                data.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查询数据库连接字段失败");
                data.Success = false;
                data.Msg = "查询数据库连接字段失败";
            }
            return data;
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [HttpPost("CommandSQLString")]
        public MessageModel<int> CommandSQLString(string sql)
        {
            var data = new MessageModel<int>();
            try
            {
                data.Response = _sqlSugarClient.Ado.GetDataTable(sql).Columns.Count;
                data.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                data.Success = false;
                data.Msg = ex.Message;
            }
            return data;
        }

        /// <summary>
        /// 创建种子数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreateDataBase")]
        public MessageModel<string> CreateDataBase()
        {
            var data =new MessageModel<string>();
            try
            {
                _sqlSugarClient.DbMaintenance.CreateDatabase();
                _logger.LogInformation("创建数据库");
                //var modelTypes = from t in Assembly.GetExecutingAssembly().GetTypes()
                //                 where t.IsClass && t.Namespace == "GXVCU.Model.Models"
                //                 select t;
                var modelTypes = from t in Assembly.GetAssembly(typeof(TasksQz)).GetTypes()
                                 where t.IsClass && t.Namespace == "GXVCU.Model.Models"
                                 select t;
                _logger.LogInformation("获取数据库表实体数：" + modelTypes.ToList().Count);
                modelTypes.ToList().ForEach(t =>
                {
                    if (!_sqlSugarClient.DbMaintenance.IsAnyTable(t.Name))
                    {
                        Console.WriteLine(t.Name);
                        _sqlSugarClient.CodeFirst.InitTables(t);
                        _logger.LogInformation("创建表：" + t.Name);
                    }
                });
                data.Success = true;
                data.Msg = "创建数据库完成";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"创建数据库失败");
                data.Success = false;
                data.Msg = ex.Message;
            }
            return data;
        }

        /// <summary>
        /// 获取数据库表实体
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetTableClass")]
        public MessageModel<string> GetTableClass([FromBody] TableEntity tableEntity)
        {
            var data = new MessageModel<string>();
            try
            {
                if (string.IsNullOrEmpty(tableEntity.DirectoryPath) || tableEntity.DirectoryPath == "string")
                    tableEntity.DirectoryPath = "D:\\my-files";
                if (string.IsNullOrEmpty(tableEntity.NameSpace) || tableEntity.NameSpace == "string")
                    tableEntity.NameSpace = "GXVCU.Model.Models";
                foreach (var item in _sqlSugarClient.DbMaintenance.GetTableInfoList())
                {
                    string entityName = item.Name;
                    _sqlSugarClient.MappingTables.Add(entityName, item.Name);
                    foreach (var col in _sqlSugarClient.DbMaintenance.GetColumnInfosByTableName(item.Name))
                    {
                        _sqlSugarClient.MappingColumns.Add(col.DbColumnName, col.DbColumnName, entityName);
                    }
                }
                _sqlSugarClient.DbFirst.IsCreateAttribute().CreateClassFile(tableEntity.DirectoryPath, tableEntity.NameSpace);

                data.Success = true;
                data.Msg = "数据库表实体生成完成";
                data.Response = tableEntity.DirectoryPath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "数据库表实体生成异常");
                data.Success = false;
                data.Msg = ex.Message;
            }
            return data;
        }

    }
}
