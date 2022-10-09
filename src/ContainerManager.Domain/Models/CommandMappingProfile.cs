using AutoMapper;
using ContainerManager.Domain.Commands;
using ContainerManager.Domain.Models;

namespace ContainerManager.API.ViewModels
{
	public class CommandMappingProfile : Profile
	{
		public CommandMappingProfile()
		{
			CreateMap<CreateUserCommand, User>();
			CreateMap<CreateMachineCommand, Machine>();
			CreateMap<CreateApplicationCommand, Application>();
		}
	}
}
