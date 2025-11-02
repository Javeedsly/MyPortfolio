using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Authorization;
using MyPortfolio.Core.DTOs;
using MyPortfolio.Core.Interfaces;

namespace MyPortfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Ok(projects);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProject(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }


        [HttpPost]
        //[Authorize]
        public async Task<ActionResult<ProjectDto>> CreateProject([FromForm] CreateProjectDto createDto) 
        {
            var newProject = await _projectService.CreateProjectAsync(createDto);
            return CreatedAtAction(nameof(GetProject), new { id = newProject.Id }, newProject);
        }


        [HttpPut("{id}")]
        //[Authorize]
        public async Task<IActionResult> UpdateProject(int id, [FromForm] UpdateProjectDto updateDto) 
        {
            try
            {
                await _projectService.UpdateProjectAsync(id, updateDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        //[Authorize]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                await _projectService.DeleteProjectAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); 
            }

            return NoContent(); 
        }
    }
}