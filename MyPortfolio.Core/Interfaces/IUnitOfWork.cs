namespace MyPortfolio.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProjectRepository Projects { get; }
        IBlogRepository Blogs { get; }
        IFeedbackRepository Feedbacks { get; }
        Task<int> CompleteAsync();
    }
}