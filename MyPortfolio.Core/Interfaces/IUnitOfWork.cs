namespace MyPortfolio.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProjectRepository Projects { get; }
        IBlogRepository Blogs { get; }
        Task<int> CompleteAsync();
    }
}