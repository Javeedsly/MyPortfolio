using MyPortfolio.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyPortfolio.Core.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
        Task UpdateProductAsync(int id, UpdateProductDto updateProductDto);
        Task DeleteProductAsync(int id);
    }
}