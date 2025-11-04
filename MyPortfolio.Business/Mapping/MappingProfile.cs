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

            CreateMap<Blog, BlogDto>().ReverseMap();

            CreateMap<CreateBlogDto, Blog>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.PublishedDate, opt => opt.Ignore()); 

            CreateMap<UpdateBlogDto, Blog>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            CreateMap<Feedback, FeedbackDto>().ReverseMap();
            CreateMap<CreateFeedbackDto, Feedback>()
                .ForMember(dest => dest.SubmittedDate, opt => opt.Ignore());
        }
    }
}