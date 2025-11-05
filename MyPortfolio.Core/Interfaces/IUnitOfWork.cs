namespace MyPortfolio.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProjectRepository Projects { get; }
        IBlogRepository Blogs { get; }
        IFeedbackRepository Feedbacks { get; }
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        Task<int> CompleteAsync();
    }
}