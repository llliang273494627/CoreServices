using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GXVCU.Common.Helper
{
    public static class FileHelper
    {
        /// <summary>
        /// 读取文件中所以文本
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <returns></returns>
        public static MessageModel<Exception> ReadFileTextAll(string fileFullName)
        {
            var res = new MessageModel<Exception>();
            try
            {
                StreamReader sr = new StreamReader(fileFullName, Encoding.UTF8);
                res.Msg = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Msg = "读取文件中文本异常";
                res.Response = ex;
            }
            return res;
        }
    }
}
