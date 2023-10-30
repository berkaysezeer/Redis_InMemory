using Microsoft.EntityFrameworkCore;
using RedisExampleApp.Api.Models;

namespace RedisExampleApp.Api.Repositories
{

    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> GetByIdAsync(int Id)
        {
            return await _context.Products.FindAsync(Id);
        }

        public async Task<List<Product>> GetListAsync()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
