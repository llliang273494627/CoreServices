using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GXVCU.Tasks.QuartzNet.Jobs
{
    public class MyJob : IJob
    {
        private readonly ILogger<MyJob> _logger;

        //public MyJob(ILogger<MyJob> logger)
        //{
        //    _logger = logger;
        //}

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("执行MyJob");
                //_logger.LogInformation("执行MyJob");
            });
        }
    }
}
