using AutoMapper;
using Microsoft.Extensions.Logging;
using MyPortfolio.Core.DTOs;
using MyPortfolio.Core.Entities;
using MyPortfolio.Core.Interfaces;
using MyPortfolio.Data.Migrations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyPortfolio.Business.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<FeedbackService> _logger;
        private readonly IEmailService _emailService;

        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<FeedbackService> logger, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task<FeedbackDto> CreateFeedbackAsync(CreateFeedbackDto createFeedbackDto)
        {
            var feedback = _mapper.Map<Feedback>(createFeedbackDto);
            feedback.SubmittedDate = DateTime.UtcNow;

            await _unitOfWork.Feedbacks.AddAsync(feedback);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"New feedback submitted by {feedback.Email} with subject: {feedback.Subject}");

            
             try
            {
                var adminEmail = "cavidsly@gmail.com";
                var subject = $"New Portfolio Feedback: {feedback.Subject}";
                var message = $"<p>From: {feedback.Name} ({feedback.Email})</p>" +
                              $"<p>Message:</p>" +
                              $"<p>{feedback.Message}</p>";
                await _emailService.SendEmailAsync(adminEmail, subject, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not send feedback notification email to admin.");
            }
            

            return _mapper.Map<FeedbackDto>(feedback);
        }

        public async Task<IEnumerable<FeedbackDto>> GetAllFeedbackAsync()
        {
            var feedbacks = await _unitOfWork.Feedbacks.GetAllAsync();
            return _mapper.Map<IEnumerable<FeedbackDto>>(feedbacks);
        }

        public async Task<FeedbackDto> GetFeedbackByIdAsync(int id)
        {
            var feedback = await _unitOfWork.Feedbacks.GetByIdAsync(id);
            if (feedback == null)
            {
                throw new KeyNotFoundException("Feedback not found.");
            }
            return _mapper.Map<FeedbackDto>(feedback);
        }

        public async Task DeleteFeedbackAsync(int id)
        {
            var feedback = await _unitOfWork.Feedbacks.GetByIdAsync(id);
            if (feedback == null)
            {
                throw new KeyNotFoundException("Feedback not found.");
            }

            _unitOfWork.Feedbacks.Delete(feedback);
            await _unitOfWork.CompleteAsync();
        }
    }
}