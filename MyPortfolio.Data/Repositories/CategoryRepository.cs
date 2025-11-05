using MyPortfolio.Core.Entities;
using MyPortfolio.Core.Interfaces;

namespace MyPortfolio.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }
    }
}