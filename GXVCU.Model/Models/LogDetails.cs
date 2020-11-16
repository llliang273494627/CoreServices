using System;
using SqlSugar;

namespace GXVCU.Model.Models
{
    ///<summary>
    /// 日志
    ///</summary>
    [SugarTable("LogDetails")]
    public class LogDetails
    {        
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsIdentity = true,ColumnName = "LogID")]
        public int LogID { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [SugarColumn(ColumnName = "LogDate")]
        public DateTime LogDate { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "LogThread")]
        public string LogThread { get; set; } = string.Empty;

        /// <summary>
        /// 日志等级
        /// </summary>
        [SugarColumn(ColumnName = "LogLevel")]
        public int LogLevel { get; set; }

        /// <summary>
        /// 日志类别名称
        /// </summary>
        [SugarColumn(ColumnName = "LogLogger")]
        public string LogLogger { get; set; } = string.Empty;

        /// <summary>
        /// 日志信息
        /// </summary>
        [SugarColumn(ColumnName = "LogMessage")]
        public string LogMessage { get; set; } = string.Empty;

        [SugarColumn(ColumnName = "LogActionClick")]
        public string LogActionClick { get; set; } = string.Empty;

        [SugarColumn(ColumnName = "UserName")]
        public string UserName { get; set; } = string.Empty;

        [SugarColumn(ColumnName = "UserIP")]
        public string UserIP { get; set; } = string.Empty;

        [SugarColumn(ColumnName = "StackTrace")]
        public string StackTrace { get; set; } = string.Empty;

    }

}
