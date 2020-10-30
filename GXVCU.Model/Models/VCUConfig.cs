using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace GXVCU.Model.Models
{
    [SugarTable("T_VCUConfig")]
    public partial class VCUConfig
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }
        public string mtoc { get; set; }
        public string drivername { get; set; }
        public string driverpath { get; set; }
        public string binname { get; set; }
        public string binpath { get; set; }
        public string calname { get; set; }
        public string calpath { get; set; }
        public string softwareversion { get; set; }
        public string elementNum { get; set; }
        public string hardwarecode { get; set; }
        public string HW { get; set; }
        public string SW { get; set; }
        public string elementSign { get; set; }
        public string sign { get; set; }

    }
}
