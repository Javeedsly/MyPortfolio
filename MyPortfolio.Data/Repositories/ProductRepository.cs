using Microsoft.EntityFrameworkCore;
using MyPortfolio.Core.Entities;
using MyPortfolio.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyPortfolio.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Product>> GetAllProductsWithCategoryAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product> GetProductWithCategoryByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}