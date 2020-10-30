using GXVCU.Common.DB;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GXVCU.Api.Comm
{
    /// <summary>
    /// appsettings.json操作类
    /// </summary>
    public class Appsettings
    {
        private readonly IConfiguration _configuration;

        public Appsettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public MainDB MainDB { get { return _configuration.GetSection(MainDB.NodeName).Get<MainDB>(); } }

      
    }
}
