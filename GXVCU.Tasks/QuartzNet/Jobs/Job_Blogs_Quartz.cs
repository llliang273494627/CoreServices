using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;

namespace GXVCU.Tasks.QuartzNet.Jobs
{
    public class Job_Blogs_Quartz :JobBase, IJob
    {
        private readonly ILogger<Job_Blogs_Quartz> _logger;

        public Job_Blogs_Quartz(ILogger<Job_Blogs_Quartz> logger)
        {
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var jobHistory = await ExecuteJob(context, async () => await DoWork(context));
            Console.WriteLine(jobHistory);
            _logger.LogInformation(jobHistory);
        }

        private async Task DoWork(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync("博客总数量");
        }

       

    }
}
