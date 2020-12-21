using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BackServices.Tasks
{
    public class JobBase
    {
        /// <summary>
        /// 执行指定任务
        /// </summary>
        /// <param name="context"></param>
        /// <param name="action"></param>
        public async Task<string> ExecuteJob(IJobExecutionContext context, Func<Task> func)
        {
            string jobHistory = $"【{DateTime.Now}】执行任务【Id：{context.JobDetail.Key.Name}，组别：{context.JobDetail.Key.Group}】";
            try
            {
                var s = context.Trigger.Key.Name;
                //记录Job时间
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                await func();//执行任务
                stopwatch.Stop();
                jobHistory += $"，【执行成功】，完成时间：{stopwatch.Elapsed.TotalMilliseconds.ToString("00")}毫秒";
            }
            catch (Exception ex)
            {
                JobExecutionException e2 = new JobExecutionException(ex);
                //true  是立即重新执行任务 
                e2.RefireImmediately = true;
                jobHistory += $"，【执行失败】，异常日志：{ex.Message}";
            }
            return jobHistory;
        }
    }
}
