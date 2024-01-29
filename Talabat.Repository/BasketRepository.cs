using StackExchange.Redis;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities; 

namespace Talabat.Core.Repositories.Contract
{
    //performs CRUD operations (Create, Read, Update, Delete) on the underlying data storage

    public class BasketRepository : IBasketRepository
    {


        private readonly IDatabase _database;

        ///Explain What is benefit this Constructor
        ///To Can do Any operation(Get,Delete,...) must  take object from Redis Connection,
        ///So,constructor ask from CLR to inject object from class with implement interface(IConnectionMultiplexer)
        ///from through this object i can open connection with Redis service  and connect on Redis DB  and Do any operation
        public BasketRepository(IConnectionMultiplexer redisconnection)
        {
            _database= redisconnection.GetDatabase(); //Get DataBase of Redis(Database of redis كده معاك ال ) 
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        ///Retrieves a customer basket from a Redis database based on the provided basket ID.
        ///CustomerBasket is contain on Json file
        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
          var basket = await _database.StringGetAsync(basketId);
            return basket.IsNullOrEmpty? null : JsonSerializer.Deserialize< CustomerBasket>(basket); 
        }

        // Updates or creates a customer basket in a Redis database
        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var CreatedOrUpdated = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
       /// If the set operation fails, it returns null;
     /// otherwise, it calls the GetBasketAsync method to retrieve and return the created or updated basket.
        if (CreatedOrUpdated is false) return null;

            return await GetBasketAsync(basket.Id);
        }
    }
}
