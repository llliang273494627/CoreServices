using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace GXVCU.Model.Models
{
    [SugarTable("T_MTOC")]
    public partial class MTOC
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }

        public string vin { get; set; }
        public string mtoc { get; set; }
        public int state { get; set; }
        public string element { get; set; }
        public DateTime updateTime { get; set; }

    }
}
