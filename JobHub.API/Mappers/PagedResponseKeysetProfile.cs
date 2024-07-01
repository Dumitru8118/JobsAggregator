using AutoMapper;
using JobHub.API.Models;
using JobHub.API.Models.Dtos.Response;

namespace JobHub.API.Mappers
{
    public class PagedResponseKeysetProfile : Profile
	{
		public PagedResponseKeysetProfile()
		{
			CreateMap(typeof(PagedResponseKeyset<>), typeof(PagedResponseKeysetDto<>));
		}
	}
}
