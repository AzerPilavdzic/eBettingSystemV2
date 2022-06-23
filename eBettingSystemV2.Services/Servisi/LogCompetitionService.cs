using eBettingSystemV2.Services.DataBase;
using eBettingSystemV2.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services.Servisi
{
    public class LogCompetitionService:ILogCompetition
    {
        public praksa_dbContext Context { get; set; }


        public LogCompetitionService(praksa_dbContext Service1)
        {
            Context = Service1;
            
        }


        public void AddEntry(string naziv, DateTime Date, int Number)
        {
            var Entry = new Logcompetition
            {
                Naziv = naziv,
                Date = Date,
                Updated = Number
            };

            Context.Add(Entry);

            Context.SaveChanges();

            int i = 0;
        
        }











    }
}
