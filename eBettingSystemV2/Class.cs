﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eBettingSystemV2.Services.DataBase;
//using eProdaja.Database;
using Microsoft.EntityFrameworkCore;

namespace eBettingSystemV2
{
    public class SetupService
    {
        public void Init(praksa_dbContext context)
        {
            //context.Database.Migrate();

            ////add new new data or update data
            //if (!context.JediniceMjeres.Any(x => x.Naziv == "Test"))
            //{
            //    context.JediniceMjeres.Add(new JediniceMjere() { Naziv = "Test" });
            //}

            //context.SaveChanges();
        }
    }
}
