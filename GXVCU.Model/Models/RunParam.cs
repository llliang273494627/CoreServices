using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace GXVCU.Model.Models
{
    [SugarTable("T_RunParam")]
    public partial class RunParam
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }
        public string groups { get; set; }
        public string descriptions { get; set; }
        public string keys { get; set; }
        public string keyvalue { get; set; }

    }
}
