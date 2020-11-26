using GXVCU.Common;
using GXVCU.Model.Models;
using GXVCU.Tasks.QuartzNet.Jobs;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using Quartz.Spi;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GXVCU.Tasks
{
    /// <summary>
    /// 任务调度管理中心
    /// </summary>
    public class SchedulerCenterServer 
    {
        private Task<IScheduler> _scheduler;
        private readonly IJobFactory _iocjobFactory;
        private readonly ILogger<SchedulerCenterServer> _logger;

        public SchedulerCenterServer(IJobFactory jobFactory,ILogger<SchedulerCenterServer> logger)
        {
            _iocjobFactory = jobFactory;
            _scheduler = GetSchedulerAsync();
            _logger = logger;
        }

        private Task<IScheduler> GetSchedulerAsync()
        {
            if (_scheduler != null)
                return this._scheduler;
            else
            {
                // 从Factory中获取Scheduler实例
                NameValueCollection collection = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" },
                };
                StdSchedulerFactory factory = new StdSchedulerFactory(collection);
                return _scheduler = factory.GetScheduler();
            }
        }

        /// <summary>
        /// 开启任务调度
        /// </summary>
        /// <returns></returns>
        public async Task<MessageModel<string>> StartScheduleAsync()
        {
            var result = new MessageModel<string>();
            try
            {
                this._scheduler.Result.JobFactory = this._iocjobFactory;
                if (!this._scheduler.Result.IsStarted)
                {
                    //等待任务运行完成
                    await this._scheduler.Result.Start();
                    await Console.Out.WriteLineAsync("任务调度开启！");
                    result.Success = true;
                    result.Msg = $"任务调度开启成功";
                }
                else
                {
                    result.Success = false;
                    result.Msg = $"任务调度已经开启";
                }
                _logger.LogInformation(result.Msg);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "任务调度开启异常");
            }
            return result;
        }

        /// <summary>
        /// 停止任务调度
        /// </summary>
        /// <returns></returns>
        public async Task<MessageModel<string>> StopScheduleAsync()
        {
            var result = new MessageModel<string>();
            try
            {
                if (!this._scheduler.Result.IsShutdown)
                {
                    //等待任务运行完成
                    await this._scheduler.Result.Shutdown();
                    await Console.Out.WriteLineAsync("任务调度停止！");
                    result.Success = true;
                    result.Msg = $"任务调度停止成功";
                }
                else
                {
                    result.Success = false;
                    result.Msg = $"任务调度已经停止";
                }
                _logger.LogInformation(result.Msg);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "任务调度停止异常");
            }
            return result;
        }

        /// <summary>
        /// 添加一个计划任务（映射程序集指定IJob实现类）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        public async Task<MessageModel<string>> AddScheduleJobAsync(TasksQz tasksQz)
        {
            var result = new MessageModel<string>();
            if (tasksQz != null)
            {
                try
                {
                    JobKey jobKey = new JobKey(tasksQz.Id.ToString(), tasksQz.JobGroup);
                    if (await _scheduler.Result.CheckExists(jobKey))
                    {
                        result.Success = false;
                        result.Msg = $"该任务计划已经在执行:【{tasksQz.Name}】,请勿重复启动！";
                        return result;
                    }
                    if (tasksQz.BeginTime == null)
                    {
                        tasksQz.BeginTime = DateTime.Now;
                    }
                    DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(tasksQz.BeginTime, 1);//设置开始时间
                    if (tasksQz.EndTime == null)
                    {
                        tasksQz.EndTime = DateTime.MaxValue.AddDays(1);
                    }
                    DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(tasksQz.EndTime, 1);//设置暂停时间

                    Assembly assembly = Assembly.Load(new AssemblyName(tasksQz.AssemblyName));
                    Type jobType = assembly.GetTypes().Where(t=>t.Name==tasksQz.ClassName).First();

                    //判断任务调度是否开启
                    if (!_scheduler.Result.IsStarted)
                    {
                        await StartScheduleAsync();
                    }

                    //传入反射出来的执行程序集
                    IJobDetail job = new JobDetailImpl(tasksQz.Id.ToString(), tasksQz.JobGroup, jobType);
                    job.JobDataMap.Add("JobParam", tasksQz.JobParams);

                    ITrigger trigger = CreateCronTrigger(tasksQz);
                    // 告诉Quartz使用我们的触发器来安排作业
                    await _scheduler.Result.ScheduleJob(job, trigger);
                    result.Success = true;
                    result.Msg = $"启动任务:【{tasksQz.Name}】成功";
                    return result;
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Msg = $"任务计划异常:【{ex.Message}】";
                    _logger.LogError(ex, result.Msg);
                    return result;
                }
            }
            else
            {
                result.Success = false;
                result.Msg = $"任务计划不存在:【{tasksQz?.Name}】";
                _logger.LogInformation(result.Msg);
                return result;
            }
        }

        /// <summary>
        /// 暂停一个指定的计划任务
        /// </summary>
        /// <returns></returns>
        public async Task<MessageModel<string>> StopScheduleJobAsync(TasksQz sysSchedule)
        {
            var result = new MessageModel<string>();
            try
            {
                JobKey jobKey = new JobKey(sysSchedule.Id.ToString(), sysSchedule.JobGroup);
                if (!await _scheduler.Result.CheckExists(jobKey))
                {
                    result.Success = false;
                    result.Msg = $"未找到要暂停的任务:【{sysSchedule.Name}】";
                    _logger.LogInformation(result.Msg);
                    return result;
                }
                else
                {
                    await this._scheduler.Result.PauseJob(jobKey);
                    result.Success = true;
                    result.Msg = $"暂停任务:【{sysSchedule.Name}】成功";
                    _logger.LogInformation(result.Msg);
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "暂停任务异常");
                throw;
            }
        }

        /// <summary>
        /// 恢复指定的计划任务
        /// </summary>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        public async Task<MessageModel<string>> ResumeJob(TasksQz tasksQz)
        {
            var result = new MessageModel<string>();
            try
            {
                JobKey jobKey = new JobKey(tasksQz.Id.ToString(), tasksQz.JobGroup);
                if (!await _scheduler.Result.CheckExists(jobKey))
                {
                    result.Success = false;
                    result.Msg = $"未找到要重新的任务:【{tasksQz.Name}】,请先选择添加计划！";
                    _logger.LogInformation(result.Msg);
                    return result;
                }

                ITrigger trigger = CreateCronTrigger(tasksQz);

                TriggerKey triggerKey = new TriggerKey(tasksQz.Id.ToString(), tasksQz.JobGroup);
                await _scheduler.Result.RescheduleJob(triggerKey, trigger);

                result.Success = true;
                result.Msg = $"恢复计划任务:【{tasksQz.Name}】成功";
                _logger.LogInformation(result.Msg);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "恢复计划任务异常");
                throw;
            }
        }

        /// <summary>
        /// 创建类型Cron的触发器
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private ITrigger CreateCronTrigger(TasksQz sysSchedule)
        {
            //CronExpression.IsValidExpression(sysSchedule.Cron)
            var trigger = TriggerBuilder.Create();
            trigger.WithIdentity(sysSchedule.Id.ToString(), sysSchedule.JobGroup);
            // 开始时间
            trigger.StartAt(sysSchedule.BeginTime.Value);
            // 结束数据
            trigger.EndAt(sysSchedule.EndTime.Value);
            // 指定cron表达式
            if (CronExpression.IsValidExpression(sysSchedule.Cron))
                trigger.WithCronSchedule(sysSchedule.Cron);
            else
            {
                trigger.WithSimpleSchedule(x =>
                    x.WithIntervalInSeconds(sysSchedule.IntervalSecond)
                    .RepeatForever()).ForJob(sysSchedule.Id.ToString(), sysSchedule.JobGroup);
            }
            // 作业名称
            trigger.ForJob(sysSchedule.Id.ToString(), sysSchedule.JobGroup);
            // 作业触发器
            return trigger.Build();
        }
       
    }
}
