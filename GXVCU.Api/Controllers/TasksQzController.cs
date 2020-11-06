using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GXVCU.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace GXVCU.Api.Controllers
{
    /// <summary>
    /// 任务控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TasksQzController : ControllerBase
    {
        private readonly ILogger<TasksQzController> _logger;
        private readonly SqlSugarClient _sqlSugarClient;

        public TasksQzController(ILogger<TasksQzController> logger, SqlSugarClient sqlSugarClient)
        {
            _logger = logger;
            _sqlSugarClient = sqlSugarClient;
        }

        /// <summary>
        /// 查找所以的任务
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetJobs")]
        public List<TasksQz> GetJobs()
        {
            try
            {
                return _sqlSugarClient.Queryable<TasksQz>().ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查询所以任务失败");
            }
            return new List<TasksQz>();
        }

        /// <summary>
        /// 添加计划任务
        /// </summary>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        [HttpPost]
        public void Post([FromBody] TasksQz tasksQz)
        {
            _sqlSugarClient.Insertable(tasksQz).ExecuteCommand();
        }
    }
}
