﻿using GXVCU.Common;
using GXVCU.Common.Helper;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GXVCU.Tasks.QuartzNet.Jobs
{
    /// <summary>
    /// 监控检查心跳包
    /// </summary>
    public class Job_HealthCheck_Quartz : JobBase, IJob
    {
        private readonly ILogger<Job_HealthCheck_Quartz> _logger;
        private readonly HelperAppsettings _helperAppsettings;

        public Job_HealthCheck_Quartz(ILogger<Job_HealthCheck_Quartz> logger)
        {
            _logger = logger;
            _helperAppsettings = new HelperAppsettings(Directory.GetCurrentDirectory());
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var jobHistory = await ExecuteJob(context, async () => await DoWork(context));
        }

        private async Task DoWork(IJobExecutionContext context)
        {
            string url = _helperAppsettings.GetNodeValue("UseUrls");
            url += "/api/Values/HealthCheck";
            var repone = await HttpHelper.GetApi<MessageModel<string>>(url);
            if (repone == null || !repone.Success)
            {
                _logger.LogError(repone.Msg);
            }
        }



    }
}