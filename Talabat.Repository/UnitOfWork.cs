using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate_Modul;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    //Class Responsible for deal with Database through Dbcontext
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbcontext;

        ///Dictionary/Hashtable:Store in it repositories that will request at request,
        ///To if you needed again through request don't create same object with same request .
        /// private Dictionary<string, GenericRepository<BaseEntity>> _repositories;
        ///Dictionary(generic) == Hashtable(Non generic) 
        ///Hashtable ==> Key value paire and key and value are object

        private Hashtable _repositories; 

        //Ask from Clr For Creating object from dbcontext Implicitly
        public UnitOfWork(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;

            ///When Create object from UnitOfWork at OrderService => execute (new) 
            ///(new) =>  to repositories with null,
            ///We Must Initialize to _repositories and Change Value of _repositoriy(Null)       

            _repositories = new Hashtable();
        }

        //GenericRepository is class is typically used for common CRUD operations on entities.
        //Use This method to (Create object of GenericRepository per Request) 
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
          var Key = typeof(TEntity).Name; //""Order""

            //Which if repository first time required   
            if (!_repositories.ContainsKey(Key))
            {
                //=> (Create object of GenericRepository of specific entity type per Request) 

                var repository = new GenericRepository<TEntity>(_dbcontext);

                _repositories.Add(Key, repository);
            }
       
               return _repositories[Key] as IGenericRepository<TEntity>;
        }


        public async Task<int> CompleteAsync()
                 => await _dbcontext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
           => await _dbcontext.DisposeAsync();

    }
}





