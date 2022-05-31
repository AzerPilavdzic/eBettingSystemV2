//using AutoMapper;
//using eProdaja.Model.SearchObjects;
//using eProdaja.Services.Database;
using AutoMapper;
using eBettingSystemV2.Model.SearchObjects;
using eBettingSystemV2.Services.Database;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Services
{
    public class BaseCRUDService<T, TDb, TSearch, TInsert, TUpdate,Tless>
        : BaseService<T, TDb, TSearch,Tless>,
        ICRUDService<T, TSearch, TInsert, TUpdate,Tless>
        where T : class 
        where TDb : class 
        where TSearch : BaseSearchObject
        where TInsert : class 
        where TUpdate : class
        where Tless : class
    {
        public BaseCRUDService(praksa_dbContext context, IMapper mapper)
        : base(context, mapper) { }

        public virtual Tless Insert(TInsert insert)
        {

            if (BeforeInsertBool(insert))
            {
                return null;                       
            }
            
            var set = Context.Set<TDb>();

            TDb entity = Mapper.Map<TDb>(insert);

            set.Add(entity);

            BeforeInsert(insert, entity);

            Context.SaveChanges();

            return Mapper.Map<Tless>(entity);

           
        }

        public virtual async Task<T> InsertAsync(TInsert insert)
        {

            if (BeforeInsertBool(insert))
            {
                return null;
            }

            var set = Context.Set<TDb>();

            TDb entity = Mapper.Map<TDb>(insert);

            set.Add(entity);

            BeforeInsert(insert, entity);

            await Context.SaveChangesAsync();

            return Mapper.Map<T>(entity);


        }

        public virtual void BeforeInsert(TInsert insert, TDb entity)
        {

        }

        public virtual bool BeforeInsertBool(TInsert insert)
        {

            return false;
        }




        public virtual T Update(int id, TUpdate update)
        {
            var set = Context.Set<TDb>();

            var entity = set.Find(id);

            update = Coalesce(update, entity);


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

            T Model = null ;

            var set = Context.Set<TDb>();

            var entity = set.Find(id);




            if (entity != null)
            {

                Model = Mapper.Map<T>(entity);

                //Mapper.Map(entity,Model);
              

                Context.Remove(entity);
            }
            else
            {
                return null;
            
            }

            Context.SaveChanges();

            return Model;

        }
        public virtual async Task<T> DeleteAsync(int id)
        {

            T Model = null;

            var set = await Context.Set<TDb>().FindAsync(id);

            //var entity = set.Find(id);




            if (set != null)
            {

                Model = Mapper.Map<T>(set);

                //Mapper.Map(entity,Model);


                Context.Remove(set);
            }
            else
            {
                return null;

            }

            Context.SaveChanges();

            return Model;

        }

        public virtual TUpdate Coalesce(TUpdate update,TDb entry)
        {



            return update;

        }

        
    }
}
