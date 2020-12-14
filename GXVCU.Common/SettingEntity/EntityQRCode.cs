using System;
using System.Collections.Generic;
using System.Text;

namespace GXVCU.Common.SettingEntity
{
    /// <summary>
    /// 二维码实体
    /// </summary>
    public class EntityQRCode
    {
        /// <summary>
        /// 参数
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 缩放比例
        /// </summary>
        public int Scale { get; set; } = 4;

        /// <summary>
        /// 版本
        /// </summary>
        public int Version { get; set; } = 8;
    }
}
