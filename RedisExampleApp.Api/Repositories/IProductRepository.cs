using RedisExampleApp.Api.Models;

namespace RedisExampleApp.Api.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetListAsync();
        Task<Product> GetByIdAsync(int Id);
        Task<Product> CreateAsync(Product product);
    }
}
