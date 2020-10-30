using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace GXVCU.Model.Models
{
    [SugarTable("T_VCUCodeList")]
    public partial class VCUCodeList
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }
        public string baud { get; set; }
        public string sendaddress { get; set; }
        public string responseaddress { get; set; }

    }
}
