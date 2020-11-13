using GXVCU.Common.FromBodys;
using GXVCU.Model.Models;
using Microsoft.Extensions.Logging;
using SQLitePCL;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
        /// 创建数据库
        /// </summary>
        /// <returns></returns>
        public MessageModel<string> CreateDataBase()
        {
            var data = new MessageModel<string>() { Success=false,};
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
        /// 添加种子数据
        /// </summary>
        /// <param name="dirPath">种子数据文件夹</param>
        /// <returns></returns>
        public MessageModel<string> CreateDataInfo(string path = "")
        {
            var data = new MessageModel<string>() { Success = false, };
            try
            {
                // 添加初始数据
                if (Directory.Exists(path) == false)
                {
                    _logger.LogInformation("文件夹不存在：" + path);
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "GXVCU.Data.json");
                    _logger.LogInformation("使用默认文件：" + path);
                }
                foreach (var item in Directory.GetFiles(path))
                {
                    var res = Helper.FileHelper.ReadFileTextAll(item);
                    if (res.Success == false)
                    {
                        _logger.LogError(res.Response, $"{res.Msg}:{item}");
                        continue;
                    }
                    var jsonStr = res.Msg;
                    var fileName = new DirectoryInfo(item).Name.Split('.')[0];
                    switch (fileName)
                    {
                        case "TasksQz":
                            if (_sqlSugarClient.Queryable<TasksQz>().Any() == false)
                            {
                                var tasksQzs = Helper.FormatHelper.JsonToObj<List<TasksQz>>(jsonStr);
                                _sqlSugarClient.Insertable(tasksQzs).ExecuteCommand();
                                _logger.LogInformation("添加 TasksQz 表初始数据");
                                Console.WriteLine("添加 TasksQz 表初始数据");
                            }
                            break;
                    }
                }
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

        public MessageModel<List<T>> GetTable<T>()
        {
            return new MessageModel<List<T>>()
            {
                Success = true,
                Response = _sqlSugarClient.Queryable<T>().ToList(),
            };
        }
    }
}
