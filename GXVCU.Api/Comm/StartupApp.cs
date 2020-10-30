using GXVCU.Services;
using GXVCU.Tasks;
using log4net;
using Microsoft.AspNetCore.Builder;
using SqlSugar;
using System;

namespace GXVCU.Api.Comm
{
    public static class StartupApp
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StartupApp));

        /// <summary>
        /// 开启QuartzNetJob调度服务
        /// </summary>
        /// <param name="app"></param>
        /// <param name="qzServices"></param>
        /// <param name="schedulerCenter"></param>
        public static void UseQuartzJobMildd(this IApplicationBuilder app, 
            TasksQzServices qzServices, SchedulerCenterServer schedulerCenter)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            try
            {
                var allQzServices =qzServices.GetTasks();
                foreach (var item in allQzServices.Result)
                {
                    if (item.IsStart)
                    {
                        var ResuleModel = schedulerCenter.AddScheduleJobAsync(item).Result;
                        if (ResuleModel.Success)
                        {
                            Console.WriteLine($"任务ID:{item .Id} 任务名称:{item.Name}启动成功！");
                            log.Info($"任务ID:{item.Id} 任务名称:{item.Name}启动成功！");
                        }
                        else
                        {
                            Console.WriteLine($"任务ID:{item.Id} 任务名称:{item.Name}启动失败！错误信息：{ResuleModel.Msg}");
                            log.Error($"任务ID:{item.Id} 任务名称:{item.Name}启动失败！错误信息:{ResuleModel.Msg}");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                log.Error("查询数据表 TasksQz 异常", e);
            }
        }

      
    }
}
