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
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogsController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogDto>>> GetBlogs()
        {
            var blogs = await _blogService.GetAllBlogsAsync();
            return Ok(blogs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlogDto>> GetBlog(int id)
        {
            var blog = await _blogService.GetBlogByIdAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return Ok(blog);
        }

        [HttpGet("slug/{slug}")]
        public async Task<ActionResult<BlogDto>> GetBlogBySlug(string slug)
        {
            var blog = await _blogService.GetBlogBySlugAsync(slug);
            if (blog == null)
            {
                return NotFound();
            }
            return Ok(blog);
        }

        [HttpPost]
        //[Authorize]
        public async Task<ActionResult<BlogDto>> CreateBlog([FromForm] CreateBlogDto createDto)
        {
            var newBlog = await _blogService.CreateBlogAsync(createDto);
            return CreatedAtAction(nameof(GetBlog), new { id = newBlog.Id }, newBlog);
        }

        [HttpPut("{id}")]
        //[Authorize]
        public async Task<IActionResult> UpdateBlog(int id, [FromForm] UpdateBlogDto updateDto)
        {
            try
            {
                await _blogService.UpdateBlogAsync(id, updateDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        //[Authorize]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            try
            {
                await _blogService.DeleteBlogAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}