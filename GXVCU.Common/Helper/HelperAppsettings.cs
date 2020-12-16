using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.IO;

namespace GXVCU.Common.Helper
{
   public class HelperAppsettings
    {
        protected IConfiguration _configuration;

        public HelperAppsettings()
        {
            string contentPath = Directory.GetCurrentDirectory();
            string Path = "appsettings.json";

            //如果你把配置文件 是 根据环境变量来分开了，可以这样写
            //Path = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json";
            _configuration = new ConfigurationBuilder()
               .SetBasePath(contentPath)
               .Add(new JsonConfigurationSource { Path = Path, Optional = false, ReloadOnChange = true })//这样的话，可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性
               .Build();
        }

        public HelperAppsettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 根据配置文件所在文件夹实例化对象
        /// </summary>
        /// <param name="contentPath">配置文件所在文件夹路径</param>
        public HelperAppsettings(string contentPath)
        {
            try
            {
                string fileDir = Path.GetDirectoryName(contentPath);
                string fileName = Path.GetFileName(contentPath);
                string fileExtension = Path.GetExtension(contentPath);
                if (string.IsNullOrEmpty(fileExtension) || fileExtension.ToLower() != ".json")
                {
                    fileDir = Directory.GetCurrentDirectory();
                    fileName = "appsettings.json";
                }
                else
                {
                    if (string.IsNullOrEmpty(fileDir))
                    {
                        fileDir = Directory.GetCurrentDirectory();
                    }
                }

                //如果你把配置文件 是 根据环境变量来分开了，可以这样写
                //Path = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json";
                _configuration = new ConfigurationBuilder()
                   .SetBasePath(fileDir)
                   .Add(new JsonConfigurationSource { Path = fileName, Optional = false, ReloadOnChange = true })//这样的话，可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性
                   .Build();
            }
            catch (Exception ex)
            {
                HelperLog.Error<HelperAppsettings>($"json配置文件异常！contentPath={contentPath}", ex);
            }
        }

        /// <summary>
        /// 获取节点值 "":"" 或 ""
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public string GetNodeValue(string nodeName)
        {
            try
            {
                return _configuration.GetSection(nodeName).Get<string>();
            }
            catch (Exception ex)
            {
                HelperLog.Error<HelperAppsettings>($"获取配置参数失败！nodeName={nodeName}", ex);
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取节点实体 "":"" 或 ""
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public T GetNodeEntity<T>(string nodeName)
        {
            try
            {
                return _configuration.GetSection(nodeName).Get<T>();
            }
            catch (Exception ex)
            {
                HelperLog.Error<HelperAppsettings>($"获取配置参数失败！nodeName={nodeName}", ex);
            }
            return default(T);
        }

    }
}
