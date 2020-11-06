using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
        public string GetConnectionString()
        {
            try
            {
                return _sqlSugarClient.Ado.Connection.ConnectionString;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查询数据库连接字段失败");
            }
            return "查询数据库连接字段失败";
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [HttpPost("CommandSQLString")]
        public object CommandSQLString(string sql)
        {
            try
            {
                return _sqlSugarClient.Ado.GetDataTable(sql).Columns.Count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 创建种子数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreateDataBase")]
        public void CreateDataBase()
        {
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"创建数据库失败");
            }
        }

        /// <summary>
        /// 获取数据库表实体
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetTableClass")]
        public void GetTableClass()
        {
            foreach (var item in _sqlSugarClient.DbMaintenance.GetTableInfoList())
            {
                string entityName = item.Name;
                _sqlSugarClient.MappingTables.Add(entityName, item.Name);
                foreach (var col in _sqlSugarClient.DbMaintenance.GetColumnInfosByTableName(item.Name))
                {
                    _sqlSugarClient.MappingColumns.Add(col.DbColumnName, col.DbColumnName, entityName);
                }
            }
            _sqlSugarClient.DbFirst.IsCreateAttribute().CreateClassFile("D:\\Demo", "Models");
        }

    }
}
