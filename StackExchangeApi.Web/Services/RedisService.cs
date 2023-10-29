using StackExchange.Redis;

namespace StackExchangeApi.Web.Services
{
    public class RedisService
    {
        private ConnectionMultiplexer _redis;
        public IDatabase db { get; set; }

        public RedisService(IConfiguration configuration)
        {
            string host = configuration["Redis:Host"];
            string port = configuration["Redis:Port"];
            var configString = $"{host}:{port}";

            _redis = ConnectionMultiplexer.Connect(configString);
        }


        public IDatabase GetDb(int db)
        {
            return _redis.GetDatabase(db);
        }
    }
}
