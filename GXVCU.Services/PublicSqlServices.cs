using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace GXVCU.Services
{
    /// <summary>
    /// 通用sql服务
    /// </summary>
    public class PublicSqlServices
    {
        private readonly ILogger<PublicSqlServices> _logger;
        private readonly SqlSugarClient _sqlSugarClient;

        public PublicSqlServices(ILogger<PublicSqlServices> logger, SqlSugarClient sqlSugarClient)
        {
            _logger = logger;
            _sqlSugarClient = sqlSugarClient;
        }
    }
}
