using AutoMapper;

namespace ContainerManager.Infrastructure.Entities
{
	public class DataMappingProfile : Profile
	{
		public DataMappingProfile()
		{
			CreateMap<Application, Domain.Models.Application>().ReverseMap();
			CreateMap<Machine, Domain.Models.Machine>().ReverseMap();
			CreateMap<User, Domain.Models.User>().ReverseMap();
		}
	}
}
