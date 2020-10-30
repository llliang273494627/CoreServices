using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace GXVCU.Model.Models
{
    [SugarTable("T_Result")]
    public partial class Result
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }
        public string vin { get; set; }
        public string mtoc { get; set; }
        public string flashBin { get; set; }
        public string writeBin { get; set; }
        public string softwareversion { get; set; }
        public string tracyCode { get; set; }
        public DateTime testtime { get; set; }
        public int teststate { get; set; }
        public int uploadsign { get; set; }
        public int isprint { get; set; }
        public string calBin { get; set; }
        public int num { get; set; }
        public string sign { get; set; }

    }
}
