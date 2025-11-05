using MyPortfolio.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyPortfolio.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto> GetCategoryByIdAsync(int id);
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
        Task UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto);
        Task DeleteCategoryAsync(int id);
    }
}