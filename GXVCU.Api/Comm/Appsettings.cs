using GXVCU.Common.DB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GXVCU.Api.Comm
{
    public class Appsettings
    {
        private readonly IConfiguration _configuration;

        public Appsettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 根据配置文件所在文件夹实例化对象
        /// </summary>
        /// <param name="contentPath">配置文件所在文件夹路径</param>
        public Appsettings(string contentPath)
        {
            string Path = "appsettings.json";

            //如果你把配置文件 是 根据环境变量来分开了，可以这样写
            //Path = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json";

            _configuration = new ConfigurationBuilder()
               .SetBasePath(contentPath)
               .Add(new JsonConfigurationSource { Path = Path, Optional = false, ReloadOnChange = true })//这样的话，可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性
               .Build();
        }

        public string UseUrls { get { return _configuration.GetSection("UseUrls:Urls").Get<string>(); } }

        /// <summary>
        /// 是否是应用程序
        /// </summary>
        public bool IsExe { get { return _configuration.GetSection("UseUrls:IsExe").Get<bool>(); } }

        public MainDB MainDB { get { return _configuration.GetSection(MainDB.NodeName).Get<MainDB>(); } }
    }
}
