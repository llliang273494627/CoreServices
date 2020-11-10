using System;
using System.Collections.Generic;
using System.Text;

namespace GXVCU.Common
{
    /// <summary>
    /// 格式转换
    /// </summary>
    public class FormatConversion
    {
        /// <summary>
        /// Json字符串转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public T JsonToObj<T>(string jsonStr)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonStr);
        }
    }
}
