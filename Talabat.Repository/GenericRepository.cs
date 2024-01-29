using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbcontext;

        public GenericRepository(StoreContext dbcontext) //Ask CLR for creating object from DbContext
        {
            _dbcontext = dbcontext;
        }

        //Let two Methods Operate with traditional(Static) way to if there Entity hasn't Navigation prop and want to (get any thing)
        //for Example => if you want to (get all brands or Specific brand) and brand hasn't Navigation prop

        public async Task<T?> GetByIdAsync(int id) 
        {
            ///(مسكن)
            ///if (typeof(T) == typeof(Products))
            ///    return await _dbcontext.Set<Products>().Where(P => P.Id == id).Include(p => p.Brand).Include(p => p.Category).FirstOrDefaultAsync() as T; 
            
            return await _dbcontext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            ///(مسكن)
            /// if (typeof(T) == typeof(Products))
            ///Must be Include (Brand and Category) because Brand and Category related data with product table which (to put data of tables  Brand and Category at table Product must make loading  )  
            ///  return (IEnumerable<T>)await _dbcontext.Set<Products>().Skip(20).Take(10).Orderby(P => P.Name).Include(p => p.Brand).Include(p => p.Category).ToListAsync();

            return await _dbcontext.Set<T>().ToListAsync();
        }


        //Two Methods Operate with Specification Design Pattern(Dynamic way)
        //When we Use two methods ? when are there Navigational property that we need to make it (Include)
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {  
            return await ApplySpecifications(spec).ToListAsync();

         }

        public async Task<T?> GetEntityWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }


        public async Task<int> GetCountAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).CountAsync();
        }

        //To avoid repeating Code(to easy use)  
        private IQueryable<T> ApplySpecifications(ISpecifications<T> spec)
        {
            return SpecificationsEvaluator<T>.GetQuery(_dbcontext.Set<T>(), spec);
        }

        public async Task AddAsync(T entity)
        => await _dbcontext.AddAsync(entity);


        public void UpdateAsync(T entity)
        =>  _dbcontext.Update(entity);


        public void DeleteAsync(T entity)
          =>  _dbcontext.Remove(entity);

    }
}
