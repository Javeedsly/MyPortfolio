using MyPortfolio.Core.DTOs;

namespace MyPortfolio.Core.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectDto> GetProjectByIdAsync(int id);
        Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
        Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto);
        Task UpdateProjectAsync(int id, UpdateProjectDto updateProjectDto);
        Task DeleteProjectAsync(int id);
    }
}