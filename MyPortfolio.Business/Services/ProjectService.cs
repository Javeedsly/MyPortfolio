using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MyPortfolio.Core.DTOs;
using MyPortfolio.Core.Entities;
using MyPortfolio.Core.Interfaces;

namespace MyPortfolio.Business.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
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

            string imageUrl = await SaveImageAsync(createProjectDto.ImageFile);


            var project = _mapper.Map<Project>(createProjectDto);


            project.ImageUrl = imageUrl;

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

            if (updateProjectDto.ImageFile != null)
            {
                // if (!string.IsNullOrEmpty(project.ImageUrl))
                // {
                //     var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, project.ImageUrl.TrimStart('/'));
                //     if (File.Exists(oldImagePath))
                //     {
                //         File.Delete(oldImagePath);
                //     }
                // }

                // Yeni şəkli saxla və URL-i yenilə
                project.ImageUrl = await SaveImageAsync(updateProjectDto.ImageFile);
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
            // if (!string.IsNullOrEmpty(project.ImageUrl))
            // {
            //     var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, project.ImageUrl.TrimStart('/'));
            //     if (File.Exists(imagePath))
            //     {
            //         File.Delete(imagePath);
            //     }
            // }

            _unitOfWork.Projects.Delete(project);
            await _unitOfWork.CompleteAsync();
        }
        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("Image was not provided.");
            }

            var webRootPath = _webHostEnvironment.WebRootPath;
            if (string.IsNullOrEmpty(webRootPath))
            {
                webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var uploadPath = Path.Combine(webRootPath, "uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            var filePath = Path.Combine(uploadPath, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/uploads/{uniqueFileName}";
        }
    }
}