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

        public virtual async Task<Tless> InsertAsync(TInsert insert)
        {

            if (!BeforeInsertBool(insert))
            {
                return null;
            }

            var set = Context.Set<TDb>();

            TDb entity = Mapper.Map<TDb>(insert);

            set.Add(entity);

            BeforeInsert(insert, entity);

            await Context.SaveChangesAsync();

            return Mapper.Map<Tless>(entity);


        }

        public virtual void BeforeInsert(TInsert insert, TDb entity)
        {

        }

        public virtual bool BeforeInsertBool(TInsert insert)
        {
            return true;
        }

        public virtual IEnumerable<TDb> AddRange(IEnumerable<TInsert> insertlist ,DbSet<TDb> set)
        {
            IEnumerable<TDb> entity = Mapper.Map<IEnumerable<TDb>>(insertlist);
            set.AddRange(entity);

            return entity;


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

        public virtual async Task<T> UpdateAsync(int id, TUpdate update)
        {
            var set = await Context.Set<TDb>().FindAsync(id);

            //var entity = set.FindAsync(id);

            update = Coalesce(update, set);


            if (set != null)
            {
                Mapper.Map(update, set);
            }
            else
            {
                return null;
            }

            Context.SaveChanges();

            return Mapper.Map<T>(set);
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
        public virtual async Task<int> DeleteAsync(int id)
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
                return -1;
            }

            Context.SaveChanges();
            return id;

        }

        public virtual TUpdate Coalesce(TUpdate update,TDb entry)
        {
            return update;

        }

        public virtual async Task<IEnumerable<Tless>> InsertOneOrMoreAsync(IEnumerable<TInsert> List)
        {

            //if (!BeforeInsertBool(insert))
            //{
            //    return null;
            //}


            var set = Context.Set<TDb>();


            var entity=AddRange(List,set);


            //IEnumerable<TDb> entity = Mapper.Map<IEnumerable<TDb>>(List);

            //set.AddRange(entity);

            //BeforeInsert(insert, entity);



            await Context.SaveChangesAsync();

            return Mapper.Map<IEnumerable<Tless>>(entity);




        }







        public virtual bool checkIfNameSame(TInsert insert,TDb entry)
        {

            return false;
        
        }

        public virtual async Task<Tless> InsertById(TInsert Insert, int Id)
        {
            //if (!BeforeInsertBool(insert))
            //{
            //    return null;
            //}

            var set =  await Context.Set<TDb>().FindAsync(Id);
            TDb entity = null;


            

            if (set != null && checkIfNameSame(Insert,set)==false)
            {
               
                Mapper.Map(Insert, set);
                entity = Mapper.Map<TDb>(set);
            }
            else
            {
                if (!checkIfNameSame(Insert, set))
                {
                    entity = Mapper.Map<TDb>(Insert);
                    Context.Set<TDb>().Add(entity);

                }
                else
                {
                    entity = Mapper.Map<TDb>(set);

                }

              

            
            }



            //set.Add(entity);

            //BeforeInsert(insert, entity);


            await Context.SaveChangesAsync();

            return Mapper.Map<Tless>(entity);





        }

        
    }
}
