using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate_Modul;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Core
{
    ///There Two Problems were exist at UnitOfWork and Generic Method Solved them 
    ///The First Problem :
    ///=> I Use 3 repositories only at OrderService and you create object from UnitOfWork contain on 6 repositories 
    ///What I need Make at UnitOfWork ? => We don't want use a repository unless we need it(Per Request)
    ///which When need repository => Get it only,which (Per Request)
    ///The second Problem :
    ///We can't use  Property Signature for each and every repository at IUnitOfWork ?
    ///Because if increase new repository =>My Code is not applied (O) Priciple =>
    ///Open extension(add) and Close For Modification => you will increase new Repo(Manual/static) at IUnitOfWork
    ///this is Not dynamic,
    ///Solution Two Problems: => Make Generic Method ,When need GenericRepository<order> => execute This Method 
    ///And Method make GenericRepository<order> and when need GenericRepository<Product>
    ///this method Make GenericRepository and so on , Generic Method => Make life easy use and Dynamic 

    public interface IUnitOfWork : IAsyncDisposable 
    {

        ///Signature for Generic Method , Any body interact with this Method
        ///=>Will return IGenericRepository<Entity>, Entity that is needed
   
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;


        //Method simulate Method that Savechanges that exist at Dbcontext,return Number of Rows that made Row affected  
        Task<int> CompleteAsync();


    }
}
