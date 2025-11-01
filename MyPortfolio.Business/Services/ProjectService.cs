using AutoMapper;
using MyPortfolio.Core.DTOs;
using MyPortfolio.Core.Entities;
using MyPortfolio.Core.Interfaces;

namespace MyPortfolio.Business.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProjectDto> GetProjectByIdAsync(int id)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id);
            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        {
            var projects = await _unitOfWork.Projects.GetAllAsync();
            return _mapper.Map<IEnumerable<ProjectDto>>(projects);
        }

        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto)
        {
            var project = _mapper.Map<Project>(createProjectDto);

            await _unitOfWork.Projects.AddAsync(project);
            await _unitOfWork.CompleteAsync(); 
            return _mapper.Map<ProjectDto>(project);
        }

        public async Task UpdateProjectAsync(int id, UpdateProjectDto updateProjectDto)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id);
            if (project == null)
            {
                throw new KeyNotFoundException("Project Not Found!");
            }

            _mapper.Map(updateProjectDto, project);

            _unitOfWork.Projects.Update(project);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id);
            if (project == null)
            {
                throw new KeyNotFoundException("Project Not Found!");
            }

            _unitOfWork.Projects.Delete(project);
            await _unitOfWork.CompleteAsync();
        }
    }
}