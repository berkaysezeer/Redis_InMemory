using RedisExampleApp.Api.Models;
using RedisExampleApp.Cache;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisExampleApp.Api.Repositories
{
    public class ProductRepositoryCacheDecorator : IProductRepository
    {
        private const string productKey = "productCaches";
        private readonly IProductRepository _repository;
        private readonly RedisService _redisService;
        private readonly IDatabase _cacheRepository;

        public ProductRepositoryCacheDecorator(IProductRepository repository, RedisService redisService)
        {
            _repository = repository;
            _redisService = redisService;
            _cacheRepository = _redisService.GetDb(1);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            var databaseProduct = await _repository.CreateAsync(product);

            //if (await _cacheRepository.KeyExistsAsync(productKey))
            await _cacheRepository.HashSetAsync(productKey, databaseProduct.Id, JsonSerializer.Serialize(databaseProduct));

            return databaseProduct;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            //cache'de data yok ise veri tabanındaki değerler cache atılacak
            if (await _cacheRepository.KeyExistsAsync(productKey))
            {
                var product = await _cacheRepository.HashGetAsync(productKey, id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
            }

            var products = await LoadToCacheFromDbAsync();

            //var product döner, yoksa null döner
            return products.FirstOrDefault(x => x.Id == id);
        }

        public async Task<List<Product>> GetListAsync()
        {
            //cache'de data yok ise veri tabanındaki değerler cache atılacak
            if (!await _cacheRepository.KeyExistsAsync(productKey))
                return await LoadToCacheFromDbAsync();

            List<Product> products = new();

            var cacheProducts = await _cacheRepository.HashGetAllAsync(productKey);

            //veri tabanındaki datayı value kısmında json ile cache'e atmıştık
            foreach (var item in cacheProducts.ToList())
            {
                //bundan dolayı item json nesnesi geliyor ve Deserialize ediyoruz
                var product = JsonSerializer.Deserialize<Product>(item.Value);
                products.Add(product);
            }

            return products;
        }

        //veri tabanındaki datayı cacheleyecek metot
        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            var products = await _repository.GetListAsync();

            products.ForEach(p =>
            {
                _cacheRepository.HashSetAsync(productKey, p.Id, JsonSerializer.Serialize(p));
            });

            return products;
        }
    }
}
