using Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Services {
    public class RedisService : IRedisService {
        private readonly StackExchange.Redis.IDatabase _redisDB;

        public RedisService(IConnectionMultiplexer redisMultiplexer) {
            _redisDB = redisMultiplexer.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key) {
            var val = await _redisDB.StringGetAsync(key);
            if (val.HasValue) {
                return JsonSerializer.Deserialize<T>(val!);
            }
            else {
                return default;
            }
        }
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null) {
            await _redisDB.StringSetAsync(key, JsonSerializer.Serialize(value), expiry);
        }

        public async Task RemoveAsync(string key) => await _redisDB.KeyDeleteAsync(key);

        public async Task RemoveByPatternAsync(string pattern) {
            var endpoints = _redisDB.Multiplexer.GetEndPoints();
            var server = _redisDB.Multiplexer.GetServer(endpoints.First());
            var keys = server.Keys(pattern: pattern);

            foreach (var key in keys) {
                await _redisDB.KeyDeleteAsync(key);
            }

        }

    }
}
