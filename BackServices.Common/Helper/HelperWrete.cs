using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackServices.Common.Helper
{
    public class HelperWrete
    {
        /// <summary>
        /// 默认日志文件夹
        /// </summary>
        private static string defaultDir = Path.Combine(Directory.GetCurrentDirectory(), "Log");

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="fileName"></param>
        /// <param name="msg"></param>
        private static void WriteLog(string dir, string fileName, string msg)
        {
            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                string tmpfileName = Path.Combine(dir, $"{DateTime.Now:yyyyMMdd}_{fileName}.txt");
                using (StreamWriter stream = new StreamWriter(tmpfileName, true))
                {
                    stream.WriteLine("------------------------------------------------------------");
                    stream.WriteLine($"{DateTime.Now}\t{msg}");
                    stream.Flush();
                    stream.Close();
                    stream.Dispose();
                }
            }
            catch (Exception) { }
        }

        public static void Info(object message)
        {
            WriteLog(defaultDir, "Info", message.ToString());
        }

        public static void Error(object message)
        {
            WriteLog(defaultDir, "Error", message.ToString());
        }

        public static void Error(object message, Exception exception)
        {
            WriteLog(defaultDir, "Error", message.ToString() + "\r\n" + exception.Message + "\r\n" + exception.StackTrace);
        }

        public static void Error<T>(object message, Exception exception)
        {
            WriteLog(defaultDir, "Error", message.ToString() + "\r\n" + exception.Message + "\r\n" + exception.StackTrace);
        }


    }
}
