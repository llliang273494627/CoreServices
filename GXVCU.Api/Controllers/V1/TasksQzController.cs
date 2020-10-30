using System.Threading.Tasks;
using GXVCU.Api.Filter;
using GXVCU.Common;
using GXVCU.Model.Models;
using GXVCU.Services;
using GXVCU.Tasks;
using GXVCU.Tasks.QuartzNet.Jobs;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace GXVCU.Api.Controllers.V1
{
    [Route("api/TasksQz")]
    [ApiController]
    public class TasksQzController : ControllerBase
    {
        private readonly SchedulerCenterServer _schedulerCenterServer;
        private readonly TasksQzServices _qzServices;

        public TasksQzController(SchedulerCenterServer schedulerCenterServer, TasksQzServices qzServices)
        {
            _schedulerCenterServer = schedulerCenterServer;
            _qzServices = qzServices;
        }

        /// <summary>
        /// 获取所以的任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustomRoute(ApiVersions.V1, "GetJobs")]
        public async Task<MessageModel<object>> GetJobs()
        {
            return new MessageModel<object>()
            {
                Response = await _qzServices.GetTasks(),
            };
        }

        /// <summary>
        /// 查找任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustomRoute(ApiVersions.V1, "SelectJob")]
        public async Task<MessageModel<object>> SelectJob(int jobId)
        {
            return new MessageModel<object>()
            {
                Response = await _qzServices.SelectJob(jobId)
            };
        }

        /// <summary>
        /// 添加计划任务
        /// </summary>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomRoute(ApiVersions.V1, "AddJob")]
        public async Task<MessageModel<string>> AddJob([FromBody] TasksQz tasksQz)
        {
            var data = new MessageModel<string>();
            var id = await _qzServices.AddTasks(tasksQz);
            data.Success = id > 0;
            if (data.Success)
            {
                data.Response = id.ObjToString();
                data.Msg = "添加成功";
            }
            return data;
        }

        /// <summary>
        /// 修改计划任务
        /// </summary>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomRoute(ApiVersions.V1, "UpdataJob")]
        public async Task<MessageModel<string>> UpdataJob([FromBody] TasksQz tasksQz)
        {
            var data = new MessageModel<string>();
            if (tasksQz != null && tasksQz.Id > 0)
            {
                data.Success = await _qzServices.UpdataJob(tasksQz);
                if (data.Success)
                {
                    data.Msg = "更新成功";
                    data.Response = tasksQz?.Id.ObjToString();
                }
            }
            return data;
        }

        /// <summary>
        /// 启动计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomRoute(ApiVersions.V1, "StartJob")]
        public async Task<MessageModel<string>> StartJob(int jobId)
        {
            var data = new MessageModel<string>();
            var model = await _qzServices.SelectJob(jobId);
            if (model != null)
            {
                var ResuleModel =await _schedulerCenterServer.AddScheduleJobAsync(model);
                if (ResuleModel.Success)
                {
                    model.IsStart = true;
                    data.Success = await _qzServices.UpdataJob(model);
                }
                if (data.Success)
                {
                    data.Msg = "启动成功";
                    data.Response = jobId.ObjToString();
                }
            }
            return data;
        }

        /// <summary>
        /// 停止一个计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>        
        [HttpPost]
        [CustomRoute(ApiVersions.V1, "StopJob")]
        public async Task<MessageModel<string>> StopJob(int jobId)
        {
            var data = new MessageModel<string>();

            var model = await _qzServices.SelectJob(jobId);
            if (model != null)
            {
                var ResuleModel = await _schedulerCenterServer.StopScheduleJobAsync(model);
                if (ResuleModel.Success)
                {
                    model.IsStart = false;
                    data.Success = await _qzServices.UpdataJob(model);
                }
                if (data.Success)
                {
                    data.Msg = "暂停成功";
                    data.Response = jobId.ObjToString();
                }
            }
            return data;

        }

        /// <summary>
        /// 重启一个计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomRoute(ApiVersions.V1, "ReCovery")]
        public async Task<MessageModel<string>> ReCovery(int jobId)
        {
            var data = new MessageModel<string>();

            var model = await _qzServices.SelectJob(jobId);
            if (model != null)
            {
                var ResuleModel = await _schedulerCenterServer.ResumeJob(model);
                if (ResuleModel.Success)
                {
                    model.IsStart = true;
                    data.Success = await _qzServices.UpdataJob(model);
                }
                if (data.Success)
                {
                    data.Msg = "重启成功";
                    data.Response = jobId.ObjToString();
                }
            }
            return data;

        }
    }
}
