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

            CreateMap<CreateProjectDto, Project>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            CreateMap<Project, CreateProjectDto>();

            CreateMap<UpdateProjectDto, Project>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            CreateMap<Project, UpdateProjectDto>();
        }
    }
}