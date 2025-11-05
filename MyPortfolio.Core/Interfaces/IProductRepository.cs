using MyPortfolio.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyPortfolio.Core.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetProductWithCategoryByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsWithCategoryAsync();
    }
}