﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackServices.Common.DB
{
    public class MainDB
    {
        /// <summary>
        /// 是否写入日志
        /// </summary>
        public bool SqlAOPEnabled { get; set; }

        public string CurrentDbConnId { get; set; }

        public List<MutiDBOperate> DBS { get; set; }

        public MutiDBOperate CurrentDb
        {
            get
            {
                if (DBS == null) return null;
                return DBS.First(c => c.ConnId == CurrentDbConnId);
            }
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
