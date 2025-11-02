using MyPortfolio.Core.Entities;
using System.Threading.Tasks;

namespace MyPortfolio.Core.Interfaces
{
    public interface IBlogRepository : IRepository<Blog>
    {
        Task<Blog> GetBySlugAsync(string slug);
    }
}