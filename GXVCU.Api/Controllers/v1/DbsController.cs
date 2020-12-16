using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GXVCU.Api.Comm;
using GXVCU.Common;
using GXVCU.Common.DB;
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
        private readonly DBSeed _dBSeed;

        public DbsController(ILogger<DbsController> logger,SqlSugarClient sqlSugarClient, DBSeed dBSeed)
        {
            _logger = logger;
            _sqlSugarClient = sqlSugarClient;
            _dBSeed = dBSeed;
        }

        /// <summary>
        /// 查看数据库连接字符串
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustomRoute(ApiVersions.V1, "GetConnectionString")]
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
        [HttpPost]
        [CustomRoute(ApiVersions.V1, "CommandSQLString")]
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
        [HttpPost]
        [CustomRoute(ApiVersions.V1, "CreateDataBase")]
        public MessageModel<string> CreateDataBase()
        {
            return _dBSeed.CreateDataBase();
        }

        /// <summary>
        /// 创建指定表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustomRoute(ApiVersions.V1, "CreateAppointDataBase")]
        public MessageModel<string> CreateAppointDataBase([FromBody] BodyTableSpace entity)
        {
            return _dBSeed.CreateAppointDataBase(entity.NameSpace);
        }

        /// <summary>
        /// 获取数据库表实体
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustomRoute(ApiVersions.V1, "GetTableClass")]
        public MessageModel<string> GetTableClass([FromBody] TableEntity tableEntity)
        {
            return _dBSeed.GetTableClass(tableEntity);
        }

    }
}
