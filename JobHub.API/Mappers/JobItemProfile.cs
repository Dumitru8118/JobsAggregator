using AutoMapper;
using JobHub.API.Models;
using JobHub.API.Models.Database;
using JobHub.API.Models.Dtos.Response;

namespace JobHub.API.Mappers
{
    public class JobItemProfile : Profile
	{
		public JobItemProfile()
		{
			CreateMap<Job, JobItemResponse>();
				//.ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
				//.ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
				//.ForMember(dest => dest.JobName, opt => opt.MapFrom(src => src.JobName))
				//.ForMember(dest => dest.DatePosted, opt => opt.MapFrom(src => src.DatePosted))
				//.ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.JobPage.Salary))
				//.ForMember(dest => dest.City, opt => opt.MapFrom(src => src.JobPage.City))
				//.ForMember(dest => dest.JobType, opt => opt.MapFrom(src => src.JobPage.JobType))
				//.ForMember(dest => dest.ExpertiseLvl, opt => opt.MapFrom(src => src.JobPage.ExpertiseLvl))
				//.ForMember(dest => dest.Industry, opt => opt.MapFrom(src => src.JobPage.Industry))
				//.ForMember(dest => dest.QualificationLvl, opt => opt.MapFrom(src => src.JobPage.QualificationLvl))
				//.ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.JobPage.Department))
				//.ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.JobPage.Language));
		}
	}
}
