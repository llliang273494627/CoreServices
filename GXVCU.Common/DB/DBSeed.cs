using GXVCU.Common.FromBodys;
using GXVCU.Model.Models;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GXVCU.Common.DB
{
    public class DBSeed
    {
        private readonly ILogger<DBSeed> _logger;
        private readonly SqlSugarClient _sqlSugarClient;

        public DBSeed(ILogger<DBSeed> logger, SqlSugarClient sqlSugarClient)
        {
            _logger = logger;
            _sqlSugarClient = sqlSugarClient;
        }

        /// <summary>
        /// 添加种子数据
        /// </summary>
        /// <returns></returns>
        public MessageModel<string> CreateDataBase()
        {
            var data = new MessageModel<string>();
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
                _logger.LogInformation(data.Msg);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建数据库失败");
                data.Success = false;
                data.Msg = ex.Message;
            }
            return data;
        }

        /// <summary>
        /// 获取数据库表实体
        /// </summary>
        /// <param name="tableEntity"></param>
        /// <returns></returns>
        public MessageModel<string> GetTableClass(TableEntity tableEntity)
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
                    _logger.LogInformation($"添加{item.Name}表实体");
                }
                _sqlSugarClient.DbFirst.IsCreateAttribute().CreateClassFile(tableEntity.DirectoryPath, tableEntity.NameSpace);

                data.Success = true;
                data.Msg = "数据库表实体生成完成";
                data.Response = tableEntity.DirectoryPath;
                _logger.LogInformation(data.Msg);
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
