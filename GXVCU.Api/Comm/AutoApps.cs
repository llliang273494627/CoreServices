using GXVCU.Common.DB;
using GXVCU.Model.Models;
using GXVCU.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace GXVCU.Api.Comm
{
    public static class AutoApps
    {
        /// <summary>
        /// 添加种子数据
        /// </summary>
        /// <param name="app"></param>
        /// <param name="dBSeed"></param>
        public static void UseSeedDataMildd(this IApplicationBuilder app, DBSeed dBSeed)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            // 创建数据库
            dBSeed.CreateDataBase();
            // 创建种子数据
            dBSeed.CreateDataInfo();
        }

        /// <summary>
        /// 开启QuartzNetJob调度服务
        /// </summary>
        public async static void UseQuartzJobMildd(this IApplicationBuilder app, ILogger<Startup> logger,
            DBSeed dBSeed, SchedulerCenterServer schedulerCenter)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            var tasksQzs = dBSeed.GetTable<TasksQz>().Response?.Where(t => t.IsStart).ToList();
            foreach (var item in tasksQzs)
            {
                var res = await schedulerCenter.AddScheduleJobAsync(item);
                string msg = res.Success ? "启动任务：" + item.Name : "启动任务失败：" + item.Name;
                Console.WriteLine(msg);
                logger.LogInformation(msg);
            }

        }
    }
}
