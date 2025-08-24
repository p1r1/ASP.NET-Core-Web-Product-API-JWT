using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories {
    public class ProductRepository : IProductRepository {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context) {
            _context = context;
        }

        public async Task<Product?> GetByIdAsync(int id) {
            return await _context.Products.FindAsync(id);
        }

        public async Task<List<Product>> GetAllAsync() {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> AddAsync(Product product) {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task UpdateAsync(Product product) {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) {
            var product = await GetByIdAsync(id);
            if (product != null) {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

    }
}
