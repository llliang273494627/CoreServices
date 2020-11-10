using System;
using System.Collections.Generic;
using System.Text;

namespace GXVCU.Common.Helper
{
    /// <summary>
    /// 格式转换
    /// </summary>
    public class FormatHelper
    {
        /// <summary>
        /// Json字符串转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T JsonToObj<T>(string jsonStr)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonStr);
        }
    }
}
