﻿using AutoMapper;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.Database;
//using eProdaja.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProdaja.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Country, CountryModel>();

        }
    }
}