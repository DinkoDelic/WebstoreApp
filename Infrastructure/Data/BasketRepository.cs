using System;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entitites;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            // Delete key value 
           return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            // Gets the value of the key (basketId)
            var data = await _database.StringGetAsync(basketId);

            // If data is not null we deserialize into our customer basket
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            // Replace existing basket (string) with the new one, setting expiry date to 30 days from update
           var created = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));

           return created ? await GetBasketAsync(basket.Id) : null;
        }
    }
}