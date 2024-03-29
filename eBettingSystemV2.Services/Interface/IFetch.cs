﻿using eBettingSystemV2.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static eBettingSystemV2.Model.Models.FetchEventModel;

namespace eBettingSystemV2.Services.Interface
{
    public interface IFetch
    {
        List<PodaciSaStranice> FetchSportAndData();
        List<string> FetchAllSports();

        List<PodaciSaStranice> FetchDataBySport(string sport, bool ispis = true);

        abstract List<string> FetchAllEvents();

        public List<string> FetchEventKeys();

        public Task FetchEventData();

        public List<FetchEventModel> FetchAllEvents2();

        public IEnumerable<Tuple<string, string>> FetchEventsForUpdate();

        public List<string> FetchCompetitionsName(string sport);

    }
}
