namespace MyPortfolio.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProjectRepository Projects { get; }
        Task<int> CompleteAsync();
    }
}