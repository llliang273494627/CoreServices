using System;
using System.Collections.Generic;
using System.Text;

namespace GXVCU.Common.SettingEntity
{
    /// <summary>
    /// 接口日志
    /// </summary>
    public class EntityMiddleware
    {
        /// <summary>
        /// 是否写入日志文件
        /// </summary>
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// 需要写入日志文件的接口
        /// </summary>
        public string IgnoreApis { get; set; } = string.Empty;
    }
}
