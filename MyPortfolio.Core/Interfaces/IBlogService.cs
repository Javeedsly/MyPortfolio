using MyPortfolio.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyPortfolio.Core.Interfaces
{
    public interface IBlogService
    {
        Task<BlogDto> GetBlogByIdAsync(int id);
        Task<BlogDto> GetBlogBySlugAsync(string slug);
        Task<IEnumerable<BlogDto>> GetAllBlogsAsync();
        Task<BlogDto> CreateBlogAsync(CreateBlogDto createBlogDto);
        Task UpdateBlogAsync(int id, UpdateBlogDto updateBlogDto);
        Task DeleteBlogAsync(int id);
    }
}