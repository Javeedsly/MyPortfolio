using MyPortfolio.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyPortfolio.Core.Interfaces
{
    public interface IFeedbackService
    {
        Task<FeedbackDto> CreateFeedbackAsync(CreateFeedbackDto createFeedbackDto);
        Task<IEnumerable<FeedbackDto>> GetAllFeedbackAsync();
        Task<FeedbackDto> GetFeedbackByIdAsync(int id);
        Task DeleteFeedbackAsync(int id);
    }
}