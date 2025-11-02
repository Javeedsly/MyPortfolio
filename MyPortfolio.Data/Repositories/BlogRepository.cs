using Microsoft.EntityFrameworkCore;
using MyPortfolio.Core.Entities;
using MyPortfolio.Core.Interfaces;
using System.Threading.Tasks;

namespace MyPortfolio.Data.Repositories
{
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {
        public BlogRepository(AppDbContext context) : base(context) { }

        public async Task<Blog> GetBySlugAsync(string slug)
        {
            return await _context.Blogs.FirstOrDefaultAsync(b => b.Slug == slug);
        }
    }
}