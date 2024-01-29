using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Repositories.Contract
{
    //This special interface Repository that deal with Redis DB Not Store Context //
    public interface IBasketRepository
    {

     Task<CustomerBasket?> GetBasketAsync(string basketId);

    //Create or Update 
      Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket);
  
     Task<bool> DeleteBasketAsync(string basketId);

    }
}
