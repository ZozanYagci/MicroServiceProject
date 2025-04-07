using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Domain.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace BasketService.Api.Infrastructure.Repository
{
    public class RedisBasketRepository : IBasketRepository
    {
        private readonly ILogger<RedisBasketRepository> logger;
        private readonly ConnectionMultiplexer redis;
        private readonly IDatabase database;

        public RedisBasketRepository(ILogger<RedisBasketRepository> logger, ConnectionMultiplexer redis, IDatabase database)
        {
            this.logger = logger;
            this.redis = redis;
            this.database = database;
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await database.KeyDeleteAsync(id);
        }

        public async Task<CustomerBasket> GetBasketAsync(string customerId)
        {
            var data = await database.StringGetAsync(customerId);
            if (data.IsNullOrEmpty)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<CustomerBasket>(data);
        }

        public IEnumerable<string> GetUsers()
        {
            var server = GetServer();
            var data = server.Keys();

            return data?.Select(k => k.ToString());
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket customerBasket)
        {
            var created = await database.StringSetAsync(customerBasket.BuyerId, JsonConvert.SerializeObject(customerBasket));

            if (!created)
            {
                logger.LogInformation("Problem occur persisting the item.");
                return null;
            }
            logger.LogInformation("Basket item persisted succesfully");

            return await GetBasketAsync(customerBasket.BuyerId);
        }

        private IServer GetServer()
        {
            var endpoint = redis.GetEndPoints();
            return redis.GetServer(endpoint.First());
        }
    }
}
