using RedisExampleApp.Api.Models;

namespace RedisExampleApp.Api.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetListAsync();
        Task<Product> GetByIdAsync(int Id);
        Task<Product> CreateAsync(Product product);
    }
}
