using RedisExampleApp.Api.Models;
using RedisExampleApp.Api.Repositories;

namespace RedisExampleApp.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            return await _repository.CreateAsync(product);
        }

        public async Task<Product> GetByIdAsync(int Id)
        {
            return await _repository.GetByIdAsync(Id);
        }

        public async Task<List<Product>> GetListAsync()
        {
            return await _repository.GetListAsync();
        }
    }
}
