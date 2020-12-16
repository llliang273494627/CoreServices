using log4net;
using System;
using System.Collections.Generic;
using System.Text;

namespace GXVCU.Common.Helper
{
    public class HelperLog
    {
        private static readonly ILog logger = LogManager.GetLogger("GXVCU.Common");

        public static void Error(object message, Exception exception)
        {
            logger.Error(message, exception);
        }

        public static void Error(object message)
        {
            logger.Error(message);
        }

        public static void Error<T>(object message, Exception exception)
        {
            LogManager.GetLogger(typeof(T)).Error(message, exception);
        }

        public static void Error<T>(object message)
        {
            LogManager.GetLogger(typeof(T)).Error(message);
        }
    }
}
