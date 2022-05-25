﻿//using AutoMapper;
//using eProdaja.Model.SearchObjects;
//using eProdaja.Services.Database;
using AutoMapper;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Services.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services
{
    public class BaseCRUDService<T, TDb, TSearch, TInsert, TUpdate>
        : BaseService<T, TDb, TSearch>,
        ICRUDService<T, TSearch, TInsert, TUpdate>
        where T : class 
        where TDb : class 
        where TSearch : BaseSearchObject
        where TInsert : class 
        where TUpdate : class
    {
        public BaseCRUDService(praksa_dbContext context, IMapper mapper)
        : base(context, mapper) { }

        public virtual T Insert(TInsert insert)
        {
            var set = Context.Set<TDb>();

            TDb entity = Mapper.Map<TDb>(insert);

            set.Add(entity);

            BeforeInsert(insert, entity);

            Context.SaveChanges();

            return Mapper.Map<T>(entity);

           
        }

        public virtual void BeforeInsert(TInsert insert, TDb entity)
        {

        }

        public virtual T Update(int id, TUpdate update)
        {
            var set = Context.Set<TDb>();

            var entity = set.Find(id);

            if (entity != null)
            {
                Mapper.Map(update, entity);
            }
            else
            {
                return null;
            }

            Context.SaveChanges();

            return Mapper.Map<T>(entity);

           

        }

        public virtual T Delete(int id)
        {

            T Model = null;

            var set = Context.Set<TDb>();

            var entity = set.Find(id);

            if (entity != null)
            {

              
                Mapper.Map(entity, Model);
                Context.Remove(entity);
            }
            else
            {
                return null;
            
            }

            Context.SaveChanges();

            return Model;

        }


    }
}
