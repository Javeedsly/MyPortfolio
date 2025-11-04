using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Core.DTOs;
using MyPortfolio.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyPortfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbacksController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbacksController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        // POST: api/Feedbacks
        [HttpPost]
        public async Task<ActionResult<FeedbackDto>> SubmitFeedback([FromBody] CreateFeedbackDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newFeedback = await _feedbackService.CreateFeedbackAsync(createDto);
            return Ok(newFeedback);
        }


        // GET: api/Feedbacks
        [HttpGet]
        [Authorize] 
        public async Task<ActionResult<IEnumerable<FeedbackDto>>> GetFeedbacks()
        {
            var feedbacks = await _feedbackService.GetAllFeedbackAsync();
            return Ok(feedbacks);
        }

        // GET: api/Feedbacks/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<FeedbackDto>> GetFeedback(int id)
        {
            try
            {
                var feedback = await _feedbackService.GetFeedbackByIdAsync(id);
                return Ok(feedback);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/Feedbacks/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            try
            {
                await _feedbackService.DeleteFeedbackAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}