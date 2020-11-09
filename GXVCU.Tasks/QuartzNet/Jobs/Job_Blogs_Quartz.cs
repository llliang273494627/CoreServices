using log4net;
using Microsoft.Extensions.Logging;
using Quartz;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GXVCU.Tasks.QuartzNet.Jobs
{
    public class Job_Blogs_Quartz : IJob
    {
        private readonly ILogger<Job_Blogs_Quartz> _logger;
        public Job_Blogs_Quartz(ILogger<Job_Blogs_Quartz> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                Console.WriteLine(DateTime.Now + " 执行MyJob");
                _logger.LogInformation("执行MyJob");
            });
        }
    }
}
