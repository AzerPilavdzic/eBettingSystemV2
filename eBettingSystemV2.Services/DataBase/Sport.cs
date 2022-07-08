﻿using System;
using System.Collections.Generic;

#nullable disable

namespace eBettingSystemV2.Services.DataBase
{
    public partial class Sport
    {
        public Sport()
        {
            Competitions = new HashSet<Competition>();
            Teams = new HashSet<teams>();
        }

        public int SportsId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Competition> Competitions { get; set; }
        public virtual ICollection<teams> Teams { get; set; }
    }
}