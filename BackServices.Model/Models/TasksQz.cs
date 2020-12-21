using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackServices.Model.Models
{
    /// <summary>
    /// 任务计划表
    /// </summary>
    public class TasksQz 
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Name { get; set; }

        /// <summary>
        /// 任务分组
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string JobGroup { get; set; }

        /// <summary>
        /// 任务运行时间表达式
        /// </summary>
        [SugarColumn( IsNullable = true)]
        public string Cron { get; set; }

        /// <summary>
        /// 时间间隔（Cron表达式无效时启用）
        /// </summary>
        public int IntervalSecond { get; set; }

        /// <summary>
        /// 任务所在DLL对应的程序集名称
        /// </summary>
        [SugarColumn( IsNullable = true)]
        public string AssemblyName { get; set; }

        /// <summary>
        /// 任务所在类
        /// </summary>
        [SugarColumn( IsNullable = true)]
        public string ClassName { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        [SugarColumn( IsNullable = true)]
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 是否启动
        /// </summary>
        public bool IsStart { get; set; } 

        /// <summary>
        /// 执行传参
        /// </summary>
        public string JobParams { get; set; }

    }
}
