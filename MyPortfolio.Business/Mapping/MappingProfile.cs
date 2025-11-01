using AutoMapper;
using MyPortfolio.Core.DTOs;
using MyPortfolio.Core.Entities;

namespace MyPortfolio.Business.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectDto>().ReverseMap();

            CreateMap<Project, CreateProjectDto>().ReverseMap();

            CreateMap<Project, UpdateProjectDto>().ReverseMap();
        }
    }
}