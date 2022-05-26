//using AutoMapper;
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
        public virtual IEnumerable<T> Get(TSearch search = null)
        {
            var entity = Context.Set<TDb>().AsQueryable();

            entity = AddFilter(entity, search);

            entity = AddInclude(entity, search);

            
            if (search?.Page.HasValue == true && search?.PageSize.HasValue == true)
            {

                int broj = search.Page.Value * search.PageSize.Value;
                entity = entity.Take(search.PageSize.Value).Skip(search.Page.Value * search.PageSize.Value);
            }

            var list = entity.ToList();
        //NOTE: elaborate IEnumerable vs IList
            return Mapper.Map<IList<T>>(list);
         
        }

        public virtual IQueryable<TDb> AddInclude(IQueryable<TDb> query, TSearch search = null)
        {
            return query;
        }

        public virtual IQueryable<TDb> AddFilter(IQueryable<TDb> query , TSearch search = null)
        {
            return query;
        }

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






    }
}