using MyPortfolio.Core.Entities;
using MyPortfolio.Core.Interfaces;

namespace MyPortfolio.Data.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(AppDbContext context) : base(context) { }

        // public async Task<IEnumerable<Project>> GetProjectsByTechnologyAsync(string technology)
        // {
        //     return await _context.Projects
        //         .Where(p => p.Technologies.Contains(technology))
        //         .ToListAsync();
        // }
    }
}