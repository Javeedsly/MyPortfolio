using Microsoft.AspNetCore.Http;

namespace MyPortfolio.Core.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string ProjectUrl { get; set; }
        public string? SourceCodeUrl { get; set; }
        public List<string> Technologies { get; set; } = new();
    }

    public class CreateProjectDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ProjectUrl { get; set; }
        public string? SourceCodeUrl { get; set; }
        public List<string> Technologies { get; set; } = new();
    }

    public class UpdateProjectDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? SourceCodeUrl { get; set; }
        public List<string> Technologies { get; set; } = new();
    }
}