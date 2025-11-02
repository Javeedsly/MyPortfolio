using MyPortfolio.Core.Interfaces;

namespace MyPortfolio.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IProjectRepository Projects { get; private set; }
        public IBlogRepository Blogs { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Projects = new ProjectRepository(_context);
            Blogs = new BlogRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}