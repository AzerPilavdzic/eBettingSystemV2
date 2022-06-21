using System;
using System.Collections.Generic;

#nullable disable

namespace eBettingSystemV2.Services.DataBase
{
    public partial class Logcompetition
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public DateTime Column1 { get; set; }
        public int? Updated { get; set; }
    }
}
