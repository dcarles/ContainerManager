using AutoMapper;
using ContainerManager.Domain.Commands;

namespace ContainerManager.API.ViewModels
{
	public class ApiMappingProfile : Profile
	{
		public ApiMappingProfile()
		{
			CreateMap<UserRequest, CreateUserCommand>();
			CreateMap<MachineRequest, CreateMachineCommand>();
			CreateMap<ApplicationRequest, CreateApplicationCommand>();
			CreateMap<ApplicationRequest, UpdateApplicationCommand>();
		}
	}
}
