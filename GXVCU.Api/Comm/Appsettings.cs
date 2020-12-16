using GXVCU.Common.DB;
using GXVCU.Common.Helper;
using GXVCU.Common.SettingEntity;
using Microsoft.Extensions.Configuration;

namespace GXVCU.Api.Comm
{
    public class Appsettings : HelperAppsettings
    {
        public Appsettings() : base() { }

        public Appsettings(IConfiguration configuration) : base(configuration) { }

        /// <summary>
        /// 根据配置文件所在文件夹实例化对象
        /// </summary>
        /// <param name="contentPath">配置文件所在文件夹路径</param>
        public Appsettings(string contentPath) : base(contentPath) { }

        public string UseUrls { get { return _configuration.GetSection("UseUrls").Get<string>(); } }

        public MainDB MainDB { get { return _configuration.GetSection("MainDB").Get<MainDB>(); } }

        public EntityMiddleware Middleware { get { return _configuration.GetSection("Middleware").Get<EntityMiddleware>(); } }
    }
}
