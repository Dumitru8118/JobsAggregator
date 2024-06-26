using AutoMapper;
using JobHub.API.Dtos.Response;
using JobHub.API.Models;

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
