using AutoMapper;
using JobHub.API.Models;
using JobHub.API.Models.Database;
using JobHub.API.Models.Dtos.Request;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium.DevTools;

namespace JobHub.API.Mappers
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<LoginInputModel, User>().ReverseMap();
			CreateMap<RegisterInputModel, User>().ReverseMap();
			CreateMap<JobInputModel, Job>().ReverseMap();
			CreateMap<UserJobInputModel, UserJob>().ReverseMap();
		}
	}
}
