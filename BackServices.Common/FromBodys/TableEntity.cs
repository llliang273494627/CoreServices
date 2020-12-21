using System;
using System.Collections.Generic;
using System.Text;

namespace BackServices.Common.FromBodys
{
    public class TableEntity
    {
        /// <summary>
        /// 表实体文件夹路径
        /// </summary>
        public string DirectoryPath { get; set; }

        /// <summary>
        /// 表实体命名空间
        /// </summary>
        public string NameSpace { get; set; }
    }
}
