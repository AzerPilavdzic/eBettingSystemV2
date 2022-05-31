//using AutoMapper;
//using eProdaja.Model.SearchObjects;
//using eProdaja.Services.Database;
using AutoMapper;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Services.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services
{
    public class BaseService<T,TDb,TSearch>:
        IService<T, TSearch>
        where T : class 
        where TDb : class
        where TSearch : BaseSearchObject  //base search service
    {
        public praksa_dbContext Context { get; set; }
        public IMapper Mapper { get; set; }

        public BaseService(praksa_dbContext context , IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }


        public virtual bool CheckPage0(TSearch search=null)
        {
            return search.Page == 0 || search.PageSize == 0;
            
        }
        public virtual bool CheckNegative(TSearch search = null)
        {

            return search.Page < 0 || search.PageSize < 0;

        }




        public async virtual  Task<IEnumerable<T>> Get(TSearch search = null)
        {


            var entity = await Context.Set<TDb>().ToListAsync();

            var quary = entity.ToList().AsQueryable();

            entity = AddFilter(quary, search).ToList();

            //entity = AddInclude(entity, search).ToList();


            if (search?.Page.HasValue == true && search?.PageSize.HasValue == true)
            {
                //search.Page.Value* search.PageSize.Value


                entity = entity.Skip((search.Page.Value - 1) * search.PageSize.Value)
                    .Take(search.PageSize.Value).ToList();





            }

            var list = entity.ToList();
            //NOTE: elaborate IEnumerable vs IList

            //puca ovdje

            return Mapper.Map<IEnumerable<T>>(list);
         
        }

        public virtual IQueryable<TDb> AddInclude(IQueryable<TDb> query, TSearch search = null)
        {
            return query;
        }

        public virtual IQueryable<TDb> AddFilter(IQueryable<TDb> query , TSearch search = null)
        {
            return query;
        }

        // ne koristi se
        public T GetById(int id)
        {
            var set = Context.Set<TDb>();

            var entity = set.Find(id);

            return Mapper.Map<T>(entity);
      
        }



        //get by foreign key

        public virtual IQueryable<TDb> ForeignKeyFilter(IQueryable<TDb> query,int Id)
        {
                   
            return query;
        }



        public IEnumerable<T> GetbyForeignKey(int Id)
        {
            var set = Context.Set<TDb>();

            var list = ForeignKeyFilter(set, Id);

            if (list == null)
            {
                return null;


            }
            else
            {

                return Mapper.Map<IEnumerable<T>>(list);
            }

            


        }


        public async virtual Task<T> GetByIdAsync(int id)
        {
           
            var entity = await Context.Set<TDb>().FindAsync(id);
            
            //var entity = set.Find(id);

            return Mapper.Map<T>(entity);

        }


        public async Task<IEnumerable<T>> GetbyForeignKeyAsync(int Id)
        {
            var set = await Context.Set<TDb>().ToListAsync();

            var setq = set.AsQueryable();

            var list = ForeignKeyFilter(setq,Id);

            if (list == null)
            {
                return null;


            }
            else
            {

                return Mapper.Map<IEnumerable<T>>(list);
            }




        }





    }
}