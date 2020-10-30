using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace GXVCU.Model.Models
{
    [SugarTable("T_DefineFlow1")]
    public partial class DefineFlow1
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }
        public string flowname { get; set; }
        public string sendcmd { get; set; }
        public int waittime { get; set; }
        public string receivecmd { get; set; }
        public int enabled { get; set; }
        public string sendaddress { get; set; }
        public int sleeptime { get; set; }
        public int receivenum { get; set; }
        public int canind { get; set; }
    }
}
