using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GXVCU.Common;
using GXVCU.Model.Models;
using GXVCU.Services;
using GXVCU.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        private readonly TasksQzServices _tasksQz;
        private readonly SchedulerCenterServer _scheduler;

        public TasksQzController(ILogger<TasksQzController> logger, TasksQzServices tasksQz, SchedulerCenterServer scheduler)
        {
            _logger = logger;
            _tasksQz = tasksQz;
            _scheduler = scheduler;
        }

        /// <summary>
        /// 查找所以的任务记录
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetJobs")]
        public async Task<MessageModel<List<TasksQz>>> GetJobsEntity()
        {
            var data = new MessageModel<List<TasksQz>>();
            try
            {
                data.Response = await _tasksQz.GetTasks();
                data.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查询所以任务失败");
                data.Success = false;
                data.Msg = ex.Message;
            }
            return data;
        }

        /// <summary>
        /// 查找任务记录
        /// </summary>
        /// <returns></returns>
        [HttpPost("SelectJob")]
        public async Task<MessageModel<TasksQz>> SelectJobEntity(int jobId)
        {
            var data = new MessageModel<TasksQz>();
            try
            {
                data.Response = await _tasksQz.SelectJob(jobId);
                data.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查询所以任务失败");
                data.Success = false;
                data.Msg = ex.Message;
            }
            return data;
        }

        /// <summary>
        /// 添加计划任务记录
        /// </summary>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        [HttpPost("AddJob")]
        public async Task<MessageModel<int>> AddJobEntity([FromBody] TasksQz tasksQz)
        {
            var data = new MessageModel<int>();
            try
            {
                data .Response= await _tasksQz.AddTasks(tasksQz);
                data.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "添加计划任务失败");
                data.Success = false;
                data.Msg = ex.Message;
            }
            return data;
        }

        /// <summary>
        /// 修改任务记录
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdataJob")]
        public async Task<MessageModel<bool>> UpdataJobEntity([FromBody]TasksQz tasksQz)
        {
            var data = new MessageModel<bool>();
            try
            {
                data.Response = await _tasksQz.UpdataJob(tasksQz);
                data.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "修改任务失败");
                data.Success = false;
                data.Msg = ex.Message;
            }
            return data;
        }

        /// <summary>
        /// 删除任务记录
        /// </summary>
        /// <returns></returns>
        [HttpPost("DelJob")]
        public async Task<MessageModel<int>> DelJobEntity(int jobId)
        {
            var data = new MessageModel<int>();
            try
            {
                data.Response = await _tasksQz.DelJob(jobId);
                data.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除任务失败");
                data.Success = false;
                data.Msg = ex.Message;
            }
            return data;
        }

        /// <summary>
        /// 添加一个任务
        /// </summary>
        //[HttpPost("AddJob")]
        //public async Task<MessageModel<string>> AddJob([FromBody] TasksQz tasksQz)
        //{
        //   // var v =new MessageModel<string>();

        //   //var vv= await _scheduler.StopScheduleJobAsync(tasksQz);
        //   // return v;
        //}

        /// <summary>
        /// 暂停一个任务
        /// </summary>
        //[HttpPost("AddJob")]
        //public async Task<MessageModel<string>> StopJob([FromBody] TasksQz tasksQz)
        //{
        //    var v= await _scheduler.StopScheduleJobAsync(tasksQz);
        //    return v;
        //}



    }
}
