using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GXVCU.Common.DB
{
    public class MainDB
    {
        public static string NodeName = "MainDB";

        public string CurrentDbConnId { get; set; }

        public bool MutiDBEnabled { get; set; }

        public bool CQRSEnabled { get; set; }

        public List<MutiDBOperate> DBS { get; set; }

        public static MutiDBOperate SpecialDbString(MutiDBOperate mutiDBOperate)
        {
            switch (mutiDBOperate?.DbType)
            {
                case DataBaseType.Sqlite:
                    mutiDBOperate.Connection = $"DataSource=" + Path.Combine(Environment.CurrentDirectory, mutiDBOperate.Connection);
                    break;
            }
            return mutiDBOperate;
        }

        public MutiDBOperate CurrentDb()
        {
            if (DBS == null)
                return null;
            var mutiDBOperate = DBS.First(c => c.ConnId == CurrentDbConnId);
            return SpecialDbString(mutiDBOperate);
        }
    }

    public class MutiDBOperate
    {
        /// <summary>
        /// 连接启用开关
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 连接ID
        /// </summary>
        public string ConnId { get; set; }
        /// <summary>
        /// 从库执行级别，越大越先执行
        /// </summary>
        public int HitRate { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string Connection { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DbType { get; set; }
    }

    public enum DataBaseType
    {
        MySql = 0,
        SqlServer = 1,
        Sqlite = 2,
        Oracle = 3,
        PostgreSQL = 4
    }
}
